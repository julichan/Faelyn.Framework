using System;
using System.Windows;
using System.Windows.Controls;
using Faelyn.Framework.Interfaces;

namespace Faelyn.Framework.WPF.Components
{
    public sealed class PasswordBoxHandler
    {
        private bool _isUpdating;
        private ISensibleDataGuard<PasswordBox> _passwordBoxDataGuard;
        public ISensibleDataGuard ModelDataGuard { get; }

        public PasswordBoxHandler(ISensibleDataGuard modelDataGuard)
        {
            ModelDataGuard = modelDataGuard ?? throw new ArgumentNullException(nameof(modelDataGuard));
        }

        public override string ToString()
        {
            return ModelDataGuard.ToString();
        }

        internal void SetPasswordBox(PasswordBox passwordBox)
        {
            _passwordBoxDataGuard?.ClearData();
            if (passwordBox != null)
            {
                _passwordBoxDataGuard = new PasswordBoxDataGuard(passwordBox);
            }
            else
            {
                _passwordBoxDataGuard = null;
            }
            OnModelDataChanged(this, EventArgs.Empty);
        }
        
        internal void Register()
        {
            var passwordBox = _passwordBoxDataGuard?.EncryptedData;
            if (passwordBox != null)
            {
                passwordBox.PasswordChanged += OnPasswordBoxDataChanged;
                ModelDataGuard.OnSensibleDataChanged += OnModelDataChanged;
            }
        }

        internal void Unregister()
        {
            var passwordBox = _passwordBoxDataGuard?.EncryptedData;
            if (passwordBox != null)
            {
                passwordBox.PasswordChanged -= OnPasswordBoxDataChanged;
                ModelDataGuard.OnSensibleDataChanged -= OnModelDataChanged;
            }
        }
        
        private void OnModelDataChanged(object sender, EventArgs eventArgs)
        {
            if (_passwordBoxDataGuard != null && !_isUpdating)
            {
                Unregister();
                _isUpdating = true;
                ModelDataGuard.ProtectStringAction(clearPassword =>
                {
                    _passwordBoxDataGuard.SetStringData(() => clearPassword);
                });
                _isUpdating = false;
                Register();
            }
        }

        private void OnPasswordBoxDataChanged(object sender, RoutedEventArgs e)
        {
            if (!_isUpdating)
            {
                _passwordBoxDataGuard.ProtectStringAction(clearPassword =>
                {
                    ModelDataGuard.SetStringData(() => clearPassword);
                });
            }
        }
    }
}