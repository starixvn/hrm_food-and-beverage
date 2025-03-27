using Fanex.Data.Repository;

namespace NexFB.WorkerService.Criteria;

public class GetSectionLastModifiedDateCriteria : CriteriaBase
{
    public int CompanyId { get; set; }

    public override string GetSettingKey()
    {
        return "Section_GetLastModifiedDate";
    }

    public override bool IsValid()
    {
        return CompanyId > 0;
    }
}