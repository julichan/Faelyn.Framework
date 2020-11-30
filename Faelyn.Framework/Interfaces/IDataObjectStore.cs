using System;
using System.IO;
using Faelyn.Framework.Events;
using Faelyn.Framework.Interfaces;

namespace Faelyn.Framework.Interfaces
{
    /// <typeparam name="TState"></typeparam>
    public interface IDataObjectStore<out TState> : INotifyStateChanges, IReceiveStateChanges
    {
        #region Properties
        
        /// <summary>
        /// This property contains the actual data.
        /// </summary>
        TState CurrentData { get; }

        #endregion Properties
    }
}