namespace NexFB.Criteria.MealRegistration
{
    using System;
    using Fanex.Data.Repository;

    public class GetEmployeeRegisterMealByDateCriteria : CriteriaBase
    {
        public DateTime OnDate { get; set; }

        public override string GetSettingKey()
        {
            return "RegisterMeal_GetEmployeeRegisterByDate";
        }

        public override bool IsValid()
        {
            return OnDate > DateTime.MinValue;
        }
    }
}
