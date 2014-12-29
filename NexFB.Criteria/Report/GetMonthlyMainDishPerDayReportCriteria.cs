// ----------------------------------------------------------------------
// <copyright file="GetMonthlyMainDishPerDayReportCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/26/2014 2:41:33 PM">Created.</change>
// </history>

namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get monthly main dish per day report criteria.
    /// </summary>
    public class GetMonthlyMainDishPerDayReportCriteria : CriteriaBase
    {
        public DateTime ReportDate { get; set; }

        public string DepartmentId { get; set; }

        public string EmployeeTypeId { get; set; }

        public string TeamId { get; set; }

        public bool? WorkDay { get; set; }

        public int GroupBy { get; set; }

        public bool IsDebug { get; set; }

        public int IsLocal { get; set; }

        public override string GetSettingKey()
        {
            return "RPT_MonthMainDishByExpat";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
