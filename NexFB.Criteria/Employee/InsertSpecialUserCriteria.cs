namespace NexFB.Criteria.Employee
{
    using System;
    using Fanex.Data.Repository;

    public class InsertSpecialUserCriteria : CriteriaBase
    {
        public string Code { get; set; }

        public string NickName { get; set; }

        [SpParam(SqlParamName = "CardNumber")]
        public string EmployeeCard { get; set; }

        public short CompanyId { get; set; }

        public short EmployeeTypeId { get; set; }

        public bool IsActive { get; set; }

        [SpParam(Ignored = true)]
        public DateTime CreatedDate { get; set; }

        [SpParam(Ignored = true)]
        public int CreatedBy { get; set; }

        public override string GetSettingKey() => "Employee_Ins_SpecialEmp";

        public override bool IsValid()
            => !string.IsNullOrEmpty(Code) && !string.IsNullOrEmpty(NickName) && CompanyId > 0 && EmployeeTypeId > 0;
    }
}