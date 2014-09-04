// ----------------------------------------------------------------------
// <copyright file="Enumerations.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/4/2014 2:51:21 PM">Created.</change>
// </history>

namespace NexFB.Core.Utilities
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// The utility methods of ENUM.
    /// </summary>
    public static class Enumerations
    {
        /// <summary>
        /// Get ENUM Description.
        /// </summary>
        /// <param name="enumType">ENUM type.</param>
        /// <returns>String description of an ENUM.</returns>
        public static string GetDescripton(Enum enumType)
        {
            FieldInfo fi = enumType.GetType().GetField(enumType.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return enumType.ToString();
            }
        }
    }
}
