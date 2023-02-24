namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    public class GetDailyMainDishPricePerMealReportCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string CompanyId { get; set; }

        public string EmployeeTypeId { get; set; }

        public string TeamId { get; set; }

        public string MealId { get; set; }

        public bool IsDebug { get; set; }

        public int? IsLocal { get; set; }

        [SpParam(SqlParamName = "Sysid")]
        public int? UserId { get; set; }

        public int OrderCount { get; set; }

        public string Query { get; set; }

        [SpParam(Ignored = true)]
        public int StartIndex { get; set; }

        [SpParam(Ignored = true)]
        public int ItemsPerPage { get; set; } = 50;

        [SpParam(Ignored = true)]
        public bool IsPagingCallback { get; set; }

        public override string GetSettingKey()
        {
            return "RPT_DailyMainDishPricePerMeal";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate && FromDate.AddYears(1) >= ToDate;
        }
    }
}