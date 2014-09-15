// --------------------------------------------------------------------------
// <copyright file="InsertOrderCriteria.cs" company="Nexcel Solutions Vietnam">
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
    using NexFB.Core.Extensions;
    using NexFB.Models.Ordering;

    /// <summary>
    /// InsertOrder Criteria.
    /// </summary>
    public class InsertOrderCriteria : CriteriaBase
    {
        private const string _MemberTpl = "<User SysId='{0}' IsOrder='{1}'></User>";

        [Output]
        public int OrderNo { get; set; }

        public int LocationId { get; set; }

        public string Phone { get; set; }

        public string Remark { get; set; }

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
                        value.AppendFormat(_MemberTpl, member.Member.UserId, member.IsOrder);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "MealOrder_Insert_RequestByWeb";
        }

        public override bool IsValid()
        {
            return Members != null && Members.Any();
        }
    }
}
