namespace NexFB.Criteria.SideDishManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all food unit criteria.
    /// </summary>
    public class GetAllFoodUnitCriteria : CriteriaBase
    {
        public int? FoodUnitId { get; set; }

        public override string GetSettingKey()
        {
            return "FoodUnit_GetAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
