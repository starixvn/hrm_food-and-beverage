// ----------------------------------------------------------------------
// <copyright file="IEmployeeOrderHistoryRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/22/2014 2:10:29 PM">Created.</change>
// </history>

namespace NexFB.Repositories.Interface.Report
{
    using Fanex.Data.Repository;
    using NexFB.Models.Report.EmployeeOrderHistory;

    /// <summary>
    /// The interface of Employee Order History repository.
    /// </summary>
    public interface IEmployeeOrderHistoryRepository
    {
        EmployeeOrderHistoryReport GetEmployeeOrderHistoryReport(ICriteria criteria);
    }
}
