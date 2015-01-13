// --------------------------------------------------------------------------
// <copyright file="GetEmployeeByDomainCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.26">Created</change>
// </history>

namespace NexFB.Criteria.Employee
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetEmployeeByDomain Criteria.
    /// </summary>
    public class GetEmployeeByDomainCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public string Username { get; set; }

        public string DomainUsername 
        { 
            get 
            {
                return Username.Contains("spvn") ? Username : string.Format("spvn\\{0}", Username);
            } 
        }

        public string DepartmentId { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_GetEmployeeByDomain";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(DomainUsername);
        }
    }
}
