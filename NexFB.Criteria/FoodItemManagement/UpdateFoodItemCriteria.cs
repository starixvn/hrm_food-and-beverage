namespace NexFB.Criteria.FoodItemManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update food item criteria.
    /// </summary>
    public class UpdateFoodItemCriteria : CriteriaBase
    {
        public int FoodId { get; set; }

        [SpParam(SqlParamName = "FoodSetID")]
        public string SetMenuId { get; set; }

        public string FoodVnmName { get; set; }

        public string FoodEngName { get; set; }

        public string FoodCode { get; set; }

        public string FoodPhoto { get; set; }

        public bool? IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Food_EditByFoodID";
        }

        public override bool IsValid()
        {
            return FoodId > 0 && !string.IsNullOrEmpty(FoodEngName) && 
                !string.IsNullOrEmpty(FoodVnmName) && !string.IsNullOrEmpty(FoodCode);
        }
    }
}
