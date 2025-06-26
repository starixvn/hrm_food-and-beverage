namespace NexFB.Criteria.FoodItemManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get basic information of food item criteria.
    /// </summary>
    public class GetBasicFoodItemCriteria : CriteriaBase
    {
        public int? FoodId { get; set; }

        public string MenuGroupId { get; set; }

        public bool? IsActive { get; set; }

        public override string GetSettingKey()
        {
            return "Food_GetName";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
