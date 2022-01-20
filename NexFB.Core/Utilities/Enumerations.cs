namespace NexFB.Core.Utilities
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public static class Enumerations
    {
        public static string GetDescription(Enum enumType)
        {
            FieldInfo fi = enumType.GetType().GetField(enumType.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : enumType.ToString();
        }
    }
}