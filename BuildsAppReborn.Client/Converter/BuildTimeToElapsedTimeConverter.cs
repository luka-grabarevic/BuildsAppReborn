using System;
using System.Globalization;
using System.Windows.Data;

namespace BuildsAppReborn.Client.Converter
{
    public class BuildTimeToElapsedTimeConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var isTime = value is DateTime;
            if (!isTime)
            {
                return value;
            }

            var time = (DateTime) value;
            var elapsed = DateTime.UtcNow.Subtract(time);

            return $"{GetReadableTimespan(elapsed)}";
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private String GetReadableTimespan(TimeSpan elapsedTimeSpan)
        {
            if (elapsedTimeSpan.TotalSeconds < 1)
            {
                return "1s";
            }

            var seconds = elapsedTimeSpan.Seconds;
            var minutes = elapsedTimeSpan.Minutes;
            var hours = elapsedTimeSpan.Hours;
            var days = elapsedTimeSpan.Days;
            if (days < 1)
            {
                if (hours <= 0 && minutes <= 0 && seconds <= 0)
                {
                    return $"- {GetReadableTimespan(elapsedTimeSpan.Duration())}";
                }

                if (hours == 0 && minutes == 0 && seconds > 0)
                {
                    return seconds > 30 ? $"1m" : "<1m";
                }

                if (hours == 0 && minutes > 0)
                {
                    return seconds > 30 ? $"{minutes + 1}m" : $"{minutes}m";
                }

                if (hours > 0)
                {
                    return minutes > 30 ? $"{hours + 1}h" : $"{hours}h";
                }
            }

            if (days >= 1)
            {
                return hours > 12 ? $"{days + 1}d" : $"{days}d";
            }

            return "?";
        }
    }
}