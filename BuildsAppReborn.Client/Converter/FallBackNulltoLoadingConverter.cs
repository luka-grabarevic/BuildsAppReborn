using System;
using System.Globalization;
using System.Windows.Data;
using BuildsAppReborn.Client.Resources;

namespace BuildsAppReborn.Client.Converter
{
    public class FallBackNullToLoadingConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return String.IsNullOrWhiteSpace(value?.ToString()) ? IconProvider.LoadingIcon : value;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}