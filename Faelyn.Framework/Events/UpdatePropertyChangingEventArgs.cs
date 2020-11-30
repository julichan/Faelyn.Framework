using System.ComponentModel;

namespace Faelyn.Framework.Events
{
    /// <inheritdoc/>
    public class UpdatePropertyChangingEventArgs : PropertyChangingEventArgs
    {
        #region Properties

        /// <summary>
        /// Indicate if the property is actually changing.
        /// </summary>
        public bool Updating { get; }

        #endregion Properties

        #region Life cycle

        /// <summary>
        /// Constructor
        /// </summary>
        public UpdatePropertyChangingEventArgs(string propertyName, bool updating) : base(propertyName)
        {
            Updating = updating;
        }

        #endregion Life cycle
    }
}