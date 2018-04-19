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
            var items = values[2] as StackedItemCollection;

            if (items == null || items.Count == 0)
            {
                return 0;
            }

            var excessiveHeight = GetExcessiveHeight(items, maxHeight);

            return GetNormalizedHeight(count, items.TotalCount, maxHeight, excessiveHeight);
        }

        public Object[] ConvertBack(Object value, Type[] targetTypes, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Private Static Methods

        private static Double GetFactor(Int32 count, Int32 totalCount)
        {
            var factor = (Double) count / totalCount;
            return factor;
        }

        private static Double GetHeight(Int32 count, Int32 totalCount, Double maxHeight)
        {
            var factor = GetFactor(count, totalCount);

            return maxHeight * factor;
        }

        #endregion

        #region Private Methods

        private Double GetExcessiveHeight(StackedItemCollection items, Double maxHeight)
        {
            var excessive = 0d;

            foreach (var item in items)
            {
                if (item.Value == 0)
                {
                    continue;
                }

                var height = GetHeight(item.Value, items.TotalCount, maxHeight);
                if (height < MinHeight)
                {
                    excessive += MinHeight - height;
                }
            }

            return excessive;
        }

        private Double GetNormalizedHeight(Int32 count, Int32 totalCount, Double maxHeight, Double excessiveHeight)
        {
            if (count == 0)
            {
                return 0;
            }

            var factor = GetFactor(count, totalCount);

            var height = maxHeight * factor;
            if (height < MinHeight)
            {
                return MinHeight;
            }

            return (maxHeight - excessiveHeight) * factor;
        }

        #endregion

        private const Int32 MinHeight = 5;
    }
}