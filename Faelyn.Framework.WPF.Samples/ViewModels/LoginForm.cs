using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Faelyn.Framework.Components;
using Faelyn.Framework.Interfaces;
using Faelyn.Framework.Windows.Components;
using Faelyn.Framework.WPF.Components;
using Faelyn.Framework.WPF.Interfaces;

namespace Faelyn.Framework.WPF.Samples.ViewModels
{
    public class LoginForm : NotifyPropertyChanges
    {
        private string _username;
        public string Username
        {
            get => _username;
            set { SetProperty(ref _username, value); 
                LoginCommand?.RaiseCanExecuteChanged(); }
        }

        private PasswordBoxHandler _passwordBoxHandler;
        public PasswordBoxHandler PasswordBoxHandler
        {
            get => _passwordBoxHandler;
            set { SetProperty(ref _passwordBoxHandler, value);
                EncryptedPassword = _passwordBoxHandler.ToString();
            }
        }
        
        private string _encryptedPasword;
        public string EncryptedPassword
        {
            get => _encryptedPasword;
            set { SetProperty(ref _encryptedPasword, value); 
                LoginCommand?.RaiseCanExecuteChanged(); }
        }

        private ICommandRelay _loginCommand = null;
        public ICommandRelay LoginCommand
        {
            get => _loginCommand;
            set => SetProperty(ref _loginCommand, value);
        }


        public LoginForm()
        {
            var dataGuard = new ProtectedDataGuard(
                DataProtectionScope.CurrentUser,
                Encoding.Unicode,
                () =>
                {
                    var random = new Random();
                    var length = random.Next(16, 32);
                    var entropy = new List<Byte>();
                    for (int i = 0; i < length; ++i)
                    {
                        var c = Convert.ToChar(random.Next(UInt16.MinValue, UInt16.MaxValue));
                        entropy.AddRange(BitConverter.GetBytes(c));
                    }

                    return entropy.ToArray();
                });
            PasswordBoxHandler = new PasswordBoxHandler(dataGuard);
            PasswordBoxHandler.ModelDataGuard.OnSensibleDataChanged += OnSensibleDataChanged;
            
            LoginCommand = Command.CreateCommandRelay(ExecuteLogin, CanExecuteLogin);
        }

        private void OnSensibleDataChanged(object sender, EventArgs e)
        {
            if (sender is ISensibleDataGuard dataGuard)
            {
                EncryptedPassword = dataGuard.ToString();
            }
        }

        private bool CanExecuteLogin(object arg)
        {
            return !string.IsNullOrEmpty(Username) 
                   && !string.IsNullOrEmpty(PasswordBoxHandler?.ToString());
        }

        private void ExecuteLogin(object obj)
        {
            MessageBox.Show("Access granted!");
        }
    }
}