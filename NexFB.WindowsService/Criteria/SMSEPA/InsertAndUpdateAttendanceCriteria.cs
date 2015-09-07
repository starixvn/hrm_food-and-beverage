// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateAttendanceCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2015.8.28">Created</change>
// </history>

namespace NexFB.WindowsService.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;
    using NexFB.WindowsService.SynchronizeDataService;

    public class InsertAndUpdateAttendanceCriteria : CriteriaBase
    {
        private const string _AttendanceTpl =
            "<Attendance EmployeeId='{0}' WorkDate='{1}' ShiftId='{2}' InFixed='{3}' OutFixed='{4}' LateIn='{5}' EarlyOut='{6}'"
            + " LeaveTypeId='{7}' LeaveTypeId2='{8}' Absence='{9}' TotalWorkHours='{10}' LeaveDay='{11}' ModifiedDate='{12}'></Attendance>";

        [SpParam(Ignored = true)]
        public IList<EmployeeAttendanceModel> EmployeeAttendances { get; set; }

        public string AttendanceXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (EmployeeAttendances != null)
                {
                    foreach (EmployeeAttendanceModel attendance in EmployeeAttendances)
                    {
                        value.AppendFormat(
                            _AttendanceTpl,
                            attendance.UserId,
                            attendance.WorkDate,
                            attendance.ShiftId,
                            attendance.InFixed,
                            attendance.OutFixed,
                            attendance.LateIn,
                            attendance.EarlyOut,
                            attendance.LeaveTypeId,
                            attendance.LeaveTypeId2,
                            attendance.Absence,
                            attendance.TotalWorkHours,
                            attendance.LeaveDay,
                            attendance.ModifiedDate);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "Sync_Attendance";
        }

        public override bool IsValid()
        {
            return EmployeeAttendances != null && EmployeeAttendances.Any();
        }
    }
}
