// --------------------------------------------------------------------------
// <copyright file="SCEmployeeOrder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.20">Created</change>
// </history>

namespace NexFB.Models.SmartCardOrder
{
    using System;

    /// <summary>
    /// SC Employee Order.
    /// </summary>
    public class SCEmployeeOrder
    {
        public int SysId { get; set; }

        public int EmpId { get; set; }

        public int DepartmentId { get; set; }

        public int TeamId { get; set; }

        public string EmpTeamCode { get; set; }

        public string EmpTeamName { get; set; }

        public string DomainUsername { get; set; }

        public string Code { get; set; }

        public string FullName { get; set; }

        public string NickName { get; set; }

        public bool Enable { get; set; }

        public string EmpCategoryId { get; set; }

        public string CategoryName { get; set; }

        public int LocationId { get; set; }

        public string LocationVnmName { get; set; }

        public string LocationEngName { get; set; }

        public string FloorName { get; set; }

        public bool IsLocal { get; set; }

        public DateTime JoinedDate { get; set; }
    }
}
