namespace NexFB.Models.MealRegistration
{
    using System;
    using System.Collections.Generic;
    using NexFB.Core.Enums;
    using NexFB.Core.Utilities;

    public class EmployeeRegister
    {
        public int SysId { get; set; }

        public string FullName { get; set; }

        public string Code { get; set; }

        public string NickName { get; set; }

        public string DomainUsername { get; set; }
    }
}
