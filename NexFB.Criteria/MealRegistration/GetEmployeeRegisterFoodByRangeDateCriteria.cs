using System;
using Fanex.Data.Repository;

namespace NexFB.Criteria.MealRegistration
{
    public class GetEmployeeRegisterFoodByRangeDateCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int SystemId { get; set; }

        public short? MealId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey()
        {
            return "EmployeeRegisterFood_GetByRangeDate";
        }

        public override bool IsValid()
        {
            return SystemId > 0;
        }
    }
}
