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
    using Fanex.Data.Repository;
    using NexFB.WindowsService.SynchronizeDataService;

    public class InsertAndUpdateAttendanceViolationCriteria : CriteriaBase
    {
        private const string _AttendanceTpl =
            "<Violation EmpId='{0}' WorkDate='{1}' MissingCheckInOut='{2}' LateIn='{3}' EarlyOut='{4}' ChangeShift='{5}' WorkingBelowTime='{6}'"
            + " Absent='{7}' UnpaidLeaveAbsent='{8}' UnpaidLeave='{9}' ModifiedDate='{10}'></Violation>";

        [SpParam(Ignored = true)]
        public IList<EmployeeAttendanceViolationModel> AttendanceViolations { get; set; }

        public string VialationXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (AttendanceViolations != null)
                {
                    foreach (EmployeeAttendanceViolationModel attendance in AttendanceViolations)
                    {
                        value.AppendFormat(
                            _AttendanceTpl,
                            attendance.UserId,
                            attendance.WorkDate,
                            attendance.MissingCheckInOut,
                            attendance.LateIn,
                            attendance.EarlyOut,
                            attendance.ChangeShift,
                            attendance.WorkingBelowTime,
                            attendance.Absent,
                            attendance.UnpaidLeaveAbsent,
                            attendance.UnpaidLeave,
                            attendance.ModifiedDate);
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
