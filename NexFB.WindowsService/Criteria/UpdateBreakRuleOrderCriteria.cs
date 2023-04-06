

namespace NexFB.WindowsService.Criteria
{
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class UpdateBreakRuleOrderCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Update_BreakRuleOrder";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
