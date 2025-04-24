using Fanex.Data.Repository;

namespace NexFB.WorkerService.Criteria;

public class GetUnitLastModifiedDateCriteria : CriteriaBase
{
    public int CompanyId { get; set; }

    public override string GetSettingKey()
    {
        return "Unit_GetLastModifiedDate";
    }

    public override bool IsValid()
    {
        return CompanyId > 0;
    }
}