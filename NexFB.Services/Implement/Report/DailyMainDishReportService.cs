// ----------------------------------------------------------------------
// <copyright file="DailyMainDishReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="12/18/2014 5:26:19 PM">Created.</change>
// </history>

namespace NexFB.Services.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Services;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.DailyMainDishPricePerMealReport;
    using NexFB.Repositories.Implement;
    using NexFB.Repositories.Interface;
    using NexFB.Services.Interface.Report;

    /// <summary>
    /// The implement of Daily Main Dish Report service.
    /// </summary>
    public class DailyMainDishReportService : BaseService, IDailyMainDishReportService
    {
        private readonly IReportRepository _reportRepository;

        public DailyMainDishReportService()
            : this(null)
        {
        }

        public DailyMainDishReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository ?? new ReportRepository();
        }

        /// <summary>
        /// Get daily main dish report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The daily main dish report.</returns>
        public DailyMainDishReportModel GetDailyMainDishReport(GetDailyMainDishPricePerMealReportCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var report = new DailyMainDishReportModel();

            if (criteria.IsValid())
            {
                report = _reportRepository.GetDailyMainDishReport(criteria);
            }

            return report;
        }

        /// <summary>
        /// Get paging report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The Daily Main Dish report.</returns>
        public DailyMainDishReportModel GetPagingReport(
            DailyMainDishReportModel report,
            int startIndex,
            int itemsPerPage)
        {
            var pagingReport = new DailyMainDishReportModel
            {
                SubTotal = report.SubTotal,
                GrandTotal = report.GrandTotal,
                TotalAmount = report.TotalAmount,
                TotalItem = CalculateTotalItems(report)
            };

            pagingReport.MainDishes = LoopAndGetPagingReport(report, startIndex, itemsPerPage);
            return pagingReport;
        }

        /// <summary>
        /// Calculate total items of report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns>The total items.</returns>
        private int CalculateTotalItems(DailyMainDishReportModel report)
        {
            int totalItem = 0;

            foreach (DailyMainDishDepartmentTeamModel mainDish in report.MainDishes)
            {
                totalItem += mainDish.EmployeeDishes.Count();
            }

            return totalItem;
        }

        /// <summary>
        /// Loop and get paging report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The list new main dished of department and team.</returns>
        private List<DailyMainDishDepartmentTeamModel> LoopAndGetPagingReport(
            DailyMainDishReportModel report,
            int startIndex,
            int itemsPerPage)
        {
            var mainDishes = new List<DailyMainDishDepartmentTeamModel>();
            int numberItemsInMainDish = 0;
            int numberMainDishes = report.MainDishes.Count;
            int index = 0;

            while (index < numberMainDishes)
            {
                DailyMainDishDepartmentTeamModel mainDish = report.MainDishes[index];
                numberItemsInMainDish = mainDish.EmployeeDishes.Count;

                if (numberItemsInMainDish > startIndex)
                {
                    int numberItemsRemain = numberItemsInMainDish - startIndex;

                    if (numberItemsRemain <= itemsPerPage)
                    {
                        DailyMainDishDepartmentTeamModel pagingMainDish = AddEmployeeItemIntoPagingReport(startIndex, mainDish, numberItemsRemain);
                        mainDishes.Add(pagingMainDish);

                        if (numberItemsRemain == itemsPerPage)
                        {
                            break;
                        }
                        else
                        {
                            startIndex = 0;
                            itemsPerPage -= numberItemsRemain;
                        }
                    }
                    else
                    {
                        DailyMainDishDepartmentTeamModel paingMainDish = AddEmployeeItemIntoPagingReport(startIndex, mainDish, itemsPerPage);
                        mainDishes.Add(paingMainDish);
                        break;
                    }
                }
                else
                {
                    startIndex -= numberItemsInMainDish;
                }

                index++;
            }

            return mainDishes;
        }

        private DailyMainDishDepartmentTeamModel AddEmployeeItemIntoPagingReport(int startIndex, DailyMainDishDepartmentTeamModel mainDish, int numberItemsRemain)
        {
            var newMainDish = new DailyMainDishDepartmentTeamModel
            {
                Department = mainDish.Department,
                Team = mainDish.Team,
                EmployeeDishes = mainDish.EmployeeDishes.Skip(startIndex).Take(numberItemsRemain).ToList()
            };

            return newMainDish;
        }
    }
}
