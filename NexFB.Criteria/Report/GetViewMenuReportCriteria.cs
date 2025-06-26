namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// GetViewMenuReportCriteria class.
    /// </summary>
    public class GetViewMenuReportCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int? SysId { get; set; }

        public string MealId { get; set; }

        public bool? IsVegetarian { get; set; }

        public override string GetSettingKey()
        {
            return "RPT_ViewMenuReport";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
