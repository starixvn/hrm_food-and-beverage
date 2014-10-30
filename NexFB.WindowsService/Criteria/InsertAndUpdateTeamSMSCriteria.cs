// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateTeamSMSCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.10.2">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;

    /// <summary>
    /// InsertAndUpdateTeamSMS Criteria.
    /// </summary>
    public class InsertAndUpdateTeamSmsCriteria : CriteriaBase
    {
        private const string _UserTpl = "<Team TeamId='{0}' DepartmentId='{1}' Code='{2}' "
            + "Name='{3}' IsDeleted='{4}' ModifiedDate='{5}'></Team>";

        [SpParam(Ignored = true)]
        public int DepartmentId { get; set; }

        [SpParam(Ignored = true)]
        public IList<SMSService.TeamModel> Teams { get; set; }

        public string TeamXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (Teams != null)
                {
                    foreach (SMSService.TeamModel team in Teams)
                    {
                        value.AppendFormat(
                            _UserTpl,
                            team.TeamId,
                            DepartmentId,
                            team.Code,
                            team.Name,
                            team.IsDeleted,
                            team.ModifiedDate);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "API_SYN_Team";
        }

        public override bool IsValid()
        {
            return DepartmentId > 0 && Teams != null && Teams.Any();
        }
    }
}
