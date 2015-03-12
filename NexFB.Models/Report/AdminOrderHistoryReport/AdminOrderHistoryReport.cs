// ----------------------------------------------------------------------
// <copyright file="AdminOrderHistoryReport.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Aaron.Cao" date="03/09/2015 3:15:00 PM">Created.</change>
// </history>

namespace NexFB.Models.Report.AdminOrderHistoryReport
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Admin order history report.
    /// </summary>
    [Serializable]
    public class AdminOrderHistoryReport
    {
        public AdminOrderHistoryReport()
        {
            EmployeeOrders = new List<AdminOrderModel>();
        }

        public decimal TotalAmount { get; set; }

        public int TotalItem { get; set; }

        public int TotalMainDish { get; set; }

        public int TotalSideDish { get; set; }

        public List<AdminOrderModel> EmployeeOrders { get; set; }
    }
}
