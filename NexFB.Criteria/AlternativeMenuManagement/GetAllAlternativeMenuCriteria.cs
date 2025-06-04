namespace NexFB.Criteria.AlternativeMenuManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all alternative menu criteria.
    /// </summary>
    public class GetAllAlternativeMenuCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "AltMenuID")]
        public int? AlternativeMenuId { get; set; }

        public bool? IsActive { get; set; }

        public override string GetSettingKey()
        {
            return "AlternativeMenu_GetItem";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
