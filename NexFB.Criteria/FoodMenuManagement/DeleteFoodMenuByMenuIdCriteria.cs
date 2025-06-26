namespace NexFB.Criteria.FoodMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete food menu by menu Id criteria.
    /// </summary>
    public class DeleteFoodMenuByMenuIdCriteria : CriteriaBase
    {
        public int MenuId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodMenu_DeleteByMenuID";
        }

        public override bool IsValid()
        {
            return MenuId > 0;
        }
    }
}
