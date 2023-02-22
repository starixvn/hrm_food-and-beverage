namespace NexFB.Criteria.SmartCardOrder
{
    using Fanex.Data.Repository;

    public class GetAlternativeFoodBySysIdCriteria : CriteriaBase
    {
        public int? SysId { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "POS_Get_FoodAlternative";
        }
    }
}