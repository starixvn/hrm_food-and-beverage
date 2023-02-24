namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;
    using NexFB.Core.Extensions;

    public class GetSideDishReportCriteria : CriteriaBase
    {
        public string CompanyId { get; set; }

        [SpParam(SqlParamName = "EmployeeTypeId")]
        public string EmployeeTypeIds { get; set; }

        [SpParam(SqlParamName = "TeamId")]
        public string TeamIds { get; set; }

        [SpParam(SqlParamName = "SysID")]
        public int? UserId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "SideDishItemId")]
        public string SideDishItemIds { get; set; }

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