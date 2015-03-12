// ----------------------------------------------------------------------
// <copyright file="IAdminOrderHistoryService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Aaron.Cao" date="03/09/2015 1:10:00 PM">Created.</change>
// </history>

namespace NexFB.Services.Interface.Report
{
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.AdminOrderHistoryReport;

    /// <summary>
    /// The interface of Admin order history service.
    /// </summary>
    public interface IAdminOrderHistoryService
    {
        AdminOrderHistoryReport GetAdminOrderHistoryReport(GetAdminOrderHistoryCriteria criteria);

        AdminOrderHistoryReport GetPagingReport(AdminOrderHistoryReport fullReport, int startIndex, int itemsPerPage);

        void ExportAdminOrderHistoryReport(AdminOrderHistoryReport report, string templatePath);
    }
}