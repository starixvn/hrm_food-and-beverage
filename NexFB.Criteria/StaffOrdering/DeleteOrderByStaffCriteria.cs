namespace NexFB.Criteria.StaffOrdering
{
    using Fanex.Data.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    public class DeleteOrderByStaffCriteria : CriteriaBase
    {
        public int OrderId { get; set; }

        public string Member { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_CancelByStaff_RequestByWeb";
        }

        public override bool IsValid()
        {
            return OrderId > 0;
        }
    }
}
