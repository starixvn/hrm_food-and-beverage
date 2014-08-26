// ----------------------------------------------------------------------
// <copyright file="GetFunctionByRoleCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/26/2014 12:51:32 PM">Created.</change>
// </history>

namespace NexFB.Criteria.UserControl
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all function by role criteria.
    /// </summary>
    public class GetFunctionByRoleCriteria : CriteriaBase
    {
        public int RoleId { get; set; }

        public override string GetSettingKey()
        {
            return "Function_GetByRole";
        }

        public override bool IsValid()
        {
            return RoleId > 0;
        }
    }
}
