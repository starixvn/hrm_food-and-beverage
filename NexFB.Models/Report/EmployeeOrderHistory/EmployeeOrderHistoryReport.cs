// ----------------------------------------------------------------------
// <copyright file="EmployeeOrderHistoryReport.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/22/2014 1:23:41 PM">Created.</change>
// </history>

namespace NexFB.Models.Report.EmployeeOrderHistory
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Employee order history report.
    /// </summary>
    [Serializable]
    public class EmployeeOrderHistoryReport
    {
        public EmployeeOrderHistoryReport()
        {
            EmployeeOrders = new List<EmployeeOrderModel>();
        }

        public decimal TotalAmount { get; set; }

        public int TotalItem { get; set; }

        public List<EmployeeOrderModel> EmployeeOrders { get; set; }
    }
}
