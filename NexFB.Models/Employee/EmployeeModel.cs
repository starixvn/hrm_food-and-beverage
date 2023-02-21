namespace NexFB.Models.Employee
{
    using System.Collections.Generic;
    using NexFB.Models.Entities;

    public class EmployeeModel : Employee
    {
        public EmployeeModel()
        {
            RoleItems = new List<Role>();
        }

        public Department Department { get; set; }

        public Team Team { get; set; }

        public List<Role> RoleItems { get; set; }
    }
}
