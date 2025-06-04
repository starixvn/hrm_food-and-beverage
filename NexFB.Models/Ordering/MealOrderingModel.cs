namespace NexFB.Models.Ordering
{
    using System;
    using System.Collections.Generic;
    using NexFB.Core.Constants;
    using NexFB.Models.Entities;

    /// <summary>
    /// MealOrdering Model.
    /// </summary>
    [Serializable]
    public class MealOrderingModel
    {
        public MealOrderingModel()
        {
            Members = new List<OrderMemberModel>();
            Location = new Location();
            CurrentMenus = new List<Menu>();
            MenuQuantity = new List<MenuQuantityModel>();
            IsShowMember = false;
        }

        public int OrderId { get; set; }

        public int OrderNo { get; set; }

        public DateTime OrderTime { get; set; }


        public DateTime? AcceptanceDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public DateTime DeliverDate { get; set; }

        public IList<OrderMemberModel> Members { get; set; }

        public int MemberCount { get; set; }

        public Employee CreatorOrder { get; set; }

        public bool TakeAway { get; set; }
        public bool? IsVegetarian { get; set; }
        public bool IsAllowOrderByShift { get; set; }

        public string Phone { get; set; }

        public int MealID { get; set; }

        public DateTime OnDate { get; set; }

        public bool RawIsAllowOrder { get; set; }

        public bool IsOrder { get; set; }

        public string Remark { get; set; }

        public string OrderFrom { get; set; }

        public Location Location { get; set; }

        public Status Status { get; set; }
        public int StatusId { get; set; }

        public DateTime ModifiedDate { get; set; }

        public IList<Menu> CurrentMenus { get; set; }

        public IList<MenuQuantityModel> MenuQuantity { get; set; }

        public bool IsSavedOrder { get; set; }

        public Employee CurrentUser { get; set; }

        public string ModifiedTime
        {
            get
            {
                return ModifiedDate.ToString(Constants.TimeFormat);
            }
        }

        public string CreatedTime
        {
            get
            {
                return OrderTime.ToString(Constants.TimeFormat);
            }
        }

        public string OnDateDeliver
        {
            get
            {
                return OnDate.ToString(Constants.DateFormat);
            }
        }

        public string DeliverDateFormat
        {
            get
            {
                return DeliverDate.ToString(Constants.TimeFormat);
            }
        }

        public string AcceptanceTime
        {
            get
            {
                return AcceptanceDate.HasValue ? ((DateTime)AcceptanceDate).ToString(Constants.TimeFormat) : string.Empty;
            }
        }

        public string DeliveryTime
        {
            get
            {
                return DeliveryDate.HasValue ? ((DateTime)DeliveryDate).ToString(Constants.TimeFormat) : string.Empty;
            }
        }

        public bool IsShowMember { get; set; }

        public bool IsSideDishOrder { get; set; }

        public bool IsTranslateRemark { get; set; }
    }
}
