using System.ComponentModel;

namespace Faelyn.Framework.Events
{
    /// <inheritdoc/>
    public class UpdatePropertyChangedEventArgs : PropertyChangedEventArgs
    {
        #region Properties

        /// <summary>
        /// Indicate if the property is actually changed.
        /// </summary>
        public bool Updated { get; }

        #endregion Properties

        #region Life cycle

        /// <summary>
        /// Constructor
        /// </summary>
        public UpdatePropertyChangedEventArgs(string propertyName, bool updated) : base(propertyName)
        {
            Updated = updated;
        }

        #endregion Life cycle
    }
}