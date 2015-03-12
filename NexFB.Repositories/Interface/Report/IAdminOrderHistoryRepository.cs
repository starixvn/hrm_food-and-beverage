// ------------------------------------------------------------------------
// <copyright file="IAdminOrderHistoryRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Aaron.Cao" date="05/09/2015 12:35:00 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Interface.Report
{
    using Fanex.Data.Repository;
    using NexFB.Models.Report.AdminOrderHistoryReport;

    /// <summary>
    /// The interface of Admin Order History repository.
    /// </summary>
    public interface IAdminOrderHistoryRepository
    {
        AdminOrderHistoryReport GetAdminOrderHistoryReport(ICriteria criteria);
    }
}