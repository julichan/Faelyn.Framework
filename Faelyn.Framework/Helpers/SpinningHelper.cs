using System;
using System.Threading;

namespace Faelyn.Framework.Helpers
{
    public static class SpinningHelper
    {
        public static void ReadLockedOperation(this ReaderWriterLockSlim locker, Action action)
        {
            if (action == null)
                return;

            locker.EnterReadLock();
            try
            {
                action();
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        public static TReturn ReadLockedOperation<TReturn>(this ReaderWriterLockSlim locker, Func<TReturn> action)
        {
            if (action == null)
                return default(TReturn);

            locker.EnterReadLock();
            try
            {
                return action();
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        public static void UpgradeableReadLockedOperation(this ReaderWriterLockSlim locker, Action action)
        {
            if (action == null)
                return;

            locker.EnterUpgradeableReadLock();
            try
            {
                action();
            }
            finally
            {
                locker.ExitUpgradeableReadLock();
            }
        }

        public static TReturn UpgradeableReadLockedOperation<TReturn>(this ReaderWriterLockSlim locker, Func<TReturn> action)
        {
            if (action == null)
                return default(TReturn);

            locker.EnterUpgradeableReadLock();
            try
            {
                return action();
            }
            finally
            {
                locker.ExitUpgradeableReadLock();
            }
        }

        public static void WriteLockedOperation(this ReaderWriterLockSlim locker, Action action)
        {
            if (action == null)
                return;

            locker.EnterWriteLock();
            try
            {
                action();
            }
            finally
            {
                locker.ExitWriteLock();
            }

        }

        public static TReturn WriteLockedOperation<TReturn>(this ReaderWriterLockSlim locker, Func<TReturn> action)
        {
            if (action == null)
                return default(TReturn);

            locker.EnterWriteLock();
            try
            {
                return action();
            }
            finally
            {
                locker.ExitWriteLock();
            }

        }
    }
}