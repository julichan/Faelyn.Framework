using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Faelyn.Framework.Helpers;
using Faelyn.Framework.Interfaces;
using Faelyn.Framework.Security.Helpers;

namespace Faelyn.Framework.Security.Components
{
    public sealed class SecureStringDataGuard : ISensibleDataGuard<SecureString>, IDisposable
    {
        #region Fields
        
        private readonly Encoding _encoding;

        #endregion
        
        #region Properties

        public SecureString EncryptedData { get; private set; }
        
        public event EventHandler OnSensibleDataChanged;
        
        #endregion
        
        #region Life cycle

        public SecureStringDataGuard(Encoding encoding)
        {
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            EncryptedData = new SecureString();
        }

        public void Dispose()
        {
            EncryptedData.Dispose();
        }
        
        #endregion
        
        #region Methods

        public void ProtectRawAction(Action<byte[]> action)
        {
            var iPtr = IntPtr.Zero;
            byte[] iAry = new byte[EncryptedData.Length * 2];
            try
            {
                iPtr = Marshal.SecureStringToGlobalAllocUnicode(EncryptedData);
                for (int i=0; i < EncryptedData.Length; i++)
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
                iPtr = Marshal.SecureStringToBSTR(EncryptedData);
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
            byte[] iAry = new byte[EncryptedData.Length * 2];
            try
            {
                iPtr = Marshal.SecureStringToGlobalAllocUnicode(EncryptedData);
                for (int i=0; i < EncryptedData.Length; i++)
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
                iPtr = Marshal.SecureStringToBSTR(EncryptedData);
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
                    return _encoding.GetString(iAry);
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
                EncryptedData.Clear();
                iStr = input();
                iStr.ForEach(c => EncryptedData.AppendChar(c));
            }
            finally
            {
                MemoryHelper.OverwriteString(ref iStr);
            }
            OnSensibleDataChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ClearData()
        {
            EncryptedData.Clear();
            OnSensibleDataChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}