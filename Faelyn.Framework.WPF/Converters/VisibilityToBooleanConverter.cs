using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Faelyn.Framework.WPF.Converters
{
    /// <summary>
    /// Returns true if Visibility has 'Visible' as value, false otherwise
    /// </summary>
    public sealed class VisibilityToBooleanConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Compares bound value to parameter
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }

        /// <summary>
        /// Check the comparison value
        /// </summary>
        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            return value != null && (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion Methods
    }
}
