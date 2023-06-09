namespace NexFB.Criteria.Report
{
    using System;
    using Fanex.Data.Repository;

    public class GetBreakRuleOrderCriteria : CriteriaBase
    {
        public DateTime ReportDate { get; set; }
        public override string GetSettingKey()
        {
            return "RPT_GetBreakRuleOrder";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
