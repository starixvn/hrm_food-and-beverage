namespace NexFB.WCFService.Criteria
{
    using Fanex.Data.Repository;

    public class GetShiftCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Shift_Get";

        public override bool IsValid() => true;
    }
}