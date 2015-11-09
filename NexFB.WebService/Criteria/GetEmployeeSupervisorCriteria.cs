// --------------------------------------------------------------------------
// <copyright file="GetEmployeeSupervisorCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.9.21">Created</change>
// </history>

namespace NexFB.WebService.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetEmployeeSupervisorCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }

        public override string GetSettingKey()
        {
            return "EmployeeSupervisor_GetForSync";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
