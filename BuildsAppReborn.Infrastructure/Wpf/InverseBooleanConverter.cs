using System;
using System.Globalization;
using System.Windows.Data;

namespace BuildsAppReborn.Infrastructure.Wpf
{
    [ValueConversion(typeof(Boolean), typeof(Boolean))]
    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value is Boolean)
            {
                return !(Boolean)value;
            }
            return Binding.DoNothing;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}