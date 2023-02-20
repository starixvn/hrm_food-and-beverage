namespace NexFB.WebService.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetSectionCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }

        public override string GetSettingKey() => "Section_GetByModifiedDate";

        public override bool IsValid() => true;
    }
}