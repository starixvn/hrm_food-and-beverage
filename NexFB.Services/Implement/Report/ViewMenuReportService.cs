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
        public void ExportViewMenuReport(ViewMenuReportModel report, string templatePath, DateTime menuDate)
        {
            string reportTitle = Lang.ViewMenuReport;
            string menuTitle = string.Format("Menu for {0:dd MMM, yyyy}", menuDate);
            var writer = new ExcelTemplateWriter(templatePath, reportTitle, reportTitle);

            // Create report title.            
            writer.WriteRow(0, new { menuTitle });

            // Create report header
            writer.CopyRows(1, 1);

            FillContentRow(report, writer);

            writer.SyncColumnsWidth(3);
            writer.Render();
        }

        /// <summary>
        /// Fills the content row.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="writer">The writer.</param>
        private void FillContentRow(ViewMenuReportModel report, ExcelTemplateWriter writer)
        {
            int counter = 1;
            var groupName = new List<string>();

            foreach (ViewMenuItemModel item in report.ViewMenuItems)
            {
                var subItem = new
                {
                    No = counter,
                    FoodEngName = item.FoodEngName,
                    FoodVNMName = item.FoodVNMName
                };

                if (!groupName.Contains(item.MenuName))
                {
                    if (item.IsVegetarian)
                    {
                        writer.WriteRow(3, new object[] { item.MenuName });
                    }
                    else
                    {
                        writer.WriteRow(2, new object[] { item.MenuName });
                    }

                    groupName.Add(item.MenuName);
                }

                writer.WriteRow(4, subItem);
                counter++;
            }
        }
    }
}
