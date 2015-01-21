// -----------------------------------------------------------------------
// <copyright file="IMealForecastDetailReportRepository.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/16">Created.</change>
// </history>

namespace NexFB.Repositories.Interface.Report
{
    using Fanex.Data.Repository;
    using NexFB.Models.Report.MealForecastReport;

    /// <summary>
    /// The interface of Meal forecast detail report repository.
    /// </summary>
    public interface IMealForecastDetailReportRepository
    {
        MealForecastDetailHistoryReport GetMealForecastDetailReport(ICriteria criteria);
    }
}