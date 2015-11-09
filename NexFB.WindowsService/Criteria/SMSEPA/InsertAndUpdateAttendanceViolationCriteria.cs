// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateAttendanceViolationCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.9.14">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Fanex.Data.Repository;
    using NexFB.WindowsService.SynchronizeDataService;

    public class InsertAndUpdateAttendanceViolationCriteria : CriteriaBase
    {
        private const string _VialationTpl =
            "<Violation EmpId='{0}' WorkDate='{1}' MissingCheckInOut='{2}' LateIn='{3}' EarlyOut='{4}' ChangeShift='{5}' WorkingBelowTime='{6}'"
            + " Absent='{7}' UnpaidLeaveAbsent='{8}' UnpaidLeave='{9}' ShiftName='{10}' InFixed='{11}' OutFixed='{12}'"
            + " LateInValue='{13}' EarlyOutValue='{14}' TotalWorkHours='{15}' LateInApproverComment='{16}' LateInEmployeeComment='{17}'"
            + " EarlyOutApproverComment='{18}' EarlyOutEmployeeComment='{19}' ChangeShiftApproverComment='{20}' ChangeShiftEmployeeComment='{21}'"
            + " LeaveApproverComment='{22}' LeaveEmployeeComment='{23}' ModifiedDate='{24}'></Violation>";

        [SpParam(Ignored = true)]
        public IList<EmployeeAttendanceViolationModel> AttendanceViolations { get; set; }

        public string VialationXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (AttendanceViolations != null)
                {
                    foreach (EmployeeAttendanceViolationModel violation in AttendanceViolations)
                    {
                        value.AppendFormat(
                            _VialationTpl,
                            violation.UserId,
                            violation.WorkDate,
                            violation.MissingCheckInOut,
                            violation.LateIn,
                            violation.EarlyOut,
                            violation.ChangeShift,
                            violation.WorkingBelowTime,
                            violation.Absent,
                            violation.UnpaidLeaveAbsent,
                            violation.UnpaidLeave,
                            HttpUtility.HtmlEncode(violation.ShiftName),
                            violation.InFixed,
                            violation.OutFixed,
                            violation.LateInValue,
                            violation.EarlyOutValue,
                            violation.TotalWorkHours == null ? "0" : violation.TotalWorkHours.ToString(),
                            HttpUtility.HtmlEncode(violation.LateInApproverComment),
                            HttpUtility.HtmlEncode(violation.LateInEmployeeComment),
                            HttpUtility.HtmlEncode(violation.EarlyOutApproverComment),
                            HttpUtility.HtmlEncode(violation.EarlyOutEmployeeComment),
                            HttpUtility.HtmlEncode(violation.ChangeShiftApproverComment),
                            HttpUtility.HtmlEncode(violation.ChangeShiftEmployeeComment),
                            HttpUtility.HtmlEncode(violation.LeaveApproverComment),
                            HttpUtility.HtmlEncode(violation.LeaveEmployeeComment),
                            violation.ModifiedDate);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "Sync_AttendanceViolation";
        }

        public override bool IsValid()
        {
            return AttendanceViolations != null && AttendanceViolations.Any();
        }
    }
}
