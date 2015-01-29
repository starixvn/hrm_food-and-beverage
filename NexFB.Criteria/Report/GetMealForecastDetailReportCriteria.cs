// -----------------------------------------------------------------------
// <copyright file="GetMealForecastDetailReportCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/16">Created.</change>
// </history>

namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get meal forecast detail report criteria.
    /// </summary>
    public class GetMealForecastDetailReportCriteria : CriteriaBase
    {
        public DateTime OnDate { get; set; }

        public string DepartmentId { get; set; }

        public bool? IsVegetarian { get; set; }

        public override string GetSettingKey()
        {
            return "RPT_MealForecastDetailReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}