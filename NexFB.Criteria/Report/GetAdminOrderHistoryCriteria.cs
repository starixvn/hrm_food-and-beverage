namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

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

        public override string GetSettingKey() => "Rpt_Get_OrderHistory";

        public override bool IsValid() => FromDate <= ToDate;
    }
}