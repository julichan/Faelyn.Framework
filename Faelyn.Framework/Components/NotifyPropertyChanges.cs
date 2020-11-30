using System.ComponentModel;
using System.Runtime.CompilerServices;
using Faelyn.Framework.Events;

namespace Faelyn.Framework.Components
{
     public abstract class NotifyPropertyChanges : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Interface INotifyPropertyChanged

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Interface INotifyPropertyChanged

        #region Interface INotifyPropertyChanging

        /// <inheritdoc/>
        public event PropertyChangingEventHandler PropertyChanging;

        #endregion Interface INotifyPropertyChanging

        #region Protected Methods

        /// <summary>
        /// Allows to raise the PropertyChanged event without SetProperty
        /// Also overloads the event arg as CustomPropertyChangedEventArgs.
        /// </summary>
        public void RaisePropertyChanged(string propertyName, bool reallyChanged = false)
        {
            OnPropertyChanged(propertyName, reallyChanged);
        }

        /// <summary>
        /// Allows to raise the PropertyChanging event without SetProperty. Also overloads the event
        /// arg as CustomPropertyChangingEventArgs.
        /// </summary>
        public void RaisePropertyChanging(string propertyName, bool reallyChanging = false)
        {
            OnPropertyChanging(propertyName, reallyChanging);
        }

        /// <summary>
        /// Execute OnPropertyChanging and OnPropertyChanged whenever a property changed. Does
        /// nothing if the old and new values are equals.
        /// </summary>
        protected bool SetProperty<TField>(ref TField field, TField value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, value))
            {
                OnPropertyChanging(propertyName);
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Notifies that a property has changed
        /// </summary>
        private void OnPropertyChanged(string propertyName = null, bool updated = true)
        {
            PropertyChanged?.Invoke(this, new UpdatePropertyChangedEventArgs(propertyName, updated));
        }

        /// <summary>
        /// Notifies that a property is changing.
        /// </summary>
        private void OnPropertyChanging(string propertyName = null, bool updating = true)
        {
            PropertyChanging?.Invoke(this, new UpdatePropertyChangingEventArgs(propertyName, updating));
        }

        #endregion Private Methods
    }
}