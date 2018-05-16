// ----------------------------------------------------------------------
// <copyright file="GroupByReport.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/6/2014 3:25:22 PM">Created.</change>
// </history>

namespace NexFB.Core.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// Group by report.
    /// </summary>
    public enum GroupByReport
    {
        /// <summary>
        /// Group by employee item.
        /// </summary>
        [Description("Employee")]
        Employee = 0,

        /// <summary>
        /// Group by department item.
        /// </summary>
        [Description("Department")]
        Department = 1,

        /// <summary>
        /// Group by cost center item.
        /// </summary>
        [Description("CostCenter")]
        CostCenter = 2,
    }
}