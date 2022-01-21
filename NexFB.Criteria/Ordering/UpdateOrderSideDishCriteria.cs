namespace NexFB.Criteria.Ordering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;

    public class UpdateOrderSideDishCriteria : CriteriaBase
    {
        private const string _MemberTpl = "<User SysId='{0}' FoodID='{1}' QTY='{2}'></User>";

        public int OrderId { get; set; }

        public int? LocationId { get; set; }

        public string Phone { get; set; }

        public string Remark { get; set; }

        public string RemarkVNM { get; set; }

        public bool TakeAway { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [SpParam(Ignored = true)]
        public IList<OrderMemberModel> Members { get; set; }

        public string ListDetail
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (Members != null)
                {
                    foreach (OrderMemberModel member in Members)
                    {
                        int userId = member.Member.UserId;

                        foreach (Menu menu in member.Menus)
                        {
                            value.AppendFormat(_MemberTpl, userId, menu.MenuId, menu.QuantityOrder);
                        }
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey() => "MealOrder_Edit_RequestSideDishByWeb";

        public override bool IsValid() => OrderId > 0 && LocationId != null && IsValidMembers();

        private bool IsValidMembers() => Members != null && Members.Any() && Members.Any(x => x.IsOrder);
    }
}