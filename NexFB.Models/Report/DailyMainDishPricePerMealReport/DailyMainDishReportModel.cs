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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Daily main dish price per meal report model.
    /// </summary>
    [Serializable]
    public class DailyMainDishReportModel
    {
        public DailyMainDishReportModel()
        {
            MainDishes = new List<DailyMainDishDepartmentTeamModel>();
        }

        public List<DailyMainDishDepartmentTeamModel> MainDishes { get; set; }

        public TotalPriceModel SubTotal { get; set; }

        public TotalPriceModel GrandTotal { get; set; }

        public int TotalAmount { get; set; }

        public int TotalItem { get; set; }
    }
}
