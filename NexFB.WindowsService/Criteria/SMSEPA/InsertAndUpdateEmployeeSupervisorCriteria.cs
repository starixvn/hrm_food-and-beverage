// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateEmployeeSupervisorCriteria.cs" company="Nexcel Solutions Vietnam">
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

    public class InsertAndUpdateEmployeeSupervisorCriteria : CriteriaBase
    {
        private const string _SupervisorTpl =
            "<Supervisor OwnerId='{0}' EmpId='{1}' ActionType='{2}' RequestType='{3}' ModifiedDate='{4}' OrderNo='{5}' OrderCode='{6}' IsDeleted='{7}'></Supervisor>";

        [SpParam(Ignored = true)]
        public IList<EmployeeSupervisorModel> Supervisors { get; set; }

        public string SupervisorXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (Supervisors != null)
                {
                    foreach (EmployeeSupervisorModel supervisor in Supervisors)
                    {
                        value.AppendFormat(
                            _SupervisorTpl,
                            supervisor.OwnerId,
                            supervisor.EmpId,
                            supervisor.ActionType,
                            supervisor.RequestType,
                            supervisor.ModifiedDate,
                            supervisor.OrderNo,
                            HttpUtility.HtmlEncode(supervisor.OrderCode),
                            supervisor.IsDeleted);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "Sync_EmployeeSupervisor";
        }

        public override bool IsValid()
        {
            return Supervisors != null && Supervisors.Any();
        }
    }
}
