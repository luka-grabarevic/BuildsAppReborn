using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Client.Converter
{
    // might want to move this to some sort of configuration ...
    public class BuildStatusToSolidColorBrushBackgroundConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var color = default(Color);
            var enumValue = EnumHelper.GetEnumValueFromObject<BuildStatus>(value);
            switch (enumValue)
            {
                case BuildStatus.Unknown:
                    color = Colors.LightGray;
                    break;
                case BuildStatus.Succeeded:
                    color = Colors.LightGreen;
                    break;
                case BuildStatus.Failed:
                    color = Colors.LightCoral;
                    break;
                case BuildStatus.PartiallySucceeded:
                    color = Colors.Wheat;
                    break;
                case BuildStatus.Running:
                    color = Colors.CadetBlue;
                    break;
                case BuildStatus.Stopped:
                    color = Colors.DarkSlateGray;
                    break;
                case BuildStatus.Queued:
                    color = Colors.Gray;
                    break;
            }

            return new SolidColorBrush(color);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}