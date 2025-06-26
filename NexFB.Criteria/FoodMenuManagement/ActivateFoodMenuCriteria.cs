namespace NexFB.Criteria.FoodMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Activate or deactivate food menu criteria.
    /// </summary>
    public class ActivateFoodMenuCriteria : CriteriaBase
    {
        public int MenuId { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "FoodMenu_EditActiveByMenuID";
        }

        public override bool IsValid()
        {
            return MenuId > 0;
        }
    }
}
