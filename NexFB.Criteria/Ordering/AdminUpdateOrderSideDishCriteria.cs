namespace NexFB.Criteria.Ordering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;

    public class AdminUpdateOrderSideDishCriteria : CriteriaBase
    {
        private const string _MemberTpl = "<User SysId='{0}' FoodID='{1}' QTY='{2}'></User>";

        public int OrderId { get; set; }

        public int? LocationId { get; set; }

        public int StatusID { get; set; }

        public string OrderFrom { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public DateTime OrderTime { get; set; }

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

        public override string GetSettingKey()
        {
            return "MealOrder_AdminEdit_RequestSideDishByWeb";
        }

        /// <summary>
        /// Function to check Is valid data.
        /// </summary>
        /// <returns>Result of check.</returns>
        public override bool IsValid()
        {
            var check = false;
            foreach (OrderMemberModel member in Members)
            {
                if (member.IsOrder)
                {
                    check = true;
                    break;
                }
            }

            return OrderId > 0 && LocationId != null && Members != null && check && Members.Any();
        }
    }
}
