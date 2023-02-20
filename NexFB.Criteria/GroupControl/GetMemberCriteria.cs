namespace NexFB.Criteria.GroupControl
{
    using System;
    using Fanex.Data.Repository;

    public class GetMemberCriteria : CriteriaBase
    {
        public string CompanyId { get; set; }

        public string EmployeeTypeId { get; set; }

        public string TeamId { get; set; }

        public string Query { get; set; }

        public string RoleId { get; set; }

        public string GroupId { get; set; }

        public DateTime? JoinedDateFrom { get; set; }

        public DateTime? JoinedDateTo { get; set; }

        public override string GetSettingKey() => "GroupOrderAdmin_GetMember";

        public override bool IsValid() => true;
    }
}