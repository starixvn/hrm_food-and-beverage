// --------------------------------------------------------------------------
// <copyright file="Employee.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2014.8.26">Created</change>
// </history>

namespace NexFB.Models.Entities
{
    using System;

    /// <summary>
    /// Employee Model.
    /// </summary>
   [Serializable]
    public class Employee
    {
        public int UserId { get; set; }

        public int EmployeeId { get; set; }

        public string Code { get; set; }

        public string DomainUsername { get; set; }

        public string FullName { get; set; }

        public string NickName { get; set; }

        public string Email { get; set; }

        public string Skype { get; set; }

        public bool? Enabled { get; set; }

        public string Password { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime JoinedDate { get; set; }

        public string Roles { get; set; }
    }
}
