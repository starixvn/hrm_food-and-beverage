namespace NexFB.Criteria.MealRegistration
{
    using System;
    using Fanex.Data.Repository;

    public class GetRegisterMealByRangeDateCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int SystemId { get; set; }

        public short? MealId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public override string GetSettingKey()
        {
            return "RegisterMeal_GetByRangeDate";
        }

        public override bool IsValid()
        {
            return SystemId > 0;
        }
    }
}