namespace NexFB.Criteria.Ordering
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get last modified date by order Id criteria.
    /// </summary>
    public class GetLastMofifiedDateByOrderIdCriteria : CriteriaBase
    {
        public int OrderId { get; set; }

        public override string GetSettingKey()
        {
            return "MealOrder_Get_ModifiedByOrderID";
        }

        public override bool IsValid()
        {
            return OrderId > 0;
        }
    }
}
