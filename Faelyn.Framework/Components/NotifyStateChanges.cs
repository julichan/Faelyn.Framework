using System;
using System.Collections;
using System.Collections.Specialized;
using Faelyn.Framework.Events;
using Faelyn.Framework.Extensions;
using Faelyn.Framework.Interfaces;

namespace Faelyn.Framework.Components
{
    public abstract class NotifyStateChanges : NotifyPropertyChanges, IReceiveStateChanges, INotifyStateChanges
    {
        #region Life cycle

        /// <summary>
        /// Protected constructor
        /// </summary>
        protected NotifyStateChanges()
        {
            // This doesn't create leak because it's linking to itself
            PropertyChanging += (s, a) =>
            {
                if (a is UpdatePropertyChangingEventArgs args && args.Updating)
                {
                    this.UnlinkState(a.PropertyName);
                }
            };
            // This doesn't create leak because it's linking to itself
            PropertyChanged += (s, a) =>
            {
                if (a is UpdatePropertyChangedEventArgs args && args.Updated)
                {
                    this.LinkState(a.PropertyName);
                    OnStateChanged?.Invoke(this, new StateChangedEventArgs(true));
                }
            };
        }

        #endregion Life cycle

        #region Interface INotifyStateChanges

        /// <inheritdoc/>
        public event EventHandler<StateChangedEventArgs> OnStateChanged;

        #endregion Interface INotifyStateChanges

        #region Interface IReceiveStateChanges

        /// <inheritdoc/>
        public virtual void ReceiveCollectionStateChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                if (sender is IEnumerable enumerable)
                {
                    foreach (INotifyStateChanges item in enumerable)
                    {
                        this.UnlinkState(item);
                    }
                }
            }

            if (args.Action == NotifyCollectionChangedAction.Remove || args.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (INotifyStateChanges item in args.OldItems)
                {
                    this.UnlinkState(item);
                }
            }
            if (args.Action == NotifyCollectionChangedAction.Add || args.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (INotifyStateChanges item in args.NewItems)
                {
                    this.LinkState(item);
                }
            }
            OnStateChanged?.Invoke(this, new StateChangedEventArgs(true));
        }

        /// <inheritdoc/>
        public virtual void ReceiveStateChanged(object sender, StateChangedEventArgs args)
        {
            OnStateChanged?.Invoke(this, args);
        }

        #endregion Interface IReceiveStateChanges
    }
}