namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    public class GetMonthlyMainDishPerDayReportCriteria : CriteriaBase
    {
        public DateTime ReportDate { get; set; }

        public string CompanyId { get; set; }

        public string EmployeeTypeId { get; set; }

        public string TeamId { get; set; }

        public int? WorkDay { get; set; }

        public int GroupBy { get; set; }

        public bool IsDebug { get; set; }

        public int IsLocal { get; set; }

        [SpParam(Ignored = true)]
        public int StartIndex { get; set; }

        [SpParam(Ignored = true)]
        public int ItemsPerPage { get; set; } = 50;

        [SpParam(Ignored = true)]
        public bool IsPagingCallback { get; set; }

        public override string GetSettingKey()
        {
            return "RPT_MonthMainDishByExpat";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}