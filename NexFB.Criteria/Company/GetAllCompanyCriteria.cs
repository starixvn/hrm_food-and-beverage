namespace NexFB.Criteria.Company
{
    using Fanex.Data.Repository;

    public class GetAllCompanyCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Company_GetAll";

        public override bool IsValid() => true;
    }
}