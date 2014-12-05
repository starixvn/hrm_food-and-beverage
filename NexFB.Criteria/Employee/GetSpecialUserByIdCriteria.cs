// --------------------------------------------------------------------------
// <copyright file="GetSpecialUserByIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.12.5">Created</change>
// </history>

namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get Special Employee by id Criteria.
    /// </summary>
    public class GetSpecialUserByIdCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_GetEmployees";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}
