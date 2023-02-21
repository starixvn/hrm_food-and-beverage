namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    public class GetEmployeeByCodeCriteria : CriteriaBase
    {
        public string Code { get; set; }

        public string CompanyId { get; set; }

        public override string GetSettingKey() => "Employee_GetEmployeeByCode";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(Code);
    }
}