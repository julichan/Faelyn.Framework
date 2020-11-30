using System;
using System.Windows.Input;

namespace Faelyn.Framework.WPF.Interfaces
{
    /// <summary>
    /// An interface extending the ICommand interface
    /// </summary>
    public interface ICommandBridge : ICommand
    {
        #region Methods

        void RaiseCanExecuteChanged(object sender = null, EventArgs e = null);

        #endregion Methods
    }
}
