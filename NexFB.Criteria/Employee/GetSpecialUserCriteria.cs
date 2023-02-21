namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    public class GetSpecialUserCriteria : CriteriaBase
    {
        public string CompanyId { get; set; }

        public string EmployeeTypeId { get; set; }

        [SpParam(SqlParamName = "Status")]
        public bool? IsActive { get; set; }

        public override string GetSettingKey() => "Employee_Sel_SpecialEmp";

        public override bool IsValid() => true;
    }
}