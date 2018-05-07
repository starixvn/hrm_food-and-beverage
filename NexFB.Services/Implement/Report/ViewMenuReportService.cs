// --------------------------------------------------------------------------
// <copyright file="ViewMenuReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Larry.Tran" date="2015.1.19">Created</change>
// </history>

namespace NexFB.Services.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Services;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.ViewMenuReport;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Resources;
    using NexFB.Services.Interface.Report;

    /// <summary>
    /// ViewMenuReportService class.
    /// </summary>
    public class ViewMenuReportService : BaseService, IViewMenuReportService
    {
        private IReportRepository _reportRepository;

        public ViewMenuReportService()
            : this(null)
        {
        }

        public ViewMenuReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository ?? new ReportRepository();
        }

        /// <summary>
        /// Gets the view menu report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ViewMenuReportModel data.</returns>
        public ViewMenuReportModel GetViewMenuReport(GetViewMenuReportCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var report = new ViewMenuReportModel();
            if (criteria.IsValid())
            {
                report = _reportRepository.GetViewMenuReport(criteria);
            }

            return report;
        }

        /// <summary>
        /// Exports the view menu report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="templatePath">The template path.</param>
        /// <param name="menuDate">The menu date.</param>
        public void ExportViewMenuReport(ViewMenuReportModel report, string templatePath, DateTime fromDate, DateTime toDate)
        {
            string reportTitle = Lang.ViewMenuReport;
            string menuTitle = string.Format("Menu from {0:dd/MM/yyyy} to {1: dd/MM/yyyy}", fromDate, toDate);
            var writer = new ExcelTemplateWriter(templatePath, reportTitle, reportTitle);

            // Create report title.
            writer.WriteRow(0, new { menuTitle });

            writer.CopyRows(1, 1);
            //Write header row
            FillHeaderRow(report.Dates, writer);

            // Create report header
            FillContentRow(report.Menus, writer);

            writer.SyncColumnsWidth(report.Dates.Count() + 1);
            writer.Render();
        }

        private void FillHeaderRow(IEnumerable<DateTime> dates, ExcelTemplateWriter writer)
        {
            var colIndex = 1;
            foreach (var date in dates)
            {
                writer.WriteCellRange(1, 1, colIndex, 1, date.ToString("dd/MM/yyyy"));
                colIndex++;
            }
        }

        /// <summary>
        /// Fills the content row.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="writer">The writer.</param>
        private void FillContentRow(IList<MenuTypeModel> menus, ExcelTemplateWriter writer)
        {
            var rowIndex = 3;
            foreach (var item in menus)
            {
                writer.WriteRow(2, new { MealName = item.MenuName });
                FillSubRows(item.MenuForEachDay, item.MaxMenuItemPerMeal, writer, ref rowIndex);
                rowIndex++;
            }
        }

        private void FillSubRows(IList<MenuItemPerDateModel> menuForEachDay, int maxMenuItemPerMeal, ExcelTemplateWriter writer, ref int rowIndex)
        {
            for (int i = 0; i < maxMenuItemPerMeal; i++)
            {
                writer.CopyRows(3, 1);
                writer.WriteCellRange(rowIndex, 1, 0, 1, i + 1);
                var colIndex = 1;
                foreach (var menu in menuForEachDay)
                {
                    var menuFoodName = GetMenuFoodName(menu.Menus, i);
                    writer.WriteCellRange(rowIndex, 1, colIndex, 1, menuFoodName);
                    colIndex++;
                }
                rowIndex++;
            }
        }

        private string GetMenuFoodName(IList<ViewMenuItemModel> menus, int index)
        {
            string menuFoodName = string.Empty;
            try
            {
                menuFoodName = menus[index] != null ? string.Format("{0}{1}{2}", menus[index].FoodVNMName, "\n", menus[index].FoodEngName) : string.Empty;
            }
            catch
            {
                return menuFoodName;
            }

            return menuFoodName;
        }
    }
}