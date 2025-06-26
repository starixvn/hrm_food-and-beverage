namespace NexFB.Criteria.MealRegistration
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Register meal by range date criteria.
    /// </summary>
    public class RegisterMealByRangeDateCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int SystemId { get; set; }

        public string Meal { get; set; }

        public string OnDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "RegisterMeal_SetByRangeDate";
        }

        public override bool IsValid()
        {
            return SystemId > 0;
        }
    }
}
