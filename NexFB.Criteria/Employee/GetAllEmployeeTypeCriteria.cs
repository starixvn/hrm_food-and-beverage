namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    public class GetAllEmployeeTypeCriteria : CriteriaBase
    {
        public string CompanyId { get; set; }

        public bool? IsLocal { get; set; }

        public override string GetSettingKey() => "Employee_GetEmployeeType";

        public override bool IsValid() => true;
    }
}