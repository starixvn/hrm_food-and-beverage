// --------------------------------------------------------------------------
// <copyright file="SideDishItemModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Larry.Tran" date="2015.1.15">Created</change>
// </history>

namespace NexFB.Models.Report.SideDishReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// SideDishItemModel class.
    /// </summary>
    public class SideDishItemModel
    {
        public string DepartmentTeamName { get; set; }

        public DateTime WorkDate { get; set; }

        public string Shift { get; set; }

        public string FullName { get; set; }

        public string NickName { get; set; }
        public string Team { get; set; }

        public string FoodVNMName { get; set; }

        public string FoodEngName { get; set; }

        public decimal Price { get; set; }

        public decimal Qty { get; set; }

        public decimal Amount { get; set; }
    }
}