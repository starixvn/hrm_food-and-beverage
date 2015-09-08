// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateEmployeeCalendarCriteria.cs" company="Nexcel Solutions Vietnam">
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
    using NexFB.WindowsService.SynchronizeDataService;

    public class InsertAndUpdateEmployeeCalendarCriteria : CriteriaBase
    {
        private const string _EmployeeCalendarTpl =
            "<Calendar EmpId='{0}' WorkDate='{1}' ShiftId='{2}' IsHoliday='{3}' IsOffDay='{4}' IsLeaveDay='{5}' IsKeepInView='{6}'"
            + " LeaveTypeId='{7}' LeaveTypeId2='{8}' LeaveDay='{9}' LateIn='{10}' EarlyOut='{11}' RequestedCheckinTime='{12}'" 
            + " RequestedCheckoutTime='{13}' ChangeShiftId='{14}' ModifiedDate='{15}'></Calendar>";

        [SpParam(Ignored = true)]
        public IList<EmployeeCalendarModel> EmployeeCalendars { get; set; }

        public string CalendarXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (EmployeeCalendars != null)
                {
                    foreach (EmployeeCalendarModel calendar in EmployeeCalendars)
                    {
                        value.AppendFormat(
                            _EmployeeCalendarTpl,
                            calendar.UserId,
                            calendar.WorkDate,
                            calendar.ShiftId,
                            calendar.IsHoliday,
                            calendar.IsOffDay,
                            calendar.IsLeaveDay,
                            calendar.IsKeepInView,
                            calendar.LeaveTypeId == null ? -1 : calendar.LeaveTypeId,
                            calendar.LeaveTypeId2 == null ? -1 : calendar.LeaveTypeId2,
                            calendar.LeaveDay,
                            calendar.LateIn,
                            calendar.EarlyOut,
                            calendar.RequestedCheckinTime,
                            calendar.RequestedCheckoutTime,
                            calendar.ChangeShiftId,
                            calendar.ModifiedDate);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "Sync_Calendar";
        }

        public override bool IsValid()
        {
            return EmployeeCalendars != null && EmployeeCalendars.Any();
        }
    }
}
