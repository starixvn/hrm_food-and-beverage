namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    public class GetMonthlyLunchAndDinnerReportCriteria : CriteriaBase
    {
        public DateTime ReportDate { get; set; }

        public string DepartmentId { get; set; }

        public override string GetSettingKey()
        {
            return "Rpt_Get_MonthlyLunchAndDinnerMealsAmount";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(DepartmentId);
        }
    }
}