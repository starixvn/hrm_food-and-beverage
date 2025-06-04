namespace NexFB.Criteria.Ordering
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// DeleteOrder Criteria.
    /// </summary>
    public class DeleteOrderCriteria : CriteriaBase
    {
        public int OrderId { get; set; }

        public string Member { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Cancel_RequestByWeb";
        }

        public override bool IsValid()
        {
            return OrderId > 0;
        }
    }
}
