using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NexFB.Criteria.MealRegistration
{
    public class GetVegetarianManagementByRangeDateCriteria : CriteriaBase
    {
        public short? MealId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey()
        {
            return "VegetarianManagement_GetByRangeDate";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
