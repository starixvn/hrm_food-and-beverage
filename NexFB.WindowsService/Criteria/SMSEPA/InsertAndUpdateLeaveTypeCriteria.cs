// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateLeaveTypeCriteria.cs" company="Nexcel Solutions Vietnam">
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
    using System.Web;
    using Fanex.Data.Repository;
    using NexFB.WindowsService.SynchronizeDataService;

    public class InsertAndUpdateLeaveTypeCriteria : CriteriaBase
    {
        private const string _LeaveTypeTpl = "<LeaveType LeaveTypeId='{0}' Code='{1}' "
            + "Name='{2}' IsDisabled='{3}'></LeaveType>";

        [SpParam(Ignored = true)]
        public IList<LeaveTypeModel> LeaveTypes { get; set; }

        public string LeaveTypeXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (LeaveTypes != null)
                {
                    foreach (LeaveTypeModel leaveType in LeaveTypes)
                    {
                        value.AppendFormat(
                            _LeaveTypeTpl,
                            leaveType.LeaveTypeId,
                            leaveType.Code,
                            HttpUtility.HtmlEncode(leaveType.Name),
                            leaveType.IsDisabled ? "1" : "0");
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "Sync_LeaveType";
        }

        public override bool IsValid()
        {
            return LeaveTypes != null && LeaveTypes.Any();
        }
    }
}
