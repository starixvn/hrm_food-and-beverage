// ----------------------------------------------------------------------
// <copyright file="DayType.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/29/2014 1:45:29 PM">Created.</change>
// </history>

namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// The Day Type ENUM.
    /// </summary>
    public enum DayType
    {
        /// <summary>
        /// The Off day item.
        /// </summary>
        [Description("Off day")]
        OffDay = 0,

        /// <summary>
        /// The Work day item.
        /// </summary>
        [Description("Work day")]
        WorkDay = 1,
    }
}
