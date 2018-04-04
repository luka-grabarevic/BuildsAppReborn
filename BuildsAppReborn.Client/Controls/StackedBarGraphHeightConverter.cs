using System;
using System.Globalization;
using System.Windows.Data;

namespace BuildsAppReborn.Client.Controls
{
    internal class StackedBarGraphHeightConverter : IMultiValueConverter
    {
        #region Implementation of IMultiValueConverter

        public Object Convert(Object[] values, Type targetType, Object parameter, CultureInfo culture)
        {
            var maxHeight = (Double) values[0];
            var count = (Int32) values[1];
            var maxCount = (Int32) values[2];

            if (maxCount == 0)
            {
                return 0;
            }

            var factor = (Double)count / maxCount;

            return maxHeight * factor;
        }

        public Object[] ConvertBack(Object value, Type[] targetTypes, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}