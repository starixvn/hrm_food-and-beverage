namespace NexFB.Criteria.FoodItemManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert food item criteria.
    /// </summary>
    public class InsertFoodItemCriteria : CriteriaBase
    {
        public string FoodEngName { get; set; }

        public string FoodVnmName { get; set; }

        public string FoodPhoto { get; set; }

        public string FoodCode { get; set; }

        [SpParam(SqlParamName = "FoodSetID")]
        public string SetMenuId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public override string GetSettingKey()
        {
            return "Food_Insert";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(FoodEngName) && !string.IsNullOrEmpty(FoodVnmName) && 
                        !string.IsNullOrEmpty(FoodCode);
        }
    }
}
