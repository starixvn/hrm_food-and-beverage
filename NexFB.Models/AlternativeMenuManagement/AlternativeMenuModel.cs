// ----------------------------------------------------------------------
// <copyright file="AlternativeMenuModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="10/10/2014 3:22:10 PM">Created.</change>
// </history>

namespace NexFB.Models.AlternativeMenuManagement
{
    using System.Collections.Generic;
    using NexFB.Models.Employee;

    /// <summary>
    /// The alternative menu model.
    /// </summary>
    public class AlternativeMenuModel
    {
        public int AlternativeMenuId { get; set; }

        public string AlternativeMenuCode { get; set; }

        public string AlternativeMenuVnmName { get; set; }

        public string AlternativeMenuEngName { get; set; }

        public string AlternativeMenuPhoto { get; set; }

        public bool? IsActive { get; set; }

        public IList<EmployeeTypeModel> EmployeeTypes { get; set; }
    }
}
