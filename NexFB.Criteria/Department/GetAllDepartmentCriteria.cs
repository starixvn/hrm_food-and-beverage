namespace NexFB.Criteria.Department
{
    using Fanex.Data.Repository;

    public class GetAllDepartmentCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Department_GetAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
