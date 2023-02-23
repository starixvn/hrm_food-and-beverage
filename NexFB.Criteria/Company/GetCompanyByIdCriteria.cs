namespace NexFB.Criteria.Company
{
    using Fanex.Data.Repository;

    public class GetCompanyByIdCriteria : CriteriaBase
    {
        public short CompanyId { get; set; }

        public override string GetSettingKey() => "Company_GetById";

        public override bool IsValid() => CompanyId > 0;
    }
}