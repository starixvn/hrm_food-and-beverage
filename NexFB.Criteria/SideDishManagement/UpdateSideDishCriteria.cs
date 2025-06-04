namespace NexFB.Criteria.SideDishManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update side dish criteria.
    /// </summary>
    public class UpdateSideDishCriteria : CriteriaBase
    {
        public int FoodId { get; set; }

        public string FoodCode { get; set; }

        public string FoodEngName { get; set; }

        public string FoodVnmName { get; set; }

        public string FoodPhoto { get; set; }

        public bool? IsActive { get; set; }

        public bool KitchenOnly { get; set; }

        public int UnitId { get; set; }

        public double Price { get; set; }

        public string EmployeeTypeId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodAdd_EditByFoodID";
        }

        public override bool IsValid()
        {
            return FoodId > 0 && !string.IsNullOrEmpty(FoodCode) && !string.IsNullOrEmpty(FoodVnmName) &&
                UnitId > 0 && !string.IsNullOrEmpty(EmployeeTypeId);
        }
    }
}
