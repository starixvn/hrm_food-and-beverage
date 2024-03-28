namespace NexFB.WCFService.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetDepartmentCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }

        public override string GetSettingKey() => "Department_GetByModifiedDate";

        public override bool IsValid() => true;
    }
}