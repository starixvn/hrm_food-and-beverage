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
