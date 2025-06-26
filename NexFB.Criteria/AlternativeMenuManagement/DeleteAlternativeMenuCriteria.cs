namespace NexFB.Criteria.AlternativeMenuManagement
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete alternative menu criteria.
    /// </summary>
    public class DeleteAlternativeMenuCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "AltMenuID")]
        public int AlternativeMenuId { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int ModifiedBy { get; set; }

        public override string GetSettingKey()
        {
            return "AlternativeMenu_Delete";
        }

        public override bool IsValid()
        {
            return AlternativeMenuId > 0;
        }
    }
}
