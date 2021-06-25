using System;
using System.Windows.Input;
using Faelyn.Framework.WPF.Interfaces;

namespace Faelyn.Framework.WPF.Components
{
    /// <summary>
    /// UI Base command
    /// </summary>
    public class Command : ICommandBridge, ICommandRelay, ICommand
    {
        #region Fields

        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _execute;

        #endregion Fields

        #region Life cycle

        private Command(Action<object> execute, Func<object, bool> canExecute = null, bool bindToCommandManager = false)
        {
            _execute = execute;
            _canExecute = canExecute;
            
            if (bindToCommandManager)
            {
                BindCommandManager();
            }
        }

        public static ICommandBridge CreateCommandBridge(Action<object> execute, Func<object, bool> canExecute = null,
            bool bindToCommandManager = false)
        {
            return new Command(execute, canExecute, bindToCommandManager);
        }
        
        public static ICommandRelay CreateCommandRelay(Action<object> execute, Func<object, bool> canExecute = null)
        {
            return new Command(execute, canExecute, false);
        }

        #endregion Life cycle

        #region Interface ICommand

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? _execute != null;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }

        #endregion Interface ICommand
        
        #region Interface ICommandBridge

        public void RaiseCanExecuteChanged(object sender = null, EventArgs e = null)
        {
            CanExecuteChanged?.Invoke(sender ?? this, e ?? EventArgs.Empty);
        }
        
        #endregion Interface ICommandBridge
        
        #region Interface ICommandControl

        public void BindCommandManager()
        {
            CommandManager.RequerySuggested += RaiseCanExecuteChanged;
        }

        public void UnbindCommandManager()
        {
            CommandManager.RequerySuggested -= RaiseCanExecuteChanged;
        }

        #endregion Interface ICommandControl
    }
}
