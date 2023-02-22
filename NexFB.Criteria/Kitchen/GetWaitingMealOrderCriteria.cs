namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    public class GetWaitingMealOrderCriteria : CriteriaBase
    {
        public override bool IsValid() => true;

        public override string GetSettingKey() => "POS_Get_ListRequestNewByPOS";
    }
}