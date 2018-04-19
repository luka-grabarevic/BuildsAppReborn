using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BuildsAppReborn.Client.Converter
{
    public class NullVisibilityConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}