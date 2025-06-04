namespace NexFB.Criteria.AlternativeMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Insert alternative menu criteria.
    /// </summary>
    public class InsertAlternativeMenuCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "AltMenuCode")]
        public string AlternativeMenuCode { get; set; }

        [SpParam(SqlParamName = "AltMenuEngName")]
        public string AlternativeMenuEngName { get; set; }

        [SpParam(SqlParamName = "AltMenuVNMName")]
        public string AlternativeMenuVnmName { get; set; }

        [SpParam(SqlParamName = "AltMenuPhoto")]
        public string AlternativeMenuPhoto { get; set; }

        public string EmployeeTypeId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public override string GetSettingKey()
        {
            return "AlternativeMenu_Insert";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
