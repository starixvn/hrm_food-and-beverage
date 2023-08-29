namespace NexFB.Criteria.SmartCardOrder
{
    using Fanex.Data.Repository;
    public class GetMenuShowOnScreenCriteria : CriteriaBase
    {
        public override bool IsValid() => true;

        public override string GetSettingKey() => "POS_GetMenu_ShowOnScreen";
    }
}
