namespace NexFB.Criteria.SmartCardOrder
{
    using Fanex.Data.Repository;

    public class GetSideFoodBySysIdCriteria : CriteriaBase
    {
        public int SysId { get; set; }

        public override bool IsValid()
        {
            return SysId > 0;
        }

        public override string GetSettingKey()
        {
            return "POS_Get_FoodOrder";
        }
    }
}