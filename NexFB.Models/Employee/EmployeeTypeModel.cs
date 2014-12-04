// ----------------------------------------------------------------------
// <copyright file="EmployeeTypeModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="9/26/2014 11:20:27 AM">Created.</change>
// </history>

namespace NexFB.Models.Employee
{
    using System;

    /// <summary>
    /// The employee type model.
    /// </summary>
    [Serializable]
    public class EmployeeTypeModel
    {
        public int EmployeeTypeId { get; set; }

        public string EmployeeTypeName { get; set; }
    }
}
