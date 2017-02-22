using System;

namespace BuildsAppReborn.Infrastructure
{
    public static class EnumHelper
    {
        #region Public Static Methods

        public static T GetEnumValueFromObject<T>(Object o)
        {
            var enumVal = (T) Enum.Parse(typeof(T), o.ToString());
            return enumVal;
        }

        #endregion
    }
}