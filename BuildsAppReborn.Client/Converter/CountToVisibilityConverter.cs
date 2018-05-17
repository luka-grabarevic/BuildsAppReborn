using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BuildsAppReborn.Client.Converter
{
    public class CountToVisibilityConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var collection = value as ICollection;
            if (collection != null && collection.Count > 0)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}