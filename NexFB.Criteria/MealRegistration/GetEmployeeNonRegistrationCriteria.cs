using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NexFB.Criteria.MealRegistration
{
   public class GetEmployeeNonRegistrationCriteria : CriteriaBase
    {
        public DateTime WorkDate { get; set; }

        public int? MealId { get; set; }

        public int? MenuId { get; set; }

        public string DepartmentId { get; set; }

        public string EmployeeTypeId { get; set; }

        public bool? IsVegetarian { get; set; }

        public override string GetSettingKey()
        {
            return "RPT_NonRegistrationMealDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
