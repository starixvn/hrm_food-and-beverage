namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;
    using NexFB.Core.Enums;

    public class GetEmployeeByDomainCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public string Username { get; set; }

        public string DomainUsername
        {
            get
            {
                return CompanyId == ((byte)CompanyIdentity.Nexcel).ToString()
                    ? string.Format("spvn\\{0}", Username)
                    : string.Format("lumiplus\\{0}", Username);
            }
        }

        public string CompanyId { get; set; }

        public override string GetSettingKey() => "Employee_GetEmployeeByDomain";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(DomainUsername);
    }
}