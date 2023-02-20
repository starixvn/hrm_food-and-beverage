namespace NexFB.Models.GroupControl
{
    using System;
    using System.Collections.Generic;

    public class MemberModel
    {
        public int SysId { get; set; }

        public int EmpId { get; set; }

        public string Fullname { get; set; }

        public string Nickname { get; set; }

        public short? CompanyId { get; set; }
        public string CompanyName { get; set; }

        public string EmployeeTypeName { get; set; }

        public DateTime? JoinedDate { get; set; }

        public IEnumerable<dynamic> AssignedGroups { get; set; }
    }
}