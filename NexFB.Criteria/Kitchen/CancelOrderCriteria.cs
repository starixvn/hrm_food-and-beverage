namespace NexFB.Criteria.Kitchen
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Cancel Order Criteria.
    /// </summary>
    public class CancelOrderCriteria : CriteriaBase
    {
        public int OrderId { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public override bool IsValid()
        {
            return OrderId > 0;
        }

        public override string GetSettingKey()
        {
            return "POS_Cancel_OrderMeal";
        }
    }
}
