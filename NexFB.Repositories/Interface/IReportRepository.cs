// ----------------------------------------------------------------------
// <copyright file="IReportRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="11/4/2014 9:14:25 AM">Created.</change>
// </history>

namespace NexFB.Repositories.Interface
{
    using Fanex.Data.Repository;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;

    /// <summary>
    /// The interface of Report repository.
    /// </summary>
    public interface IReportRepository
    {
        MonthlyLocalReportModel GetMonthlyMainDishReportLocal(ICriteria criteria);
    }
}
