// ----------------------------------------------------------------------
// <copyright file="AdminOrderHistoryService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Aaron.Cao" date="03/09/2015 1:15:00 PM">Created.</change>
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
    using NexFB.Models.Report.AdminOrderHistoryReport;
    using NexFB.Repositories.Implement.Report;
    using NexFB.Repositories.Interface.Report;
    using NexFB.Resources;
    using NexFB.Services.Interface.Report;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AdminOrderHistoryService : IAdminOrderHistoryService
    {
        private readonly IAdminOrderHistoryRepository _adminOrderHistoryRepository;

        public AdminOrderHistoryService()
            : this(null)
        {
        }

        public AdminOrderHistoryService(IAdminOrderHistoryRepository adminOrderHistoryRepository)
        {
            _adminOrderHistoryRepository = adminOrderHistoryRepository ?? new AdminOrderHistoryRepository();
        }

        /// <summary>
        /// Get admin order history report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The employee order history report.</returns>
        public AdminOrderHistoryReport GetAdminOrderHistoryReport(GetAdminOrderHistoryCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var report = new AdminOrderHistoryReport();

            if (criteria.IsValid())
            {
                report = _adminOrderHistoryRepository.GetAdminOrderHistoryReport(criteria);
                report.TotalAmount = CalculateTotalAmount(report.EmployeeOrders);
                report.TotalItem = report.EmployeeOrders.Count;
                report.TotalMainDish = CalculateTotalMainDish(report.EmployeeOrders);
                report.TotalSideDish = CalculateTotalSideDish(report.EmployeeOrders);
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
        public AdminOrderHistoryReport GetPagingReport(
            AdminOrderHistoryReport fullReport,
            int startIndex,
            int itemsPerPage)
        {
            var pagingReport = new AdminOrderHistoryReport
            {
                TotalAmount = fullReport.TotalAmount,
                TotalItem = fullReport.TotalItem,
                TotalMainDish = fullReport.TotalMainDish,
                TotalSideDish = fullReport.TotalSideDish
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
        public void ExportAdminOrderHistoryReport(AdminOrderHistoryReport report, string templatePath)
        {
            var reportDate = report.EmployeeOrders.ElementAt(0).OnDate.ToString(Constants.DateFormat);
            var reportName = Lang.AdminOrderHistoryReportName + "_" + reportDate.Replace(" ", "_");
            var reportTitle = Lang.AdminOrderHistoryReportTitle + " - " + reportDate;
            var writer = new ExcelTemplateWriter(templatePath, reportName, Lang.AdminOrderHistoryReport);

            // Fill report title.
            writer.CopyRows(0, 2);
            writer.WriteRow(2, new object[] { reportTitle });

            // Fill report header.
            writer.CopyRows(3, 1);

            FillContentRow(report.EmployeeOrders, ref writer);
            FillTotalRow(report.EmployeeOrders, ref writer);
            writer.SyncColumnsWidth(10);

            writer.Render();
        }

        /// <summary>
        /// Fill content report.
        /// </summary>
        /// <param name="employeeOrders">The list employee order.</param>
        /// <param name="writer">The writer.</param>
        private void FillContentRow(List<AdminOrderModel> employeeOrders, ref ExcelTemplateWriter writer)
        {
            var numberOrders = employeeOrders.Count;

            for (var i = 0; i < numberOrders; i++)
            {
                var employeeOrder = employeeOrders[i];
                var amountTemplate = employeeOrder.Status.StatusId != (int)OrderStatus.CanceledOrder ?
                                        CreateAmount(employeeOrder.Menus) :
                                        Constants.NonAvailable;
                var rowIndex = GetRowStyleByStatus(employeeOrder.Status.StatusId);

                var record = new
                {
                    Count = i + 1,
                    Time = employeeOrder.OrderTime.ToString("HH:mm"),
                    Meal = employeeOrder.Meal.MealEngName,
                    Order = employeeOrder.Order.OrderNo,
                    Ref = employeeOrder.Order.OrderId,
                    OrderPlatform = employeeOrder.OrderFrom,
                    Status = employeeOrder.Status.Name,
                    OrderedBy = employeeOrder.OrderBy.NickName,
                    MainDish = CreateMainDish(employeeOrder.Menus),
                    SideDish = CreateSideDish(employeeOrder.Menus),
                    Amount = amountTemplate
                };

                writer.WriteRow(rowIndex, record);
            }
        }

        /// <summary>
        /// Fill total row.
        /// </summary>
        /// <param name="employeeOrders">The employee orders.</param>
        /// <param name="writer">The writer.</param>
        private void FillTotalRow(List<AdminOrderModel> employeeOrders, ref ExcelTemplateWriter writer)
        {
            var totalAmount = CalculateTotalAmount(employeeOrders);

            object record = new
            {
                Count = Lang.Total,
                Time = string.Empty,
                Meal = string.Empty,
                Order = string.Empty,
                Ref = string.Empty,
                OrderPlatform = string.Empty,
                Status = string.Empty,
                OrderedBy = string.Empty,
                MainDish = CalculateTotalMainDish(employeeOrders),
                SideDish = CalculateTotalSideDish(employeeOrders),
                Amount = totalAmount > 0 ? totalAmount.ToString(Constants.NumberFormat) : "0"
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
        /// Create amount.
        /// </summary>
        /// <param name="menus">The list menu.</param>
        /// <returns>The total amount.</returns>
        private string CreateAmount(List<Menu> menus)
        {
            decimal totalAmount = 0;

            if (menus != null && menus.Count > 0)
            {
                bool hasSideDish = CheckHasSideDish(menus);

                if (hasSideDish)
                {
                    foreach (Menu menu in menus)
                    {
                        totalAmount += !menu.IsMainMenu ? menu.TotalAmount : 0;
                    }
                }
                else
                {
                    return Constants.NonAvailable;
                }
            }
            else
            {
                return string.Empty;
            }

            return totalAmount > 0 ? totalAmount.ToString(Constants.NumberFormat) : "0";
        }

        /// <summary>
        /// Create main dish.
        /// </summary>
        /// <param name="menus">The list menu.</param>
        /// <returns>The number of main dish.</returns>
        private int CreateMainDish(List<Menu> menus)
        {
            int mainDish = 0;

            foreach (var menu in menus)
            {
                if (menu.IsMainMenu)
                {
                    mainDish += menu.QuantityOrder;
                }
            }

            return mainDish;
        }

        /// <summary>
        /// Create side dish.
        /// </summary>
        /// <param name="menus">The list menu.</param>
        /// <returns>The number of side dish.</returns>
        private int CreateSideDish(List<Menu> menus)
        {
            int sideDish = 0;

            foreach (var menu in menus)
            {
                if (menu.IsSideDish)
                {
                    sideDish += menu.QuantityOrder;
                }
            }

            return sideDish;
        }

        /// <summary>
        /// Check if menus contain side dish menu.
        /// </summary>
        /// <param name="menus">The menus.</param>
        /// <returns>The has side dish menu or not.</returns>
        private bool CheckHasSideDish(List<Menu> menus)
        {
            foreach (Menu menu in menus)
            {
                if (menu.IsSideDish)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get employee order of paging report.
        /// </summary>
        /// <param name="fullReport">The full report.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The list employee order.</returns>
        private List<AdminOrderModel> GetPagingReportDetail(
            AdminOrderHistoryReport fullReport,
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
        private decimal CalculateTotalAmount(List<AdminOrderModel> employeeOrders)
        {
            decimal totalAmount = 0;

            foreach (AdminOrderModel employeeOrder in employeeOrders)
            {
                if (employeeOrder.Status.StatusId != (int)OrderStatus.CanceledOrder)
                {
                    foreach (Menu menu in employeeOrder.Menus)
                    {
                        totalAmount += menu.TotalAmount;
                    }
                }
            }

            return totalAmount;
        }

        /// <summary>
        /// Calculate total main dish.
        /// </summary>
        /// <param name="employeeOrders">The list employee order.</param>
        /// <returns>The total main dish.</returns>
        private int CalculateTotalMainDish(List<AdminOrderModel> employeeOrders)
        {
            int totalMainDish = 0;

            foreach (var employeeOrder in employeeOrders)
            {
                foreach (Menu menu in employeeOrder.Menus)
                {
                    if (menu.IsMainMenu)
                    {
                        totalMainDish += menu.QuantityOrder;
                    }
                }
            }

            return totalMainDish;
        }

        /// <summary>
        /// Calculate total side dish.
        /// </summary>
        /// <param name="employeeOrders">The list employee order.</param>
        /// <returns>The total side dish.</returns>
        private int CalculateTotalSideDish(List<AdminOrderModel> employeeOrders)
        {
            int totalSideDish = 0;

            foreach (var employeeOrder in employeeOrders)
            {
                foreach (Menu menu in employeeOrder.Menus)
                {
                    if (menu.IsSideDish)
                    {
                        totalSideDish += menu.QuantityOrder;
                    }
                }
            }

            return totalSideDish;
        }
    }
}