// -----------------------------------------------------------------------
// <copyright file="GetSideDishSummaryReportCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/30">Created</change>
// </history>

namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;
    using NexFB.Core.Extensions;

    /// <summary>
    /// Get side dish summary report criteria.
    /// </summary>
    public class GetSideDishSummaryReportCriteria : CriteriaBase
    {
        public string DepartmentId { get; set; }

        public string EmployeeTypeId { get; set; }

        public string TeamId { get; set; }

        [SpParam(SqlParamName = "SysID")]
        public int? UserId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int? PageIndex { get; set; }

        public int? ItemsPerPage { get; set; }

        [Output]
        public int? TotalItem { get; set; }

        [Output]
        public decimal? TotalQuantity { get; set; }

        [Output]
        public decimal? TotalAmount { get; set; }

        public override string GetSettingKey()
        {
            return "RPT_SideDishSummary";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate;
        }
    }
}