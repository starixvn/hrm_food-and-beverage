namespace NexFB.WebService.Criteria
{
    using System;
    using Fanex.Data.Repository;

    public class GetEmployeeCriteria : CriteriaBase
    {
        public DateTime? FromDate { get; set; }

        public override string GetSettingKey() => "Employee_Get";

        public override bool IsValid() => true;
    }
}