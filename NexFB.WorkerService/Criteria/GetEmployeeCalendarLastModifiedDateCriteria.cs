using Fanex.Data.Repository;

namespace NexFB.WorkerService.Criteria;

public class GetEmployeeCalendarLastModifiedDateCriteria : CriteriaBase
{
    public int CompanyId { get; set; }

    public override string GetSettingKey()
    {
        return "EmpCalendar_GetLastModifiedDate";
    }

    public override bool IsValid()
    {
        return CompanyId > 0;
    }
}