// ----------------------------------------------------------------------
// <copyright file="IEmployeeOrderHistoryService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/22/2014 2:59:48 PM">Created.</change>
// </history>

namespace NexFB.Services.Interface.Report
{
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.EmployeeOrderHistory;

    /// <summary>
    /// The interface of Employee order history service.
    /// </summary>
    public interface IEmployeeOrderHistoryService
    {
        EmployeeOrderHistoryReport GetEmployeeOrderHistoryReport(GetEmployeeOrderHistoryCriteria criteria);

        EmployeeOrderHistoryReport GetPagingReport(EmployeeOrderHistoryReport fullReport, int startIndex, int itemsPerPage);
    }
}
