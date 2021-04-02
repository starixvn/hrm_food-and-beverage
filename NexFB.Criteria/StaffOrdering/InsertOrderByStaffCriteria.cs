using Fanex.Data.Repository;
using NexFB.Core.Extensions;
using NexFB.Models.Ordering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NexFB.Criteria.StaffOrdering
{
    public class InsertOrderByStaffCriteria : CriteriaBase
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
        public int StatusID { get; set; }

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
            return "MealOrder_InsertBystaff_RequestByWeb";
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

            return LocationId != null && Members != null && check && Members.Any();
        }
    }
}
