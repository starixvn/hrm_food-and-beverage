// ----------------------------------------------------------------------
// <copyright file="GetDailyMainDishPricePerMealReportCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/12/2014 9:24:56 AM">Created.</change>
// </history>

namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get daily main dish price per meal report criteria.
    /// </summary>
    public class GetDailyMainDishPricePerMealReportCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string DepartmentId { get; set; }

        public string EmployeeTypeId { get; set; }

        public string TeamId { get; set; }

        public string MealId { get; set; }

        public bool IsDebug { get; set; }

        public int? IsLocal { get; set; }

        [SpParam(SqlParamName = "Sysid")]
        public int? UserId { get; set; }

        public int OrderCount { get; set; }

        public string Query { get; set; }

        public override string GetSettingKey()
        {
            return "RPT_DailyMainDishPricePerMeal";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate && FromDate.AddYears(1) >= ToDate;
        }
    }
}
