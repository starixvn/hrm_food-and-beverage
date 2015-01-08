// -----------------------------------------------------------------------
// <copyright file="OrderModel.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Larry.Tran" date="1/7/2015 4:16 PM">Created.</change>
// </history>

namespace NexFB.Models.Report.SideDishReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The OrderModel class.
    /// </summary>
    public class OrderModel
    {
        public DateTime WorkDate { get; set; }

        public string Shift { get; set; }

        public string FullName { get; set; }

        public string NickName { get; set; }

        public string FoodVNMName { get; set; }

        public string FoodEngName { get; set; }

        public double Price { get; set; }

        public int Qty { get; set; }

        public double Amount { get; set; }
    }
}
