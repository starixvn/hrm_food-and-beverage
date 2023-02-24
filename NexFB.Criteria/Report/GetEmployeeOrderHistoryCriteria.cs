namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    public class GetEmployeeOrderHistoryCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysID")]
        public int? UserId { get; set; }

        public int OrderBy { get; set; }

        [SpParam(SqlParamName = "MyMenu")]
        public int EmployeeMenu { get; set; }

        public int OrderPlatform { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string MealId { get; set; }

        public string StatusId { get; set; }

        [SpParam(Ignored = true)]
        public int StartIndex { get; set; }

        [SpParam(Ignored = true)]
        public int ItemsPerPage { get; set; } = 50;

        [SpParam(Ignored = true)]
        public bool IsPagingCallback { get; set; }

        public override string GetSettingKey()
        {
            return "Rpt_Get_MyOrderHistory";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate;
        }
    }
}