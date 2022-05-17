using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Data.Repository;

namespace NexFB.Criteria.MealRegistration
{
    public class GetEmployeeRegisterByWorkDateAndMealIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int SystemId { get; set; }
        public string WorkDate { get; set; }
        public int MealID { get; set; }
        public override string GetSettingKey()
        {
            return "RegisterMeal_GetBySysID";
        }

        public override bool IsValid()
        {
            return SystemId > 0 && MealID > 0;
        }
    }
}
