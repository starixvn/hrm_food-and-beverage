// --------------------------------------------------------------------------
// <copyright file="InsertAndUpdateEmployeeCalendarHMCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// InsertAndUpdateEmployeeCalendar Criteria.
    /// </summary>
    public class InsertAndUpdateEmployeeCalendarHMCriteria : CriteriaBase
    {
        private const string _EmployeeCalendarTpl =
            "<Calendar EmpId='{0}' WorkDate='{1}' DepartmentId='{2}' "
            + "ShiftId='{3}' IsHoliday='{4}' IsOffDay='{5}' IsLeaveDay='{6}' LeaveTypeId='{7}' "
            + " LeaveDay='{8}'  ModifiedDate='{9}'></Calendar>";

        [SpParam(Ignored = true)]
        public int DepartmentId { get; set; }

        [SpParam(Ignored = true)]
        public IList<HongMaService.EmployeeCalendarModel> EmployeeCalendars { get; set; }

        public string CalendarXml
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (EmployeeCalendars != null)
                {
                    foreach (HongMaService.EmployeeCalendarModel calendar in EmployeeCalendars)
                    {
                        value.AppendFormat(
                            _EmployeeCalendarTpl,
                            calendar.UserId,
                            calendar.WorkDate.ToShortDateString(),
                            this.DepartmentId,
                            calendar.ShiftId,
                            calendar.IsHoliday,
                            calendar.IsOffDay,
                            calendar.IsLeaveDay,
                            calendar.LeaveTypeId,
                            calendar.LeaveDay.HasValue ? calendar.LeaveDay.Value : 0,
                            calendar.ModifiedDate);
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public override string GetSettingKey()
        {
            return "API_SYN_Calendar";
        }

        public override bool IsValid()
        {
            return DepartmentId > 0 && EmployeeCalendars != null && EmployeeCalendars.Any();
        }
    }
}
