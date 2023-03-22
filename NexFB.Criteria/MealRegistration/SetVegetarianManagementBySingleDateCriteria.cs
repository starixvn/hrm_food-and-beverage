namespace NexFB.Criteria.MealRegistration
{
    using Fanex.Data.Repository;
    using System;

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
