using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Contracts.UI;

namespace BuildsAppReborn.Client.Converter
{
    public class BuildItemToRelativeMarginConverter : IMultiValueConverter
    {
        #region Implementation of IMultiValueConverter

        public Object Convert(Object[] values, Type targetType, Object parameter, CultureInfo culture)
        {
            var currentBuild = values[0] as IBuild;
            var buildItems = values[1] as List<BuildItem>;
            var height = (Double) values[2];
            if (currentBuild != null && buildItems != null)
            {
                var durations = buildItems.Select(b => (b.Build.FinishDateTime - b.Build.StartDateTime).TotalSeconds);
                var duration = (currentBuild.FinishDateTime - currentBuild.StartDateTime).TotalSeconds;
                var barHeight = (height * duration) / durations.Max();
                if (barHeight < 1)
                {
                    barHeight = 1;
                }
                var topMargin = height - barHeight;
                return new Thickness(0, Math.Round(topMargin), 3, 0);
            }
            return default(Thickness);
        }


        public Object[] ConvertBack(Object value, Type[] targetTypes, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}