// --------------------------------------------------------------------------
// <copyright file="GetJobTitleCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.8.27">Created</change>
// </history>

namespace NexFB.WebService.Criteria
{
    using Fanex.Data.Repository;

    public class GetJobTitleCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "JobTitle_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
