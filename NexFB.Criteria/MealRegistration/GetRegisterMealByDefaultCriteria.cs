namespace NexFB.Criteria.MealRegistration
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get register meal by default criteria.
    /// </summary>
    public class GetRegisterMealByDefaultCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysId")]
        public int SystemId { get; set; }

        public override string GetSettingKey()
        {
            return "RegisterMeal_GetDefault";
        }

        public override bool IsValid()
        {
            return SystemId > 0;
        }
    }
}
