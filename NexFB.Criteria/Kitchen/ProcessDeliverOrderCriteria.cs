namespace NexFB.Criteria.Kitchen
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Process Deliver Order Criteria.
    /// </summary>
    public class ProcessDeliverOrderCriteria : CriteriaBase
    {
        public int OrderId { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public override bool IsValid()
        {
            return OrderId > 0;
        }

        public override string GetSettingKey()
        {
            return "POS_DeliverOrder";
        }
    }
}
