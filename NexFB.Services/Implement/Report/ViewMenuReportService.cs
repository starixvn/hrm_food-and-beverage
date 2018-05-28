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
            string menuTitle = string.Format("Menu from {0:dd MMM, yyyy} to {1: dd MMM, yyyy}", fromDate, toDate);
            var writer = new ExcelTemplateWriter(templatePath, reportTitle, reportTitle);

            writer.CopyRows(0, 1);
            // Create report title.
            writer.WriteCellRange(0, 1, 0, report.Dates.Count() + 1, menuTitle);

            writer.CopyRows(1, 1);
            //Write header row
            FillHeaderRow(report.Dates, ref writer);

            // Create report content
            FillContentRow(report.Menus, report.Dates.Count() + 1, ref writer);

            writer.AutosizeColumns(report.Dates.Count() + 1);
            writer.Render();
        }

        private void FillHeaderRow(IEnumerable<DateTime> dates, ref ExcelTemplateWriter writer)
        {
            var colIndex = 1;
            foreach (var date in dates)
            {
                if (colIndex > 2)
                {
                    writer.CloneCellStyleOnLeftColumn(1, 2, 1, colIndex);
                }
                writer.WriteCellRange(1, 1, colIndex, 1, date.ToString("dd MMM, yyyy"));
                colIndex++;
            }
        }

        /// <summary>
        /// Fills the content row.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="writer">The writer.</param>
        private void FillContentRow(IList<MenuTypeModel> menus, int totalColumn, ref ExcelTemplateWriter writer)
        {
            var rowIndex = 3;
            foreach (var item in menus)
            {
                writer.CopyRows(2, 1);
                writer.WriteCellRange(rowIndex - 1, 1, 0, totalColumn, item.MenuName);

                FillSubRows(item.MenuForEachDay, item.MaxMenuItemPerMeal, ref writer, ref rowIndex);
                rowIndex++;
            }
        }

        private void FillSubRows(IList<MenuItemPerDateModel> menuForEachDay, int maxMenuItemPerMeal, ref ExcelTemplateWriter writer, ref int rowIndex)
        {
            for (int i = 0; i < maxMenuItemPerMeal; i++)
            {
                writer.CopyRows(3, 1);
                writer.WriteCellRange(rowIndex, 1, 0, 1, i + 1);
                var colIndex = 1;
                foreach (var menu in menuForEachDay)
                {
                    var menuFoodName = GetMenuFoodName(menu.Menus, i);
                    writer.CloneCellStyleOnLeftColumn(rowIndex, 1, rowIndex, colIndex);
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
                menuFoodName = menus[index] != null ? string.Format("{0}{1}{2}", menus[index].FoodVNMName, Environment.NewLine, menus[index].FoodEngName) : string.Empty;
            }
            catch
            {
                return menuFoodName;
            }

            return menuFoodName;
        }
    }
}