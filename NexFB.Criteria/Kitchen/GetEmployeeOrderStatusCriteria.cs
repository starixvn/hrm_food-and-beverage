// --------------------------------------------------------------------------
// <copyright file="GetEmployeeOrderStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.11.24">Created</change>
// </history>

namespace NexFB.Criteria.Kitchen
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetEmployeeOrderStatusCriteria : CriteriaBase
    {
        public int SysId { get; set; }

        public override bool IsValid()
        {
            return SysId > 0;
        }

        public override string GetSettingKey()
        {
            return "POS_Get_MyStatusBySysID";
        }
    }
}
