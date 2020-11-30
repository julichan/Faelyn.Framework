using System.Windows;
using System.Windows.Controls;
using Faelyn.Framework.Interfaces;
using Faelyn.Framework.WPF.Components;

namespace Faelyn.Framework.WPF.Helpers
{
    /// <summary>
    /// Helper to manage encrypted password in xaml
    /// </summary>
    public static class PasswordHelper
    {
        #region Constants

        /// <summary>
        /// The converter to use to provide an encrypted password.
        /// </summary>
        private static readonly DependencyProperty PasswordBoxHandlerProperty = DependencyProperty.RegisterAttached(
            "PasswordBoxHandler",
            typeof(PasswordBoxHandler),
            typeof(PasswordHelper),
            new FrameworkPropertyMetadata(null, OnPasswordBoxHandlerPropertyChanged));
        

        #endregion Constants
        
        #region Methods

        /// <summary>
        /// Retrieves the PasswordBox's handler
        /// </summary>
        public static PasswordBoxHandler GetPasswordBoxHandler(DependencyObject dp)
        {
            return (PasswordBoxHandler) dp.GetValue(PasswordBoxHandlerProperty);
        }

        public static void SetPasswordBoxHandler(DependencyObject dp, PasswordBoxHandler newHandler)
        {
            if (dp is PasswordBox passwordBox)
            {
                var oldHandler = GetPasswordBoxHandler(dp);
                if (oldHandler != null)
                {
                    oldHandler.Unregister();
                    oldHandler.SetPasswordBox(null);
                }

                dp.SetValue(PasswordBoxHandlerProperty, newHandler);
                
                if (newHandler != null)
                {
                    newHandler.Register();
                    newHandler.SetPasswordBox(passwordBox);
                }
            }
        }

        private static void OnPasswordBoxHandlerPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SetPasswordBoxHandler(sender, e.NewValue as PasswordBoxHandler);
        }

        #endregion Methods
    }
}
