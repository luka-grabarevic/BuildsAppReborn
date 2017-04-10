using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace BuildsAppReborn.Client.Converter
{
    public class TimeSpanToNiceStringConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var duration = value as TimeSpan?;
            if (duration == null)
                return value;

            var seconds = duration.Value.Seconds;
            var minutes = duration.Value.Minutes;
            var hours = duration.Value.Hours;
            var days = (Int32) duration.Value.TotalDays;

            var values = new List<String>();

            if (days > 0)
            {
                // don't care for anything other than days after one week
                if (days > 6)
                    return days + "d";

                values.Add(days + "d");
            }
            if (hours > 0) values.Add(hours + "h");
            if (minutes > 0) values.Add(minutes + "m");
            if (seconds > 0) values.Add(seconds + "s");

            var result = String.Join("", values.Take(2));
            return result;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}