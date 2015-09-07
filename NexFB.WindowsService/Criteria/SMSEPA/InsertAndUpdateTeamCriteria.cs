// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateTeamCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.8.27">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;
    using NexFB.WindowsService.SynchronizeDataService;

    public class InsertAndUpdateTeamCriteria : CriteriaBase
    {
        private const string _UserTpl = "<Team TeamId='{0}' Code='{1}' "
            + "Name='{2}' LeaderId='{3}' AssistantLeaderId='{4}' Rank='{5}' IsDeleted='{6}' ModifiedDate='{7}' DepartmentId='{8}'></Team>";

        [SpParam(Ignored = true)]
        public IList<TeamModel> Teams { get; set; }

        public string TeamXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (Teams != null)
                {
                    foreach (TeamModel team in Teams)
                    {
                        value.AppendFormat(
                            _UserTpl,
                            team.TeamId,
                            team.Code,
                            team.Name,
                            team.LeaderId,
                            team.AssistantLeaderId,
                            team.Rank,
                            team.IsDeleted,
                            team.ModifiedDate,
                            team.DepartmentId);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "Sync_Group";
        }

        public override bool IsValid()
        {
            return Teams != null && Teams.Any();
        }
    }
}
