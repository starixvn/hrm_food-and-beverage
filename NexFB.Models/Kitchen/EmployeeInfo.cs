// --------------------------------------------------------------------------
// <copyright file="EmployeeInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Bradley.Mac" date="2014.10.1">Created</change>
// </history>

namespace NexFB.Models.Kitchen
{
    /// <summary>
    /// Group User.
    /// </summary>
    public class EmployeeInfo
    {
        public int GroupOrderId { get; set; }

        public int MealId { get; set; }

        public string MealEngName { get; set; }

        public string MealVnmName { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int SysId { get; set; }

        public int EmpId { get; set; }

        public string DomainUsername { get; set; }

        public string Code { get; set; }

        public string FullName { get; set; }

        public string NickName { get; set; }

        public int TeamId { get; set; }

        public string EmpTeamCode { get; set; }

        public string EmpTeamName { get; set; }

        public int MenuId { get; set; }

        public string MenuEngName { get; set; }

        public string MenuVnmName { get; set; }

        public bool IsVegetarian { get; set; }

        public int LocationId { get; set; }

        public string LocationEngName { get; set; }

        public string LocationVnmName { get; set; }

        public string FloorName { get; set; }

        public int IsLocal { get; set; }

        public int Quantity { get; set; }
    }
}
