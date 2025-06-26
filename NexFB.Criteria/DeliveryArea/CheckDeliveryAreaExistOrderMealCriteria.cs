namespace NexFB.Criteria.DeliveryArea
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Check delivery area exist in order meal.
    /// </summary>
    public class CheckDeliveryAreaExistOrderMealCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "LocationID")]
        public int DeliveryAreaId { get; set; }

        public string StatusId { get; set; }

        public override string GetSettingKey()
        {
            return "Location_ExistOrderMealByLocationID";
        }

        public override bool IsValid()
        {
            return DeliveryAreaId > 0;
        }
    }
}
