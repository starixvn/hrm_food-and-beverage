namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    public class GetOrderDetailsByOrderNoCriteria : CriteriaBase
    {
        public int OrderNo { get; set; }

        public override bool IsValid() => OrderNo > 0;

        public override string GetSettingKey() => "POS_Get_OrderRequestByOrderNo";
    }
}
