using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Client.Converter
{
    // might want to move this to some sort of configuration ...
    public class BuildStatusToSolidColorBrushForegroundConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var color = default(Color);
            var enumValue = EnumHelper.GetEnumValueFromObject<BuildStatus>(value);
            switch (enumValue)
            {
                case BuildStatus.Unknown:
                    color = Colors.Gray;
                    break;
                case BuildStatus.Succeeded:
                    color = Colors.Green;
                    break;
                case BuildStatus.Failed:
                    color = Colors.Red;
                    break;
                case BuildStatus.PartiallySucceeded:
                    color = Colors.Orange;
                    break;
                case BuildStatus.Running:
                    color = Colors.DarkCyan;
                    break;
                case BuildStatus.Stopped:
                    color = Colors.Black;
                    break;
                case BuildStatus.Queued:
                    color = Colors.DarkGray;
                    break;
            }
            return new SolidColorBrush(color);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}