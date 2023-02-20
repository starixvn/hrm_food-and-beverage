namespace NexFB.Criteria.Team
{
    using Fanex.Data.Repository;

    public class GetAllTeamByCompanyCriteria : CriteriaBase
    {
        public string DepartmentId { get; set; }

        public override string GetSettingKey()
        {
            return "EmpTeam_GetByDepartmentID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
