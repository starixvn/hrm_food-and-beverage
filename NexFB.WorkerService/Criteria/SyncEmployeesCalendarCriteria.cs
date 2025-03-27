using System.Text;
using Fanex.Data.Repository;
using Fanex.Data;
using NexFB.WorkerService.Models;

namespace NexFB.WorkerService.Criteria;

internal class SyncEmployeesCalendarCriteria : CriteriaBase
{
    private const string _EmployeeCalendarTpl =
        "<Calendar EmpId='{0}' WorkDate='{1}' CompanyId='{2}' "
        + "ShiftId='{3}' IsHoliday='{4}' IsOffDay='{5}' IsLeaveDay='{6}' LeaveTypeId='{7}' "
        + " LeaveDay='{8}'  ModifiedDate='{9}' HasCheckIn='{10}'></Calendar>";

    [SpParam(Ignored = true)]
    public int CompanyId { get; set; }

    [SpParam(Ignored = true)]
    public IList<EmployeeCalendar> EmployeeCalendars { get; set; }

    public string CalendarXml
    {
        get
        {
            var value = new StringBuilder("<ROOT>");

            foreach (var calendar in EmployeeCalendars)
            {
                value.AppendFormat(
                    _EmployeeCalendarTpl,
                    calendar.UserId,
                    calendar.WorkDate,
                    CompanyId,
                    calendar.ShiftId,
                    calendar.IsHoliday,
                    calendar.IsOffDay,
                    calendar.IsLeaveDay,
                    calendar.LeaveTypeId,
                    calendar.LeaveDay,
                    calendar is { LeaveDay: not null, ModifiedDate: not null } ? 
                        calendar.ModifiedDate.Value.ToString("yyyy-MM-dd HH:mm:ss:fff") 
                        : string.Empty,
                    calendar.HasCheckIn);
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
        return CompanyId > 0 && EmployeeCalendars.Any();
    }
}