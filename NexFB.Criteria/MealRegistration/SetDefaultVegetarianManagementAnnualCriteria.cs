namespace NexFB.Criteria.MealRegistration
{
    using Fanex.Data.Repository;
    public class SetDefaultVegetarianManagementAnnualCriteria : CriteriaBase
    {
        public string DateApplyAllMeal { get; set; }
        public string DateApplySupperMeal { get; set; }
        public override string GetSettingKey()
        {
            return "VegetarianManagement_SetDefaultAnnual";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(DateApplyAllMeal) || !string.IsNullOrEmpty(DateApplySupperMeal);
        }
    }
}
