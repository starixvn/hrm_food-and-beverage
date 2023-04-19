namespace NexFB.Criteria.MealRegistration
{
    using System;
    using Fanex.Data.Repository;

    public class SetVegetarianByDateCriteria : CriteriaBase
    {
        public int EmpId { get; set; }

        public int CompanyId { get; set; }

        public DateTime OnDate { get; set; }

        public override string GetSettingKey()
        {
            return "RegisterMeal_SetVegetarian";
        }

        public override bool IsValid()
        {
            return EmpId > 0;
        }
    }
}
