// --------------------------------------------------------------------------
// <copyright file="SideDishReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Larry.Tran" date="2015.1.14">Created</change>
// </history>

namespace NexFB.Services.Implement.Report
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Services;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.SideDishReport;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Resources;
    using NexFB.Services.Interface.Report;

    /// <summary>
    /// SideDishReportService class.
    /// </summary>
    public class SideDishReportService : BaseService, ISideDishReportService
    {
        private readonly IReportRepository _reportRepository;

        public SideDishReportService()
            : this(null)
        {
        }

        public SideDishReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository ?? new ReportRepository();
        }

        /// <summary>
        /// Gets the side dish report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The SideDishReportModel object.</returns>
        public SideDishReportModel GetSideDishReport(GetSideDishReportCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            SideDishReportModel report = null;
            if (criteria.IsValid())
            {
                report = _reportRepository.GetSideDishReport(criteria, criteria.PageIndex, criteria.ItemsPerPage);
            }

            return report;
        }

        /// <summary>
        /// Exports the side dish report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="templatePath">The template URL.</param>
        public void ExportSideDishReport(SideDishReportModel report, string templatePath)
        {
            string reportTitle = Lang.SideDishReport;
            var writer = new ExcelTemplateWriter(templatePath, reportTitle, reportTitle);

            // Create report title.
            writer.CopyRows(0, 1);

            // Create report header.
            writer.CopyRows(1, 1);

            FillContentRow(report, writer);

            FillTotalRow(report, writer);

            writer.SyncColumnsWidth(11);
            writer.Render();
        }

        /// <summary>
        /// Fills the total row.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="writer">The writer.</param>
        private void FillTotalRow(SideDishReportModel report, ExcelTemplateWriter writer)
        {
            var totalRecord = new
            {
                Cell1 = string.Empty,
                Cell2 = string.Empty,
                Cell3 = string.Empty,
                Cell4 = string.Empty,
                Cell5 = string.Empty,
                Cell6 = string.Empty,
                Cell7 = string.Empty,
                Cell8 = string.Empty,
                Total = Lang.Total,
                TotalQuantity = report.TotalQuantity,
                TotalAmount = report.TotalAmount,
            };

            writer.WriteRow(4, totalRecord);
        }

        /// <summary>
        /// Fills the content row.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="writer">The writer.</param>
        private void FillContentRow(SideDishReportModel report, ExcelTemplateWriter writer)
        {
            int counter = 1;
            var groupName = new List<string>();

            foreach (SideDishItemModel item in report.SideDishItems)
            {
                var subItem = new
                {
                    No = counter,
                    WorkDate = item.WorkDate,
                    Shift = item.Shift,
                    FullName = item.FullName,
                    NickName = item.NickName,
                    Section = item.Team,
                    FoodVNMName = item.FoodVNMName,
                    FoodEngName = item.FoodEngName,
                    Price = item.Price,
                    Qty = item.Qty,
                    Amount = item.Amount
                };

                if (!groupName.Contains(item.DepartmentTeamName))
                {
                    //writer.WriteRow(2, new object[] { item.CompanyTeamName });

                    groupName.Add(item.DepartmentTeamName);
                }

                writer.WriteRow(3, subItem);
                counter++;
            }
        }
    }
}