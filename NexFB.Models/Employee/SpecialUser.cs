namespace NexFB.Models.Employee
{
    using NexFB.Models.Entities;

    public class SpecialUser : Employee
    {
        public string EmployeeTypeName { get; set; }

        public bool IsActive { get; set; }
    }
}