using System.Collections.Specialized;
using Faelyn.Framework.Events;

namespace Faelyn.Framework.Interfaces
{
    /// <summary>
    /// This interface defines how a parent state object receive state changes from its children
    /// </summary>
    public interface IReceiveStateChanges
    {
        #region Methods

        /// <summary>
        /// This method is executed when a collection in properties changed (collections within
        /// collections included)
        /// </summary>
        void ReceiveCollectionStateChanged(object sender, NotifyCollectionChangedEventArgs args);

        /// <summary>
        /// This method is executed when a property changed
        /// </summary>
        void ReceiveStateChanged(object sender, StateChangedEventArgs args);

        #endregion Methods
    }
}