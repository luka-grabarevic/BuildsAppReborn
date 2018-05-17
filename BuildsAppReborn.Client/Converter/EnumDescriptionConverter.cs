using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace BuildsAppReborn.Client.Converter
{
    // https://stackoverflow.com/a/3987099
    public class EnumDescriptionConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        Object IValueConverter.Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var myEnum = (Enum) value;
            var description = GetEnumDescription(myEnum);
            return description;
        }

        Object IValueConverter.ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return String.Empty;
        }

        #endregion

        #region Private Methods

        private String GetEnumDescription(Enum enumObj)
        {
            var fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

            var attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }

            var attrib = attribArray[0] as DescriptionAttribute;
            return attrib?.Description;
        }

        #endregion
    }
}