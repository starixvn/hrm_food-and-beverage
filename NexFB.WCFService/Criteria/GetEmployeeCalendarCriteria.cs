namespace NexFB.WCFService.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetEmployeeCalendarCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }

        public override string GetSettingKey() => "EmployeeCalendar_Get";

        public override bool IsValid() => true;
    }
}