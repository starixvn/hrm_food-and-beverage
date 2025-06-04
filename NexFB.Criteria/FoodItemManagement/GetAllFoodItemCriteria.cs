namespace NexFB.Criteria.FoodItemManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all food item management criteria.
    /// </summary>
    public class GetAllFoodItemCriteria : CriteriaBase
    {
        public int? FoodId { get; set; }

        [SpParam(SqlParamName = "FoodSetID")]
        public string SetMenuId { get; set; }

        public bool? IsActive { get; set; }

        public override string GetSettingKey()
        {
            return "Food_GetItem";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
