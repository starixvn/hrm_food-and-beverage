using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexFB.Criteria.SmartCardOrder
{
    public class GetInfoMainDishOrderCriteria : CriteriaBase
    {
        public string UserIds { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "POS_GetInfo_MainDishOrder";
        }
    }
}
