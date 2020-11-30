using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Faelyn.Framework.WPF.Converters
{
    /// <summary>
    /// Return Visible if the value is true, Collapsed otherwise
    /// </summary>
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Compares bound value to parameter
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return b ? Visibility.Visible : Visibility.Collapsed;
            }
            return true;
        }

        /// <summary>
        /// Check the comparison value
        /// </summary>
        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return true;
        }

        #endregion Methods
    }
}
