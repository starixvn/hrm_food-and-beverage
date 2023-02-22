namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    public class GetOrderDetailsCriteria : CriteriaBase
    {
        public int OrderId { get; set; }

        public override bool IsValid() => OrderId > 0;

        public override string GetSettingKey() => "POS_Get_OrderDetail";
    }
}