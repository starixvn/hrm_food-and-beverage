// ----------------------------------------------------------------------
// <copyright file="DailyMainDishReportModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/12/2014 9:44:29 AM">Created.</change>
// </history>

namespace NexFB.Models.Report.DailyMainDishPricePerMealReport
{
    using System.Collections.Generic;
    using NexFB.Models.Entities;

    /// <summary>
    /// Daily main dish price per meal report model.
    /// </summary>
    public class DailyMainDishReportModel
    {
        public Department Department { get; set; }

        public Team Team { get; set; }

        public List<DailyEmployeeMainDishModel> EmployeeDishes { get; set; }

        public TotalPriceModel SubTotal { get; set; }

        public TotalPriceModel GrandTotal { get; set; }

        public int TotalAmount { get; set; }
    }
}
