namespace NexFB.Criteria.SideDishManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all side dish criteria.
    /// </summary>
    public class GetAllSideDishCriteria : CriteriaBase
    {
        public int? FoodId { get; set; }

        public bool? IsActive { get; set; }

        public string EmployeeTypeId { get; set; }

        public override string GetSettingKey()
        {
            return "FoodAdd_GetItem";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
