using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using BuildsAppReborn.Contracts.UI;

namespace BuildsAppReborn.Client.Converter
{
    public class BuildItemToRelativeMarginConverter : IMultiValueConverter
    {
        public Object Convert(Object[] values, Type targetType, Object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 4)
            {
                return default(Thickness);
            }

            var currentBuildItem = values[0] as BuildItem;
            var buildItems = values[1] as List<BuildItem>;
            var height = (Double) values[2];
            if (currentBuildItem != null && buildItems != null)
            {
                // get longest build time
                var longestDuration = buildItems.Select(b => b.BuildDuration.TotalSeconds).Max();

                var duration = currentBuildItem.BuildDuration.TotalSeconds;
                var barHeight = height * duration / longestDuration;
                if (barHeight < 1)
                {
                    barHeight = 1;
                }

                var topMargin = height - barHeight;
                return new Thickness(0, Math.Round(topMargin), 0, 0);
            }

            return default(Thickness);
        }

        public Object[] ConvertBack(Object value, Type[] targetTypes, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}