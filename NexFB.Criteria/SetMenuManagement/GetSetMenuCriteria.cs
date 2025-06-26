namespace NexFB.Criteria.SetMenuManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get set menu criteria.
    /// </summary>
    public class GetSetMenuCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "MenuGroupID")]
        public string MenuId { get; set; }

        public bool? IsActive { get; set; }

        public override string GetSettingKey()
        {
            return "FoodSet_GetItem";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
