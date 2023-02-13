using Fanex.Data.Repository;
using System;

namespace NexFB.Criteria.Employee
{
    public class GetEmployeeShiftInMonthCriteria : CriteriaBase
    {
        public DateTime ReportDate { get; set; }
        public override string GetSettingKey()
        {
            return "EmpShift_GetShiftInMonth";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
