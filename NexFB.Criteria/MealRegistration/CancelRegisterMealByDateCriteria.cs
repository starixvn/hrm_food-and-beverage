namespace NexFB.Criteria.MealRegistration
{
    using System;
    using Fanex.Data.Repository;

    public class CancelRegisterMealByDateCriteria : CriteriaBase
    {
        public int SysId { get; set; }

        public int MealId { get; set; }

        public DateTime OnDate { get; set; }

        public override string GetSettingKey()
        {
            return "RegisterMeal_CancelByDate";
        }

        public override bool IsValid()
        {
            return SysId > 0;
        }
    }
}
