using System;

namespace Faelyn.Framework.Events
{
    public class StateChangedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Property indicating something changed
        /// </summary>
        public bool HasStateChanged { get; }

        #endregion Properties

        #region Life cycle

        /// <summary>
        /// Constructor
        /// </summary>
        public StateChangedEventArgs(bool hasStateChanged)
        {
            HasStateChanged = hasStateChanged;
        }

        #endregion Life cycle
    }
}