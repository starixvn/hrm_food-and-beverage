// ----------------------------------------------------------------------
// <copyright file="UpdateOrderSideDishCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/22/2014 2:53:04 PM">Created.</change>
// </history>

namespace NexFB.Criteria.Ordering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;
    using NexFB.Models.Entities;
    using NexFB.Models.Ordering;

    /// <summary>
    /// Update Order Side Dish Criteria.
    /// </summary>
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

        public override string GetSettingKey()
        {
            return "MealOrder_Edit_RequestSideDishByWeb";
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
