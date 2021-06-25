using System.Windows.Input;

namespace Faelyn.Framework.WPF.Interfaces
{
    /// <summary>
    /// An interface extending the ICommand interface
    /// </summary>
    public interface ICommandBridge : ICommand
    {
        #region Methods

        void BindCommandManager();

        void UnbindCommandManager();

        #endregion Methods
    }
}
