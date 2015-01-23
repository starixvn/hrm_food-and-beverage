// -----------------------------------------------------------------------
// <copyright file="GetSideDishReportCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Larry.Tran" date="1/7/2015 3:44 PM">Created.</change>
// </history>

namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;
    using NexFB.Core.Extensions;

    /// <summary>
    /// The GetSideDishReportCriteria class.
    /// </summary>
    public class GetSideDishReportCriteria : CriteriaBase
    {
        public string DepartmentId { get; set; }

        public string EmployeeTypeId { get; set; }

        public string TeamId { get; set; }

        [SpParam(SqlParamName = "SysID")]
        public int? UserId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SideDishItemId { get; set; }

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
            return "RPT_DailySideDish";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate;
        }
    }
}
