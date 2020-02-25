namespace NexFB.Criteria.FoodMenuManagement
{
    using Fanex.Data.Repository;

    public class GetFoodMenuHistoryCriteria : CriteriaBase
    {
        public int MenuId { get; set; }

        public override string GetSettingKey()
        {
            return "FoodMenu_GetHistoryByMenuID";
        }

        public override bool IsValid()
        {
            return MenuId > 0;
        }
    }
}