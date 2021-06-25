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
        private readonly DataObjectRef<TState> _ref;
        private bool _hasStateChanged = true;

        #endregion
        
        #region Properties
        
        /// <summary>
        /// This property contains the data source.
        /// </summary>
        public IDataObjectRef<TState> Ref => _ref;
        
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
            _ref = new DataObjectRef<TState>(data);
        }
        
        /// <summary>
        /// Construct the object
        /// </summary>
        public DataObject(IDataObjectSource<TState> source, DataObjectRef<TState> @ref)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _ref = @ref ?? throw new ArgumentNullException(nameof(@ref));
        }

        #endregion Life cycle

        #region Methods

        /// <summary>
        /// This method loads settings from a file. Takes current configuration last loaded/saved
        /// source if parameter is null
        /// </summary>
        public void Load()
        {
            _ref.CurrentData = _source.Load();
            HasStateChanged = false;
        }

        /// <summary>
        /// This method loads settings to a source object.
        /// </summary>
        public void Save()
        {
            _source.Save(Ref.CurrentData);
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