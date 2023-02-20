namespace NexFB.WindowsService.Criteria
{
    using Fanex.Data.Repository;

    public class GetLastDepartmentModifiedDateCriteria : CriteriaBase
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
}
