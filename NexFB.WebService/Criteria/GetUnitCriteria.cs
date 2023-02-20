namespace NexFB.WebService.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetUnitCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }

        public override string GetSettingKey() => "Unit_GetByModifiedDate";

        public override bool IsValid() => true;
    }
}