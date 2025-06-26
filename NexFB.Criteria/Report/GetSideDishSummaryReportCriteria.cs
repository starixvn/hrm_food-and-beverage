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
        public string CompanyId { get; set; }

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
