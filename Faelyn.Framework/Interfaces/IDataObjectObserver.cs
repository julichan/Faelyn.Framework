using System;
using System.IO;
using Faelyn.Framework.Events;
using Faelyn.Framework.Interfaces;

namespace Faelyn.Framework.Interfaces
{
    /// <typeparam name="TState"></typeparam>
    public interface IDataObjectObserver<out TState> : INotifyStateChanges, IReceiveStateChanges
    {
        #region Properties
        
        /// <summary>
        /// This property tells whether or not the data has been modified
        /// </summary>
        bool HasStateChanged { get; }
        
        #endregion Properties
    }
}