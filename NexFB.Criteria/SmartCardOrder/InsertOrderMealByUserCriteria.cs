// --------------------------------------------------------------------------
namespace NexFB.Criteria.SmartCardOrder
{
    using System;
    using Fanex.Data.Repository;
    using NexFB.Core.Extensions;

    public class InsertOrderMealByUserCriteria : CriteriaBase
    {
        [Output]
        public int OrderNo { get; set; }

        [Output]
        public int OrderId { get; set; }

        public string ListDetail { get; set; }

        public string Foods { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public bool TakeAway { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ListDetail);
        }

        public override string GetSettingKey()
        {
            return "POS_Insert_OrderMealByUser";
        }
    }
}