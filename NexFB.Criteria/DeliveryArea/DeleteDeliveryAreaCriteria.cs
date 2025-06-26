namespace NexFB.Criteria.DeliveryArea
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete delivery area criteria.
    /// </summary>
    public class DeleteDeliveryAreaCriteria : CriteriaBase
    {
        public int LocationId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Location_DeleteByLocationID";
        }

        public override bool IsValid()
        {
            return LocationId > 0;
        }
    }
}
