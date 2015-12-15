// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateGroupSupervisorCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.9.21">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Fanex.Data.Repository;
    using NexFB.WindowsService.SynchronizeDataService;

    public class InsertAndUpdateGroupSupervisorCriteria : CriteriaBase
    {
        private const string _SupervisorTpl =
            "<Supervisor OwnerId='{0}' GroupId='{1}' ActionType='{2}' RequestType='{3}' ModifiedDate='{4}' IsDeleted='{5}'></Supervisor>";

        [SpParam(Ignored = true)]
        public IList<GroupSupervisorModel> Supervisors { get; set; }

        public string SupervisorXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (Supervisors != null)
                {
                    foreach (GroupSupervisorModel supervisor in Supervisors)
                    {
                        value.AppendFormat(
                            _SupervisorTpl,
                            supervisor.OwnerId,
                            supervisor.GroupId,
                            supervisor.ActionType,
                            supervisor.RequestType,
                            supervisor.ModifiedDate,
                            supervisor.IsDeleted);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "Sync_GroupSupervisor";
        }

        public override bool IsValid()
        {
            return Supervisors != null && Supervisors.Any();
        }
    }
}
