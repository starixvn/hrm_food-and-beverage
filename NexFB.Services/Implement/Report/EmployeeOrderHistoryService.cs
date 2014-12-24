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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Report;
    using NexFB.Models.Entities;
    using NexFB.Models.Report.EmployeeOrderHistory;
    using NexFB.Repositories.Implement.Report;
    using NexFB.Repositories.Interface.Report;
    using NexFB.Resources;
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
        /// Export report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="templatePath">The template path.</param>
        public void ExportEmployeeOrderHistoryReport(EmployeeOrderHistoryReport report, string templatePath)
        {
            string reportName = Lang.EmployeeOrderHistoryReportName;
            string reportTitle = Lang.EmployeeOrderHistoryReportTitle;
            var writer = new ExcelTemplateWriter(templatePath, reportName, Lang.MonthlyMainDishLocalSheetName);

            // Fill report title.
            writer.CopyRows(0, 2);
            writer.WriteRow(2, new object[] { reportTitle });

            // Fill report header.
            writer.CopyRows(3, 1);

            FillContentRow(report.EmployeeOrders, ref writer);
            FillTotalRow(report.TotalAmount, ref writer);
            writer.SyncColumnsWidth(9);
            writer.AutosizeColumns(9);

            writer.Render();
        }

        /// <summary>
        /// Fill content report.
        /// </summary>
        /// <param name="employeeOrders">The list employee order.</param>
        /// <param name="writer">The writer.</param>
        private void FillContentRow(List<EmployeeOrderModel> employeeOrders, ref ExcelTemplateWriter writer)
        {
            int numberOrders = employeeOrders.Count;

            for (int i = 0; i < numberOrders; i++)
            {
                EmployeeOrderModel employeeOrder = employeeOrders[i];
                string menuTemplate = CreateMyMenu(employeeOrder.Menus);
                string amountTemplate = CreateMyAmount(employeeOrder.Menus);
                int rowIndex = GetRowStyleByStatus(employeeOrder.Status.StatusId);

                var record = new
                {
                    Count = i + 1,
                    Date = employeeOrder.OnDate,
                    Meal = employeeOrder.Meal.MealEngName,
                    Order = employeeOrder.Order.OrderNo,
                    Status = employeeOrder.Status.Name,
                    OrderedBy = employeeOrder.OrderBy.NickName,
                    OrderPlatform = employeeOrder.OrderFrom,
                    MyMenu = menuTemplate,
                    MyAmount = amountTemplate
                };
                writer.WriteRow(rowIndex, record);
            }
        }

        /// <summary>
        /// Fill total row.
        /// </summary>
        /// <param name="totalAmount">The total amount.</param>
        /// <param name="writer">The writer.</param>
        private void FillTotalRow(decimal totalAmount, ref ExcelTemplateWriter writer)
        {
            object record = new
            {
                Count = Lang.Total,
                Date = string.Empty,
                Meal = string.Empty,
                Order = string.Empty,
                Status = string.Empty,
                OrderedBy = string.Empty,
                OrderPlatform = string.Empty,
                MyMenu = string.Empty,
                MyAmount = totalAmount
            };
            writer.WriteRow(11, record);
        }

        /// <summary>
        /// Get row index by status Id.
        /// </summary>
        /// <param name="statusId">The status Id.</param>
        /// <returns>The row index.</returns>
        private int GetRowStyleByStatus(int statusId)
        {
            int rowIndex = 10;

            switch (statusId)
            {
                case (int)OrderStatus.NewOrder:
                    rowIndex = 4;
                    break;
                case (int)OrderStatus.ReceivedOrder:
                    rowIndex = 5;
                    break;
                case (int)OrderStatus.DeliveredOrder:
                    rowIndex = 6;
                    break;
                case (int)OrderStatus.ReducedOrder:
                    rowIndex = 7;
                    break;
                case (int)OrderStatus.EditedOrder:
                    rowIndex = 8;
                    break;
                case (int)OrderStatus.CanceledOrder:
                    rowIndex = 9;
                    break;
            }

            return rowIndex;
        }

        /// <summary>
        /// Create my menu.
        /// </summary>
        /// <param name="menus">The list menu.</param>
        /// <returns>The menu template.</returns>
        private string CreateMyMenu(List<Menu> menus)
        {
            string menuTemplate = string.Empty;

            foreach (Menu menu in menus)
            {
                string menuItem = menu.QuantityOrder + " x \"" + menu.NameEN + "\"";
                menuTemplate += !string.IsNullOrEmpty(menuTemplate) ? Environment.NewLine + menuItem : menuItem;
            }

            return menuTemplate;
        }

        /// <summary>
        /// Create my amount.
        /// </summary>
        /// <param name="menus">The list menu.</param>
        /// <returns>The total amount.</returns>
        private string CreateMyAmount(List<Menu> menus)
        {
            decimal totalAmount = 0;

            if (menus != null && menus.Count > 0)
            {
                foreach (Menu menu in menus)
                {
                    if (menu.IsMainMenu && menus.Count == 1)
                    {
                        return Constants.NonAvailable;
                    }

                    totalAmount += !menu.IsMainMenu ? menu.TotalAmount : 0;
                }
            }
            else
            {
                return string.Empty;
            }

            return totalAmount.ToString(Constants.NumberFormat);
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
