// --------------------------------------------------------------------------
// <copyright file="GetLeaveTypeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.8.27">Created</change>
// </history>

namespace NexFB.WebService.Criteria
{
    using Fanex.Data.Repository;

    public class GetLeaveTypeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "LeaveType_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
