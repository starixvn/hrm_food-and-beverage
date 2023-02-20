namespace NexFB.WindowsService.Criteria
{
    using Fanex.Data.Repository;

    public class GetLastUnitModifiedDateCriteria : CriteriaBase
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
}
