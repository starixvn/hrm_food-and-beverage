namespace NexFB.WindowsService.Criteria
{
    using Fanex.Data.Repository;

    public class GetLastEmployeeCalendarModifiedDateCriteria : CriteriaBase
    {
        public int CompanyId { get; set; }

        public override string GetSettingKey()
        {
            return "EmpCalendar_GetLastModifiedDate";
        }

        public override bool IsValid()
        {
            return CompanyId > 0;
        }
    }
}
