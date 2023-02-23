namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    public class GetMonthlyBreakfastAndSupperReportCriteria : CriteriaBase
    {
        public DateTime ReportDate { get; set; }

        public string CompanyId { get; set; }

        public override string GetSettingKey() => "Rpt_Get_MonthlyBreakfastAndSupperMealsAmount";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(CompanyId);
    }
}