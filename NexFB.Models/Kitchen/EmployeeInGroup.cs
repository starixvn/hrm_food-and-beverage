namespace NexFB.Models.Kitchen
{
    using System.Collections.Generic;

    public class EmployeeInGroup
    {
        public List<GroupOrder> GroupOrder { get; set; }

        public List<EmployeeInfo> Employees { get; set; }
    }
}