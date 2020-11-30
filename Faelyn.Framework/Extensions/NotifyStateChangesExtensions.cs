using System.Collections;
using System.Collections.Specialized;
using Faelyn.Framework.Interfaces;

namespace Faelyn.Framework.Extensions
{
    /// <summary>
    /// Extensions methods to handle state changed objects
    /// </summary>
    public static class NotifyStateChangesExtensions
    {
        #region Methods

        /// <summary>
        /// Link an object to a child collection (Observable collection handled only)
        /// </summary>
        public static bool LinkCollectionState(IReceiveStateChanges stateParent, IEnumerable stateChildCollection)
        {
            INotifyCollectionChanged collectionChanged = stateChildCollection as INotifyCollectionChanged;
            if (stateParent == null || stateChildCollection == null || collectionChanged == null)
            {
                return false;
            }

            collectionChanged.CollectionChanged += stateParent.ReceiveCollectionStateChanged;
            foreach (object item in stateChildCollection)
            {
                if (!LinkState(stateParent, item as INotifyStateChanges))
                {
                    LinkCollectionState(stateParent, item as IEnumerable);
                }
            }
            return true;
        }

        /// <summary>
        /// Link an object to its property (INotifyStateChanged and observable collection handled only)
        /// </summary>
        public static bool LinkState(this IReceiveStateChanges stateParent, string propertyName)
        {
            object propertyValue = stateParent?.GetType()?.GetProperty(propertyName)?.GetValue(stateParent);
            return LinkState(stateParent, propertyValue);
        }

        /// <summary>
        /// Link an object to its child (INotifyStateChanged and observable collection handled only)
        /// </summary>
        public static bool LinkState(this IReceiveStateChanges stateParent, object child)
        {
            INotifyStateChanges stateChild = child as INotifyStateChanges;
            if (stateParent == null || stateChild == null)
            {
                return LinkCollectionState(stateParent, child as IEnumerable);
            }

            stateChild.OnStateChanged += stateParent.ReceiveStateChanged;
            return true;
        }

        /// <summary>
        /// Unlink an object to a child collection (Observable collection handled only)
        /// </summary>
        public static bool UnlinkCollectionState(IReceiveStateChanges stateParent, IEnumerable stateChildCollection)
        {
            INotifyCollectionChanged collectionChanged = stateChildCollection as INotifyCollectionChanged;
            if (stateParent == null || stateChildCollection == null || collectionChanged == null)
            {
                return false;
            }

            collectionChanged.CollectionChanged -= stateParent.ReceiveCollectionStateChanged;
            foreach (object item in stateChildCollection)
            {
                if (!UnlinkState(stateParent, item as INotifyStateChanges))
                {
                    UnlinkCollectionState(stateParent, item as IEnumerable);
                }
            }
            return true;
        }

        /// <summary>
        /// Unlink an object from its property (INotifyStateChanged and observable collection handled only)
        /// </summary>
        public static bool UnlinkState(this IReceiveStateChanges stateParent, string propertyName)
        {
            object propertyValue = stateParent?.GetType()?.GetProperty(propertyName)?.GetValue(stateParent);
            return UnlinkState(stateParent, propertyValue);
        }

        /// <summary>
        /// Unlink an object to its child (INotifyStateChanged and observable collection handled only)
        /// </summary>
        public static bool UnlinkState(this IReceiveStateChanges stateParent, object child)
        {
            INotifyStateChanges stateChild = child as INotifyStateChanges;
            if (stateParent == null || stateChild == null)
            {
                return UnlinkCollectionState(stateParent, child as IEnumerable);
            }

            stateChild.OnStateChanged -= stateParent.ReceiveStateChanged;
            return true;
        }

        #endregion Methods
    }
}