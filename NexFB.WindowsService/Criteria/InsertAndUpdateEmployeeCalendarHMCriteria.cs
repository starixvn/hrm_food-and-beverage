namespace NexFB.WindowsService.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;

    public class InsertAndUpdateEmployeeCalendarHMCriteria : CriteriaBase
    {
        private const string _EmployeeCalendarTpl =
            "<Calendar EmpId='{0}' WorkDate='{1}' CompanyId='{2}' "
            + "ShiftId='{3}' IsHoliday='{4}' IsOffDay='{5}' IsLeaveDay='{6}' LeaveTypeId='{7}' "
            + " LeaveDay='{8}'  ModifiedDate='{9}' HasCheckIn='{10}'></Calendar>";

        [SpParam(Ignored = true)]
        public int CompanyId { get; set; }

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
                            CompanyId,
                            calendar.ShiftId,
                            calendar.IsHoliday,
                            calendar.IsOffDay,
                            calendar.IsLeaveDay,
                            calendar.LeaveTypeId,
                            calendar.LeaveDay.HasValue ? calendar.LeaveDay.Value : 0,
                            calendar.LeaveDay.HasValue ? calendar.ModifiedDate.Value.ToString("yyyy-MM-dd HH:mm:ss:fff") : string.Empty,
                            calendar.HasCheckIn);
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
            return CompanyId > 0 && EmployeeCalendars != null && EmployeeCalendars.Any();
        }
    }
}
