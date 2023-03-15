namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    public class GetEmployeeByUserIdCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string CompanyId { get; set; }

        public override string GetSettingKey() => "Employee_GetByUserId";

        public override bool IsValid() => UserId > 0;
    }
}