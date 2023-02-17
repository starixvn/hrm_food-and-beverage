namespace NexFB.WindowsService.Criteria
{
    using Fanex.Data.Repository;

    public class GetLastUserModifiedDateCriteria : CriteriaBase
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
