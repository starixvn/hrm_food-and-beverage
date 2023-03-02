namespace NexFB.Models.Entities
{
    using System;

    [Serializable]
    public class Employee
    {
        public int UserId { get; set; }

        public int EmployeeId { get; set; }

        public string Code { get; set; }

        public string EmployeeCard { get; set; }

        public string DomainUsername { get; set; }

        public string FullName { get; set; }

        public string NickName { get; set; }
        public string Team { get; set; }

        public short EmployeeType { get; set; }

        public string Email { get; set; }

        public string Skype { get; set; }

        public bool? Enabled { get; set; }

        public string Password { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime JoinedDate { get; set; }

        public string Roles { get; set; }

        public bool? IsFreeOrder { get; set; }
        public bool? HasCheckIn { get; set; }
        public short CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Unit { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }


    }
}