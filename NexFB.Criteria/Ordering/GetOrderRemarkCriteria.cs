using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NexFB.Criteria.Ordering
{
    public class GetOrderRemarkCriteria : CriteriaBase
    {
        public int RegisterBy { get; set; }

        public override string GetSettingKey()
        {
            return "Remark_GetTopTen";
        }

        public override bool IsValid()
        {
            return RegisterBy > 0;
        }
    }
}
