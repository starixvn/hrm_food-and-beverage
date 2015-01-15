// -----------------------------------------------------------------------
// <copyright file="GetMealForecastReportCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/7">Created.</change>
// </history>

namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get meal forecast report criteria.
    /// </summary>
    public class GetMealForecastReportCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int? DepartmentId { get; set; }

        public bool? IsVegetarian { get; set; }

        public override string GetSettingKey()
        {
            return "RPT_MealForecastReport";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate;
        }
    }
}