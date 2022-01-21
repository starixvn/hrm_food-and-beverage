namespace NexFB.Criteria.Employee
{
    using System;
    using Fanex.Data.Repository;

    public class UpdateSpecialUserCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int UserId { get; set; }

        public string Code { get; set; }

        public string NickName { get; set; }

        [SpParam(SqlParamName = "CardNumber")]
        public string EmployeeCard { get; set; }

        public int DepartmentId { get; set; }

        public int EmployeeTypeId { get; set; }

        public bool IsActive { get; set; }

        [SpParam(Ignored = true)]
        public int ModifiedBy { get; set; }

        [SpParam(Ignored = true)]
        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey() => "Employee_Upd_SpecialEmp";

        public override bool IsValid() => IsValidCodeAndNickname() && DepartmentId > 0 && EmployeeTypeId > 0 && UserId > 0;

        private bool IsValidCodeAndNickname() => !string.IsNullOrWhiteSpace(Code) && !string.IsNullOrWhiteSpace(NickName);
    }
}