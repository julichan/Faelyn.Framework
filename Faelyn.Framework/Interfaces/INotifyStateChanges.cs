using System;
using Faelyn.Framework.Events;

namespace Faelyn.Framework.Interfaces
{
    /// <summary>
    /// This interface defines how an object should notify changes of its state
    /// </summary>
    public interface INotifyStateChanges
    {
        #region Methods

        /// <summary>
        /// The event executed when the state changed
        /// </summary>
        event EventHandler<StateChangedEventArgs> OnStateChanged;

        #endregion Methods
    }
}