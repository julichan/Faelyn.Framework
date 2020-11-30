using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Faelyn.Framework.Helpers;
using Faelyn.Framework.Interfaces;

namespace Faelyn.Framework.Services
{
    /// <summary>
    /// This object is an implementation of IMessagingService using weak references
    /// </summary>
    public class MessagingService : IMessagingService
    {
        #region Fields

        private ReaderWriterLockSlim _syncLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        private Dictionary<Type, List<WeakReference>> _registrations = new Dictionary<Type, List<WeakReference>>();

        #endregion

        #region Methods

        public void SendMessage<TMessage>(TMessage message)
        {
            Type messageType = typeof(TMessage);
            WeakReference[] references = null;

            _syncLock.ReadLockedOperation(() =>
            {
                if (_registrations.TryGetValue(messageType, out var referenceList))
                {
                    references = referenceList.ToArray();
                }
            });
            

            if (references != null)
            {
                foreach (var reference in references)
                {
                    if (reference.GetTargetSafe() is IMessagingListener<TMessage> listener)
                    {
                        listener.ReceiveMessage(message);
                    }
                    else
                    {
                        CleanReference(messageType, reference);
                    }
                }
            }

        }

        public void Register<TMessage>(IMessagingListener<TMessage> listener)
        {
            Type messageType = typeof(TMessage);
            List<WeakReference> references = null;

            _syncLock.WriteLockedOperation(() =>
            {
                if (!_registrations.TryGetValue(messageType, out references))
                {
                    references = new List<WeakReference>();
                    _registrations.Add(messageType, references);
                }

                bool isNotRegistered = true;
                foreach (var reference in references.ToArray())
                {
                    var target = reference.GetTargetSafe();

                    if (target == null)
                    {
                        CleanReference(messageType, reference);
                    }
                    else if (ReferenceEquals(target, listener))
                    {
                        isNotRegistered = false;
                    }
                }

                if (isNotRegistered)
                {
                    references.Add(new WeakReference(listener));
                }
            });
           
        }

        public void Unregister<TMessage>(IMessagingListener<TMessage> listener)
        {
            Type messageType = typeof(TMessage);
            WeakReference[] references = null;

            _syncLock.ReadLockedOperation(() =>
            {
                if (_registrations.TryGetValue(messageType, out var referenceList))
                {
                    references = referenceList.ToArray();
                }
            });

            if (references != null)
            {
                foreach (var reference in references)
                {
                    var target = reference.GetTargetSafe();
                    if (target == null || ReferenceEquals(target, listener))
                    {
                        CleanReference(messageType, reference);
                    }
                }
            }
        }

        public void CleanRegistrations()
        {
            foreach (var registration in _registrations)
            {
                foreach (var reference in registration.Value.ToArray())
                {
                    if (reference.GetTargetSafe() == null)
                    {
                        CleanReference(registration.Key, reference);
                    }
                }
            }
        }

        private void CleanReference(Type key, WeakReference reference)
        {
            _syncLock.WriteLockedOperation(() =>
            {
                if (_registrations.TryGetValue(key, out var references))
                {
                    if (references.Remove(reference) && !references.Any())
                    {
                        _registrations.Remove(key);
                    }
                }
            });
        }
        #endregion
    }
}