namespace NexFB.Services.Implement.Report
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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

        public void ExportAdminOrderHistoryReport(AdminOrderHistoryReport report, string templatePath)
        {
            var count = report.EmployeeOrders.Count;
            var reportToDate = report.EmployeeOrders.ElementAt(0).OnDate.ToString(Constants.DateFormat);
            var reportFromDate = report.EmployeeOrders.ElementAt(count - 1).OnDate.ToString(Constants.DateFormat);
            var reportName = Lang.AdminOrderHistoryReportName + "_" + reportFromDate.Replace(" ", "_") + "_" + reportToDate.Replace(" ", "_");
            var reportTitle = Lang.AdminOrderHistoryReportTitle + " - " + reportFromDate + " to " + reportToDate;
            var writer = new ExcelTemplateWriter(templatePath, reportName, Lang.AdminOrderHistoryReport);

            // Fill report title.
            const byte TitleRowIndex = 2;
            writer.CopyRows(0, TitleRowIndex);
            writer.WriteRow(TitleRowIndex, new object[] { reportTitle });

            // Fill report header.
            const byte HeaderRowIndex = 3;
            writer.CopyRows(HeaderRowIndex, 1);

            FillContentRow(report.EmployeeOrders, ref writer);
            FillTotalRow(report.EmployeeOrders, ref writer);

            const byte TotalColumns = 12;
            writer.SyncColumnsWidth(TotalColumns);
            writer.Render();
        }

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
                    Time = employeeOrder.OrderTime.ToString("MM/dd/yyyy hh:mm tt"),
                    Meal = employeeOrder.Meal.MealEngName,
                    Order = employeeOrder.Order.OrderNo,
                    Ref = employeeOrder.Order.OrderId,
                    OrderPlatform = employeeOrder.OrderFrom,
                    Status = employeeOrder.Status.Name,
                    OrderedBy = employeeOrder.OrderBy.NickName,
                    Company = employeeOrder.OrderBy.DepartmentName,
                    MainDish = CreateMainDish(employeeOrder.Menus),
                    SideDish = CreateSideDish(employeeOrder.Menus),
                    Amount = amountTemplate
                };

                writer.WriteRow(rowIndex, record);
            }
        }

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
                Company = string.Empty,
                MainDish = CalculateTotalMainDish(employeeOrders),
                SideDish = CalculateTotalSideDish(employeeOrders),
                Amount = totalAmount > 0 ? totalAmount.ToString(Constants.NumberFormat) : "0"
            };
            const byte TotalRowIndex = 11;
            writer.WriteRow(TotalRowIndex, record);
        }

        [SuppressMessage("Major Code Smell", "S109:Magic numbers should not be used", Justification = "Ok")]
        private static int GetRowStyleByStatus(int statusId)
        {
            int rowIndex;
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
                default:
                    rowIndex = 10;
                    break;
            }

            return rowIndex;
        }

        private static string CreateAmount(List<Menu> menus)
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

        private static int CreateMainDish(List<Menu> menus)
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

        private static int CreateSideDish(List<Menu> menus)
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

        private static bool CheckHasSideDish(List<Menu> menus)
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

        private static List<AdminOrderModel> GetPagingReportDetail(
            AdminOrderHistoryReport fullReport,
            int startIndex,
            int itemsPerPage)
        {
            int numberEmployeeOrders = fullReport.EmployeeOrders.Count;
            int numberTakenItems = (startIndex + itemsPerPage) <= numberEmployeeOrders ? itemsPerPage : (numberEmployeeOrders - startIndex);

            return fullReport.EmployeeOrders.Skip(startIndex).Take(numberTakenItems).ToList();
        }

        private static decimal CalculateTotalAmount(List<AdminOrderModel> employeeOrders)
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

        private static int CalculateTotalMainDish(List<AdminOrderModel> employeeOrders)
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

        private static int CalculateTotalSideDish(List<AdminOrderModel> employeeOrders)
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