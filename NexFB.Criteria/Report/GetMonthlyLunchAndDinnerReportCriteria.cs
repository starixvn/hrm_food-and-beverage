namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    public class GetMonthlyLunchAndDinnerReportCriteria : CriteriaBase
    {
        public DateTime ReportDate { get; set; }

        public string CompanyId { get; set; }

        public override string GetSettingKey() => "Rpt_Get_MonthlyLunchAndDinnerMealsAmount";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(CompanyId);
    }
}