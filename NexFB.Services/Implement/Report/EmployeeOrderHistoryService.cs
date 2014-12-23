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
    using System.Linq;
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
                report.TotalItem = report.EmployeeOrders.Count;
            }

            return report;
        }

        /// <summary>
        /// Get paging report.
        /// </summary>
        /// <param name="fullReport">The full report.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The paging report.</returns>
        public EmployeeOrderHistoryReport GetPagingReport(
            EmployeeOrderHistoryReport fullReport,
            int startIndex,
            int itemsPerPage)
        {
            var pagingReport = new EmployeeOrderHistoryReport
            {
                TotalAmount = fullReport.TotalAmount,
                TotalItem = fullReport.TotalItem
            };

            if (fullReport.EmployeeOrders.Count > 0)
            {
                pagingReport.EmployeeOrders = GetPagingReportDetail(fullReport, startIndex, itemsPerPage);
            }

            return pagingReport;
        }

        /// <summary>
        /// Get employee order of paging report.
        /// </summary>
        /// <param name="fullReport">The full report.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The list employee order.</returns>
        private List<EmployeeOrderModel> GetPagingReportDetail(
            EmployeeOrderHistoryReport fullReport,
            int startIndex,
            int itemsPerPage)
        {
            int numberEmployeeOrders = fullReport.EmployeeOrders.Count;
            int numberTakenItems = 0;

            if (startIndex + itemsPerPage <= numberEmployeeOrders)
            {
                numberTakenItems = itemsPerPage;
            }
            else
            {
                numberTakenItems = numberEmployeeOrders - startIndex;
            }

            return fullReport.EmployeeOrders.Skip(startIndex).Take(numberTakenItems).ToList();
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
