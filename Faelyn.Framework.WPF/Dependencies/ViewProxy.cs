using System.Windows;

namespace Faelyn.Framework.WPF.Dependencies
{
    public class ViewProxy : Freezable
    {
        #region Fields

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(ViewProxy), new PropertyMetadata(null));

        #endregion Fields

        #region Properties

        public object Data
        {
            get => (object)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        #endregion Properties

        #region Methods

        protected override Freezable CreateInstanceCore()
        {
            return new ViewProxy();
        }

        #endregion Methods
    }
}
