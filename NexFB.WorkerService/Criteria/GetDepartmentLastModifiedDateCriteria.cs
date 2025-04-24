using Fanex.Data.Repository;

namespace NexFB.WorkerService.Criteria;

public class GetDepartmentLastModifiedDateCriteria : CriteriaBase
{
    public int CompanyId { get; set; }

    public override string GetSettingKey()
    {
        return "Department_GetLastModifiedDate";
    }

    public override bool IsValid()
    {
        return CompanyId > 0;
    }
}