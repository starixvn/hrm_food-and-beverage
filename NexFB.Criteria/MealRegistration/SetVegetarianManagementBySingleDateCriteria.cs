using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NexFB.Criteria.MealRegistration
{
    public class SetVegetarianManagementBySingleDateCriteria : CriteriaBase
    {
        public string Meal { get; set; }

        public string OnDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "VegetarianManagement_SetBySingleDate";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
