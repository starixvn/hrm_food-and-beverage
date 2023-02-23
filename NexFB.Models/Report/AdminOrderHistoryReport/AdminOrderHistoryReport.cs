namespace NexFB.Models.Report.AdminOrderHistoryReport
{
    using System;
    using System.Collections.Generic;

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