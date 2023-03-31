using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexFB.Criteria.Report
{
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
