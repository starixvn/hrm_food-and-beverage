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
                return DepartmentId == ((int)DepartmentIdentity.Nexcel).ToString()
                    ? string.Format("spvn\\{0}", Username)
                    : string.Format("luminet\\{0}", Username);
            } 
        }

        public string DepartmentId { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_GetEmployeeByDomain";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(DomainUsername);
        }
    }
}
