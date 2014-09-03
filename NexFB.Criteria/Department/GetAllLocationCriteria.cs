// --------------------------------------------------------------------------
// <copyright file="GetAllLocationCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.9.1">Created</change>
// </history>

namespace NexFB.Criteria.Department
{
    using Fanex.Data.Repository;

    /// <summary>
    /// GetAllLocation Criteria.
    /// </summary>
    public class GetAllLocationCriteria : CriteriaBase
    {
        public short? LocationId { get; set; }

        public override string GetSettingKey()
        {
            return "Location_GetAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
