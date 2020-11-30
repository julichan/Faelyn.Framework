using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Controls;
using Faelyn.Framework.Helpers;
using Faelyn.Framework.Interfaces;
using Faelyn.Framework.Security.Helpers;

namespace Faelyn.Framework.WPF.Components
{
    internal sealed class PasswordBoxDataGuard : ISensibleDataGuard<PasswordBox>
    {
        #region Fields
        
        private readonly WeakReference _passwordBoxRef;

        #endregion
        
        #region Properties

        public PasswordBox EncryptedData
        {
            get => _passwordBoxRef?.GetTargetSafe<PasswordBox>();
        }
        
        public event EventHandler OnSensibleDataChanged;
        
        #endregion
        
        #region Life cycle

        public PasswordBoxDataGuard(PasswordBox passwordBox)
        {
            _passwordBoxRef = new WeakReference(passwordBox);
        }

        #endregion
        
        #region Methods

        public void ProtectRawAction(Action<byte[]> action)
        {
            var iPtr = IntPtr.Zero;
            byte[] iAry = new byte[EncryptedData.SecurePassword.Length * 2];
            try
            {
                iPtr = Marshal.SecureStringToGlobalAllocUnicode(EncryptedData.SecurePassword);
                for (int i=0; i < EncryptedData.SecurePassword.Length; i++)
                {
                    var nb = BitConverter.GetBytes(Marshal.ReadInt16(iPtr, i * 2));
                    iAry[i * 2] = nb[0];
                    iAry[i * 2 + 1] = nb[1];
                }
                
                action(iAry);
            }
            finally
            {
                MemoryHelper.OverwriteBytes(ref iAry);

                if (iPtr == IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(iPtr);
            }
        }

        public void ProtectStringAction(Action<string> action)
        {
            var iPtr = IntPtr.Zero;
            var iStr = String.Empty;
            try
            {
                iPtr = Marshal.SecureStringToBSTR(EncryptedData.SecurePassword);
                iStr = Marshal.PtrToStringBSTR(iPtr);

                action(iStr);
            }
            finally
            {
                MemoryHelper.OverwriteString(ref iStr);

                if (iPtr == IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(iPtr);
            }
        }

        public TReturn ProtectRawFunction<TReturn>(Func<byte[], TReturn> func)
        {
            var iPtr = IntPtr.Zero;
            byte[] iAry = new byte[EncryptedData.SecurePassword.Length * 2];
            try
            {
                iPtr = Marshal.SecureStringToGlobalAllocUnicode(EncryptedData.SecurePassword);
                for (int i=0; i < EncryptedData.SecurePassword.Length; i++)
                {
                    var nb = BitConverter.GetBytes(Marshal.ReadInt16(iPtr, i * 2));
                    iAry[i * 2] = nb[0];
                    iAry[i * 2 + 1] = nb[1];
                }
                
                return func(iAry);
            }
            finally
            {
                MemoryHelper.OverwriteBytes(ref iAry);

                if (iPtr == IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(iPtr);
            }
        }

        public TReturn ProtectStringFunction<TReturn>(Func<string, TReturn> func)
        {
            var iPtr = IntPtr.Zero;
            var iStr = String.Empty;
            try
            {
                iPtr = Marshal.SecureStringToBSTR(EncryptedData.SecurePassword);
                iStr = Marshal.PtrToStringBSTR(iPtr);

                return func(iStr);
            }
            finally
            {
                MemoryHelper.OverwriteString(ref iStr);

                if (iPtr == IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(iPtr);
            }
        }

        public void SetRawData(Func<byte[]> input)
        {
            byte[] iAry = null;
            try
            {
                SetStringData(() =>
                {
                    iAry = input();
                    return Encoding.UTF8.GetString(iAry);
                });
            }
            finally
            {
                MemoryHelper.OverwriteBytes(ref iAry);
            }
        }

        public void SetStringData(Func<string> input)
        {
            string iStr = null;
            try
            {
                EncryptedData.SecurePassword.Clear();
                iStr = input();
                iStr.ForEach(c => EncryptedData.SecurePassword.AppendChar(c));
            }
            finally
            {
                MemoryHelper.OverwriteString(ref iStr);
            }
            OnSensibleDataChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ClearData()
        {
            EncryptedData.SecurePassword.Clear();
            OnSensibleDataChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}