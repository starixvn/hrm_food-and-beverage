using System;
namespace NexFB.Criteria.Ordering
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;
    using NexFB.Models.Ordering;

    public class AdminUpdateOrderCriteria : CriteriaBase
    {
        private const string _MemberTpl = "<User SysId='{0}' MenuID='{1}' IsMainMenu='{2}' IsOrder='{3}'></User>";

        public int OrderId { get; set; }

        public int? LocationId { get; set; }

        public int MealID { get; set; }

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
                        value.AppendFormat(_MemberTpl, member.Member.UserId, member.Menus.First().MenuId, member.Menus.First().IsMainMenu, member.IsOrder);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "MealOrder_AdminEdit_RequestByWeb";
        }

        public override bool IsValid()
        {
            return OrderId > 0;
        }
    }
}
