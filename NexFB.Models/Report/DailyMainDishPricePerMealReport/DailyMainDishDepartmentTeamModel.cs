// ----------------------------------------------------------------------
// <copyright file="DailyMainDishDepartmentTeamModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/12/2014 10:48:17 AM">Created.</change>
// </history>

namespace NexFB.Models.Report.DailyMainDishPricePerMealReport
{
    using System;
    using System.Collections.Generic;
    using NexFB.Models.Entities;

    /// <summary>
    /// Daily main dish of each team in each department model.
    /// </summary>
    [Serializable]
    public class DailyMainDishDepartmentTeamModel
    {
        public DailyMainDishDepartmentTeamModel()
        {
            EmployeeDishes = new List<DailyEmployeeMainDishModel>();
        }

        public Department Department { get; set; }

        public Team Team { get; set; }

        public List<DailyEmployeeMainDishModel> EmployeeDishes { get; set; }
    }
}
