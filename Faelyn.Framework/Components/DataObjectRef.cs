using System;
using System.IO;
using Faelyn.Framework.Events;
using Faelyn.Framework.Interfaces;

namespace Faelyn.Framework.Components
{
    /// <summary>
    /// Actual implementation of IDataObjectObserver
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class DataObjectRef<TState> : NotifyStateChanges, IDataObjectRef<TState>
    {
        #region Fields
        
        private TState _currentData;

        #endregion Fields

        #region Properties
        
        /// <summary>
        /// This property contains the actual data.
        /// </summary>
        public TState CurrentData
        {
            get => _currentData;
            set => SetProperty(ref _currentData, value);
        }

        #endregion Properties

        #region Life cycle

        /// <summary>
        /// Construct the object
        /// </summary>
        public DataObjectRef(TState data = default)
        {
            CurrentData = data;
        }

        #endregion Life cycle
    }
}