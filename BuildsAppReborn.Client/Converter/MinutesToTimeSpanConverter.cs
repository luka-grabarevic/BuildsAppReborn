using System;
using System.Globalization;
using System.Windows.Data;

namespace BuildsAppReborn.Client.Converter
{
    internal class MinutesToTimeSpanConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value is TimeSpan)
            {
                var ts = (TimeSpan) value;
                return ts.TotalMinutes;
            }

            return Binding.DoNothing;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            // ToDo: proper string parsing
            if (value is Int32)
            {
                var minutes = (Int32) value;
                return TimeSpan.FromMinutes(minutes);
            }

            return Binding.DoNothing;
        }
    }
}