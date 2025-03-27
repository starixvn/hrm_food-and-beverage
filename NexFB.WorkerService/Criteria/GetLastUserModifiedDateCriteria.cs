using Fanex.Data.Repository;

namespace NexFB.WorkerService.Criteria
{
    public class GetEmployeeLastModifiedDateCriteria : CriteriaBase
    {
        public int CompanyId { get; set; }

        public override string GetSettingKey()
        {
            return "Employee_GetLastModifiedDate";
        }

        public override bool IsValid()
        {
            return CompanyId > 0;
        }
    }
}
