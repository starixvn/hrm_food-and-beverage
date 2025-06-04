namespace NexFB.Criteria.SideDishManagement
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Side Dish By User Id Criteria.
    /// </summary>
    public class GetSideDishByUserIdCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "SysID")]
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Get_FoodOrder";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
