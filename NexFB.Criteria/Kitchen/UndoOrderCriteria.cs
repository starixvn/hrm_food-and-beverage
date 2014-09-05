// --------------------------------------------------------------------------
// <copyright file="UndoOrderCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.9.5">Created</change>
// </history>

namespace NexFB.Criteria.Kitchen
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Undo Order Criteria.
    /// </summary>
    public class UndoOrderCriteria : CriteriaBase
    {
        public int OrderId { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public override bool IsValid()
        {
            return OrderId > 0;
        }

        public override string GetSettingKey()
        {
            return "POS_UndoOrder";
        }
    }
}
