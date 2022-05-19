// ----------------------------------------------------------------------
// <copyright file="GetAdminOrderHistoryCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Aaron.Cao" date="03/09/2015 4:03:33 PM">Created.</change>
// </history>

namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Get admin order history criteria.
    /// </summary>
    public class GetAdminOrderHistoryCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysID")]
        public int? UserId { get; set; }

        public int OrderBy { get; set; }

        public int MyMenu { get; set; }

        public int OrderPlatform { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string MealId { get; set; }

        public string StatusId { get; set; }
        public string CompanyId { get; set; }

        public override string GetSettingKey()
        {
            return "Rpt_Get_OrderHistory";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate;
        }
    }
}
