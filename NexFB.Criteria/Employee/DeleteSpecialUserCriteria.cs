// --------------------------------------------------------------------------
// <copyright file="DeleteSpecialUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.12.5">Created</change>
// </history>

namespace NexFB.Criteria.Employee
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Delete Special Employee criteria.
    /// </summary>
    public class DeleteSpecialUserCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "Food_DeleteByFoodID";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
