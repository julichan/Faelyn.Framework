﻿using System;
using System.Security.Cryptography;
using System.Text;
using Faelyn.Framework.Interfaces;
using Faelyn.Framework.Security.Helpers;

namespace Faelyn.Framework.Security.Components
{
    public sealed class ProtectedDataGuard : ISensibleDataGuard<byte[]>
    {
        #region Fields

        private readonly DataProtectionScope _scope;
        private readonly Encoding _encoding;
        private readonly SecureStringDataGuard _protectedEntropy;

        private byte[] _encryptedData;
        
        #endregion
        
        #region Properties

        public byte[] EncryptedData { get => _encryptedData; }
        
        public event EventHandler OnSensibleDataChanged;
        
        #endregion
        
        #region Life cycle

        public ProtectedDataGuard(DataProtectionScope scope, Encoding encoding)
        {
            _scope = scope;
            _encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            _protectedEntropy = null;
        }
        
        public ProtectedDataGuard(DataProtectionScope scope, Encoding encoding, Func<byte[]> optionalEntropy)
            : this(scope, encoding)
        {
            if (optionalEntropy == null) throw new ArgumentNullException(nameof(optionalEntropy));
            _protectedEntropy = new SecureStringDataGuard(encoding);
            _protectedEntropy.SetRawData(optionalEntropy);
        }
        
        public ProtectedDataGuard(DataProtectionScope scope, Encoding encoding, Func<string> optionalEntropy)
            : this(scope, encoding)
        {
            if (optionalEntropy == null) throw new ArgumentNullException(nameof(optionalEntropy));
            _protectedEntropy = new SecureStringDataGuard(encoding);
            _protectedEntropy.SetStringData(optionalEntropy);
        }

        #endregion
        
        #region Methods

        public void ProtectRawAction(Action<byte[]> action)
        {
            if (!IsInitialized()) return;
            byte[] iAry = null;
            try
            {
                _protectedEntropy.ProtectRawAction((entropy) =>
                {
                    iAry = ProtectedData.Unprotect(EncryptedData, entropy, _scope);
                    action(iAry);
                });
            }
            finally
            {
                MemoryHelper.OverwriteBytes(ref iAry);
            }
        }

        public void ProtectStringAction(Action<string> action)
        {
            if (!IsInitialized()) return;
            string iStr = null;
            try
            {
                ProtectRawAction((iAry) =>
                {
                    iStr = Convert.ToBase64String(iAry);
                    action(iStr);
                });
            }
            finally
            {
                MemoryHelper.OverwriteString(ref iStr);
            }
        }

        public TReturn ProtectRawFunction<TReturn>(Func<byte[], TReturn> func)
        {
            if (!IsInitialized()) return default(TReturn);
            byte[] iAry = null;
            try
            {
                return _protectedEntropy.ProtectRawFunction((entropy) =>
                {
                    iAry = ProtectedData.Unprotect(EncryptedData, entropy, _scope);
                    return func(iAry);
                });
            }
            finally
            {
                MemoryHelper.OverwriteBytes(ref iAry);
            }
            
        }

        public TReturn ProtectStringFunction<TReturn>(Func<string, TReturn> func)
        {
            if (!IsInitialized()) return default(TReturn);
            string iStr = null;
            try
            {
                return ProtectRawFunction<TReturn>((iAry) =>
                {
                    iStr = _encoding.GetString(iAry);
                    return func(iStr);
                });
            }
            finally
            {
                MemoryHelper.OverwriteString(ref iStr);
            }
        }

        public void SetRawData(Func<byte[]> input)
        {
            byte[] iAry = null;
            try
            {
                _protectedEntropy.ProtectRawAction((entropy) =>
                {
                    iAry = input();
                    _encryptedData = ProtectedData.Protect(iAry, entropy, _scope);
                });
            }
            finally
            {
                MemoryHelper.OverwriteBytes(ref iAry);
            }
            OnSensibleDataChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SetStringData(Func<string> input)
        {
            string iStr = null;
            try
            {
                SetRawData(() =>
                {
                    iStr = input();
                    return _encoding.GetBytes(iStr);
                });
            }
            finally
            {
                MemoryHelper.OverwriteString(ref iStr);
            }
        }

        public void ClearData()
        {
            MemoryHelper.OverwriteBytes(ref _encryptedData);
            _encryptedData = null;
            OnSensibleDataChanged?.Invoke(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            if (!IsInitialized())
                return string.Empty;
            return Convert.ToBase64String(EncryptedData);
        }

        private bool IsInitialized()
        {
            return EncryptedData != null && EncryptedData.Length > 0;
        }
        
        #endregion
    }
}