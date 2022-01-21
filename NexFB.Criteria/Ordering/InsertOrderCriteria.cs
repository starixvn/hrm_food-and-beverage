namespace NexFB.Criteria.Ordering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;
    using NexFB.Core.Extensions;
    using NexFB.Models.Ordering;

    public class InsertOrderCriteria : CriteriaBase
    {
        private const string _MemberTpl = "<User SysId='{0}' MenuID='{1}' IsMainMenu='{2}' IsOrder='{3}'></User>";

        [Output]
        public int OrderNo { get; set; }

        [Output]
        public int OrderId { get; set; }

        public bool TakeAway { get; set; }

        public int? LocationId { get; set; }

        public string Phone { get; set; }

        public string Remark { get; set; }

        public string RemarkVNM { get; set; }

        [SpParam(SqlParamName = "IsSaveOrder")]
        public bool IsSavedOrder { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

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

        public override string GetSettingKey() => "MealOrder_Insert_RequestByWeb";

        public override bool IsValid()
        {
            return LocationId != null && Members != null && Members.Any() && Members.Any(x => x.IsOrder);
        }
    }
}