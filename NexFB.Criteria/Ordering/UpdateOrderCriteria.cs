// --------------------------------------------------------------------------
// <copyright file="UpdateOrderCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.10">Created</change>
// </history>

namespace NexFB.Criteria.Ordering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;
    using NexFB.Models.Ordering;

    /// <summary>
    /// UpdateOrder Criteria.
    /// </summary>
    public class UpdateOrderCriteria : CriteriaBase
    {
        private const string _MemberTpl = "<User SysId='{0}' MenuID='{1}' IsMainMenu='{2}' IsOrder='{3}'></User>";

        public int OrderId { get; set; }

        public int? LocationId { get; set; }

        public string Phone { get; set; }

        public string Remark { get; set; }

        public string RemarkVNM { get; set; }

        public bool TakeAway { get; set; }

        [SpParam(SqlParamName = "IsSaveOrder")]
        public bool IsSavedOrder { get; set; }

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
                        value.AppendFormat(_MemberTpl, member.Member.UserId, member.Menus.First().MenuId, member.Menus.First().IsMainMenu, member.IsOrder);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "MealOrder_Edit_RequestByWeb";
        }

        public override bool IsValid()
        {
            return OrderId > 0;
        }
    }
}
