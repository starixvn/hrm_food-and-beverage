// -----------------------------------------------------------------------
// <copyright file="SideDishReportModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Larry.Tran" date="1/7/2015 3:58 PM">Created.</change>
// </history>

namespace NexFB.Models.Report.SideDishReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The SideDishReportModel class.
    /// </summary>
    public class SideDishReportModel
    {
        public List<DepartmentTeamModel> DepartmentTeamModels { get; set; }

        public decimal TotalQuantity { get; set; }

        public double TotalAmount { get; set; }

        public int TotalItem { get; set; }
    }
}
