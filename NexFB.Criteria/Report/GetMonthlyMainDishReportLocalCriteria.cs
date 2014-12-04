// ----------------------------------------------------------------------
// <copyright file="GetMonthlyMainDishReportLocalCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/4/2014 9:02:09 AM">Created.</change>
// </history>

namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get monthly main dish report by local criteria.
    /// </summary>
    public class GetMonthlyMainDishReportLocalCriteria : CriteriaBase
    {
        public DateTime ReportDate { get; set; }

        public string DepartmentId { get; set; }

        public string EmployeeTypeId { get; set; }

        public string TeamId { get; set; }

        public string MealId { get; set; }

        public int GroupBy { get; set; }

        public bool IsDebug { get; set; }

        public int IsLocal { get; set; }

        [SpParam(Ignored = true)]
        public int StartIndex { get; set; }

        [SpParam(Ignored = true)]
        public int ItemsPerPage { get; set; }

        public override string GetSettingKey()
        {
            return "RPT_MonthMainDishByLocal";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
