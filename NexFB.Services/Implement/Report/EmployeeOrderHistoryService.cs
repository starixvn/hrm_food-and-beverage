// ----------------------------------------------------------------------
// <copyright file="EmployeeOrderHistoryService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/22/2014 3:01:31 PM">Created.</change>
// </history>

namespace NexFB.Services.Implement.Report
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Criteria.Report;
    using NexFB.Models.Entities;
    using NexFB.Models.Report.EmployeeOrderHistory;
    using NexFB.Repositories.Implement.Report;
    using NexFB.Repositories.Interface.Report;
    using NexFB.Services.Interface.Report;

    /// <summary>
    /// The implement of .
    /// </summary>
    public class EmployeeOrderHistoryService : IEmployeeOrderHistoryService
    {
        private readonly IEmployeeOrderHistoryRepository _employeeOrderHistoryRepository;

        public EmployeeOrderHistoryService()
            : this(null)
        {
        }

        public EmployeeOrderHistoryService(IEmployeeOrderHistoryRepository employeeOrderHistoryRepository)
        {
            _employeeOrderHistoryRepository = employeeOrderHistoryRepository ?? new EmployeeOrderHistoryRepository();
        }

        /// <summary>
        /// Get employee order history report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The employee order history report.</returns>
        public EmployeeOrderHistoryReport GetEmployeeOrderHistoryReport(GetEmployeeOrderHistoryCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var report = new EmployeeOrderHistoryReport();

            if (criteria.IsValid())
            {
                report = _employeeOrderHistoryRepository.GetEmployeeOrderHistoryReport(criteria);
                report.TotalAmount = CalculateTotalAmount(report.EmployeeOrders);
            }

            return report;
        }

        /// <summary>
        /// Calculate total amount of all employees.
        /// </summary>
        /// <param name="employeeOrders">The list employee order.</param>
        /// <returns>The total amount.</returns>
        private decimal CalculateTotalAmount(List<EmployeeOrderModel> employeeOrders)
        {
            decimal totalAmount = 0;

            foreach (EmployeeOrderModel employeeOrder in employeeOrders) 
            {
                foreach (Menu menu in employeeOrder.Menus) 
                {
                    totalAmount += menu.TotalAmount;
                }
            }

            return totalAmount;
        }
    }
}
