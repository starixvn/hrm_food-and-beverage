namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    public class GetMonthlyBreakfastAndSupperReportCriteria : CriteriaBase
    {
        public DateTime ReportDate { get; set; }

        public string DepartmentId { get; set; }

        public override string GetSettingKey()
        {
            return "Rpt_Get_MonthlyBreakfastAndSupperMealsAmount";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(DepartmentId);
        }
    }
}