// ----------------------------------------------------------------------
// <copyright file="GetAllSystemRoleCriteria.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/26/2014 1:40:12 PM">Created.</change>
// </history>

namespace NexFB.Criteria.UserControl
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all system role criteria.
    /// </summary>
    public class GetAllSystemRoleCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "SystemRole_GetAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
