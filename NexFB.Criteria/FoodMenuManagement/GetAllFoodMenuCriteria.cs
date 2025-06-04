namespace NexFB.Criteria.FoodMenuManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all food menu criteria.
    /// </summary>
    public class GetAllFoodMenuCriteria : CriteriaBase
    {
        public int? MenuId { get; set; }

        public string MealId { get; set; }

        public string EmployeeTypeId { get; set; }

        public bool? IsActive { get; set; }

        public override string GetSettingKey()
        {
            return "FoodMenu_GetAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
