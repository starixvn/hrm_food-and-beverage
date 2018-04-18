// --------------------------------------------------------------------------
// <copyright file="GetViewMenuReportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Larry.Tran" date="2015.1.19">Created</change>
// </history>

namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// GetViewMenuReportCriteria class.
    /// </summary>
    public class GetViewMenuReportCriteria : CriteriaBase
    {
        public DateTime OnDate { get; set; }

        public int? SysId { get; set; }

        public string MealId { get; set; }

        public bool? IsVegetarian { get; set; }

        public override string GetSettingKey()
        {
            return "RPT_ViewMenuReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}