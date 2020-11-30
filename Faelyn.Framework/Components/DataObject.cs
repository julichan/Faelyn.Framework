using System;
using Faelyn.Framework.Events;
using Faelyn.Framework.Interfaces;

namespace Faelyn.Framework.Components
{
    /// <summary>
    /// Actual implementation of IDataManager
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class DataObject<TState> : NotifyStateChanges, IDataObjectObserver<TState>
    {
        #region Fields

        private readonly IDataObjectSource<TState> _source;
        private readonly DataObjectStore<TState> _store;
        private bool _hasStateChanged = true;

        #endregion
        
        #region Properties
        
        /// <summary>
        /// This property contains the data source.
        /// </summary>
        public IDataObjectStore<TState> Store => _store;
        
        /// <summary>
        /// This property tells whether non persisted changes are present or not.
        /// </summary>
        public bool HasStateChanged
        {
            get => _hasStateChanged;
            private set => SetProperty(ref _hasStateChanged, value);
        }

        #endregion Properties

        #region Life cycle

        /// <summary>
        /// Construct the object
        /// </summary>
        public DataObject(IDataObjectSource<TState> source, TState data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _store = new DataObjectStore<TState>(data);
        }
        
        /// <summary>
        /// Construct the object
        /// </summary>
        public DataObject(IDataObjectSource<TState> source, DataObjectStore<TState> store)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        #endregion Life cycle

        #region Methods

        /// <summary>
        /// This method loads settings from a file. Takes current configuration last loaded/saved
        /// source if parameter is null
        /// </summary>
        public void Load()
        {
            _store.CurrentData = _source.Load();
            HasStateChanged = false;
        }

        /// <summary>
        /// This method loads settings to a source object.
        /// </summary>
        public void Save()
        {
            _source.Save(Store.CurrentData);
            HasStateChanged = false;
        }

        /// <inheritdoc/>
        public override void ReceiveStateChanged(object sender, StateChangedEventArgs args)
        {
            if (args.HasStateChanged)
            {
                HasStateChanged = true;
            }
            base.ReceiveStateChanged(sender, args);
        }

        #endregion Methods
    }
}