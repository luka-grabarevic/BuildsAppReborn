using System;
using System.Globalization;
using System.Windows.Data;
using BuildsAppReborn.Client.Resources;
using BuildsAppReborn.Contracts.Models;
using BuildsAppReborn.Infrastructure;

namespace BuildsAppReborn.Client.Converter
{
    public class BuildStatusToImageConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var enumValue = EnumHelper.GetEnumValueFromObject<BuildStatus>(value);
            return new Uri(IconProvider.GetIconForBuildStatus(enumValue));
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}