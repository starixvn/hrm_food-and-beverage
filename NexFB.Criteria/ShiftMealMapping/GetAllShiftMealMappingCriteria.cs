namespace NexFB.Criteria.ShiftMealMapping
{
    using Fanex.Data.Repository;

    public class GetAllShiftMealMappingCriteria : CriteriaBase
    {
        public string CompanyId { get; set; }
        public string ShiftCode { get; set; }
        public string TeamCode { get; set; }

        public override string GetSettingKey() => "Shift_GetAll";

        public override bool IsValid() => true;
    }
}