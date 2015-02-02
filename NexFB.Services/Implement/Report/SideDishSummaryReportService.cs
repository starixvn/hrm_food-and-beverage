// -----------------------------------------------------------------------
// <copyright file="SideDishSummaryReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/30">Created</change>
// </history>

namespace NexFB.Services.Implement.Report
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using NexFB.Core.Services;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.SideDishSummaryReport;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Resources;
    using NexFB.Services.Interface.Report;

    /// <summary>
    /// The implement of side dish summary report service.
    /// </summary>
    public class SideDishSummaryReportService : BaseService, ISideDishSummaryReportService
    {
        private readonly IReportRepository _reportRepository;

        public SideDishSummaryReportService()
            : this(null)
        {
        }

        public SideDishSummaryReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository ?? new ReportRepository();
        }

        /// <summary>
        /// Get side dish summary report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The side dish summary report.</returns>
        public SideDishSummaryReportModel GetSideDishSummaryReport(GetSideDishSummaryReportCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var report = new SideDishSummaryReportModel();

            if (criteria.IsValid())
            {
                report = _reportRepository.GetSideDishSummaryReport(criteria, criteria.PageIndex, criteria.ItemsPerPage);
            }

            return report;
        }

        /// <summary>
        /// Export report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="templatePath">The template path.</param>
        public void ExportSideDishSummaryReport(SideDishSummaryReportModel report, string templatePath)
        {
            string reportName = Lang.SideDishSummaryReportName,
                reportTitle = Lang.SideDishSummaryReport,
                reportSheetName = Lang.SideDishSummaryReport;
            var writer = new ExcelTemplateWriter(templatePath, reportName, reportSheetName);

            // Fill report title.
            writer.CopyRows(0, 2);
            writer.WriteRow(2, new object[] { reportTitle });

            // Fill report header.
            writer.CopyRows(3, 1);

            FillContentRow(report, ref writer);

            FillTotalRow(report, ref writer);

            writer.SyncColumnsWidth(6);
            writer.Render();
        }

        /// <summary>
        /// Fill content row.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="writer">The writer.</param>
        private void FillContentRow(SideDishSummaryReportModel report, ref ExcelTemplateWriter writer)
        {
            int rowIndex = 1;
            var groupName = new List<string>();

            foreach (var item in report.SideDishSummarys)
            {
                if (!groupName.Contains(item.DepartmentTeamName))
                {
                    writer.WriteRow(4, new object[] { item.DepartmentTeamName });
                    groupName.Add(item.DepartmentTeamName);
                    rowIndex = 1;
                }

                var subRow = new
                {
                    Count = rowIndex,
                    FullName = item.FullName,
                    NickName = item.NickName,
                    EmpType = item.EmpType,
                    Qty = item.Qty,
                    Amount = item.Amount
                };

                writer.WriteRow(5, subRow);
                rowIndex++;
            }
        }

        /// <summary>
        /// Fill total row.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="writer">The writer.</param>
        private void FillTotalRow(SideDishSummaryReportModel report, ref ExcelTemplateWriter writer)
        {
            var record = new
            {
                Count = Lang.Total,
                FullName = string.Empty,
                NickName = string.Empty,
                Title = string.Empty,
                Qty = report.TotalQuantity,
                Amount = report.TotalAmount
            };

            writer.WriteRow(6, record);
        }
    }
}