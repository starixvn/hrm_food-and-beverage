// -----------------------------------------------------------------------
// <copyright file="MealForecastDetailReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/16">Created.</change>
// </history>

namespace NexFB.Services.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MealForecastReport;
    using NexFB.Repositories.Implement.Report;
    using NexFB.Repositories.Interface.Report;
    using NexFB.Resources;
    using NexFB.Services.Interface.Report;

    /// <summary>
    /// The implement of meal forecast detail report service.
    /// </summary>
    public class MealForecastDetailReportService : IMealForecastDetailReportService
    {
        private readonly IMealForecastDetailReportRepository _mealForecastDetailReportRepository;

        public MealForecastDetailReportService()
            : this(null)
        {
        }

        public MealForecastDetailReportService(IMealForecastDetailReportRepository mealForecastDetailReportRepository)
        {
            _mealForecastDetailReportRepository = mealForecastDetailReportRepository ?? new MealForecastDetailReportRepository();
        }

        /// <summary>
        /// Get meal forecast detail report
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The meal forecast report.</returns>
        public MealForecastDetailHistoryReport GetMealForecastDetailReport(GetMealForecastDetailReportCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var report = new MealForecastDetailHistoryReport();

            if (criteria.IsValid())
            {
                report = _mealForecastDetailReportRepository.GetMealForecastDetailReport(criteria);
                if (report.MealForecastDetails != null)
                {
                    int actualOrderNonVegNA = 0, actualOrderVegNA = 0,
                        numberRecord = report.MealForecastDetails.Count,
                        recordNonVeg = report.MealForecastDetails.Where(x => x.IsVegetarian == false).ToList().Count,
                        recordVeg = report.MealForecastDetails.Where(x => x.IsVegetarian == true).ToList().Count;

                    report.TotalNonVegetarian = new TotalMealForecastModel
                    {
                        Registration = 0,
                        ActualOrder = 0,
                        Efficiency = 0
                    };

                    report.TotalVegetarian = new TotalMealForecastModel
                    {
                        Registration = 0,
                        ActualOrder = 0,
                        Efficiency = 0
                    };

                    report.TotalMeal = new TotalMealForecastModel
                    {
                        Registration = 0,
                        ActualOrder = 0,
                        Efficiency = 0
                    };

                    foreach (var item in report.MealForecastDetails)
                    {
                        if (item.IsVegetarian == false)
                        {
                            report.TotalNonVegetarian.Registration += item.Registration;
                            if (item.ActualOrder < 0)
                            {
                                actualOrderNonVegNA++;
                                item.OrderRate = -1;
                            }
                            else
                            {
                                report.TotalNonVegetarian.ActualOrder += item.ActualOrder;
                                if (item.Registration.Equals(0))
                                {
                                    item.OrderRate = -1;
                                }
                                else
                                {
                                    item.OrderRate = Math.Round((double)item.ActualOrder / item.Registration * 100, 2);
                                }
                            }
                        }
                        else
                        {
                            report.TotalVegetarian.Registration += item.Registration;
                            if (item.ActualOrder < 0)
                            {
                                actualOrderVegNA++;
                                item.OrderRate = -1;
                            }
                            else
                            {
                                report.TotalVegetarian.ActualOrder += item.ActualOrder;
                                if (item.Registration.Equals(0))
                                {
                                    item.OrderRate = -1;
                                }
                                else
                                {
                                    item.OrderRate = Math.Round((double)item.ActualOrder / item.Registration * 100, 2);
                                }
                            }
                        }
                    }

                    // Calculate total non-vegetarian
                    if (actualOrderNonVegNA.Equals(recordNonVeg))
                    {
                        report.TotalNonVegetarian.ActualOrder = -1;
                        report.TotalNonVegetarian.Efficiency = -1;
                    }
                    else
                    {
                        if (report.TotalNonVegetarian.Registration.Equals(0))
                        {
                            report.TotalNonVegetarian.Efficiency = -1;
                        }
                        else
                        {
                            report.TotalNonVegetarian.Efficiency = Math.Round((double)report.TotalNonVegetarian.ActualOrder / report.TotalNonVegetarian.Registration * 100, 2);
                        }

                        report.TotalMeal.ActualOrder += report.TotalNonVegetarian.ActualOrder;
                    }

                    // Calculate total vegetarian
                    if (actualOrderVegNA.Equals(recordVeg))
                    {
                        report.TotalVegetarian.ActualOrder = -1;
                        report.TotalVegetarian.Efficiency = -1;
                    }
                    else
                    {
                        if (report.TotalVegetarian.Registration.Equals(0))
                        {
                            report.TotalVegetarian.Efficiency = -1;
                        }
                        else
                        {
                            report.TotalVegetarian.Efficiency = Math.Round((double)report.TotalVegetarian.ActualOrder / report.TotalVegetarian.Registration * 100, 2);
                        }

                        report.TotalMeal.ActualOrder += report.TotalVegetarian.ActualOrder;
                    }

                    // Calculate total meal
                    report.TotalMeal.Registration += report.TotalNonVegetarian.Registration;
                    report.TotalMeal.Registration += report.TotalVegetarian.Registration;
                    if ((actualOrderNonVegNA + actualOrderVegNA).Equals(numberRecord))
                    {
                        report.TotalMeal.ActualOrder = -1;
                        report.TotalMeal.Efficiency = -1;
                    }
                    else
                    {
                        if (report.TotalMeal.Registration.Equals(0))
                        {
                            report.TotalMeal.Efficiency = -1;
                        }
                        else
                        {
                            report.TotalMeal.Efficiency = Math.Round((double)report.TotalMeal.ActualOrder / report.TotalMeal.Registration * 100, 2);
                        }
                    }

                    report.TotalItem = report.MealForecastDetails.Count;
                }
            }

            return report;
        }

        /// <summary>
        /// Get paging report.
        /// </summary>
        /// <param name="fullReport">The full report.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The paging report.</returns>
        public MealForecastDetailHistoryReport GetPagingReport(MealForecastDetailHistoryReport fullReport, int startIndex, int itemsPerPage)
        {
            var pagingReport = new MealForecastDetailHistoryReport();

            if (fullReport.MealForecastDetails.Count > 0)
            {
                pagingReport.MealForecastDetails = GetPagingReportDetail(fullReport, startIndex, itemsPerPage);
                pagingReport.TotalItem = fullReport.TotalItem;
                pagingReport.TotalNonVegetarian = fullReport.TotalNonVegetarian;
                pagingReport.TotalVegetarian = fullReport.TotalVegetarian;
                pagingReport.TotalMeal = fullReport.TotalMeal;
            }

            return pagingReport;
        }

        /// <summary>
        /// Export report.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <param name="templatePath">The template path.</param>
        /// <param name="isVegetarian">The is vegetarian.</param>
        /// <param name="date">The date.</param>
        public void ExportMealForecastDetailReport(MealForecastDetailHistoryReport report, string templatePath, bool? isVegetarian, DateTime onDate)
        {
            string reportName = Lang.MealForecastDetailReportName;
            string reportTitle = Lang.MealForecastDetailReportTitle + " For " + onDate.ToString("dd MMM, yyyy");
            string reportSheetName = Lang.MealForecastDetailReportSheetName;
            var writer = new ExcelTemplateWriter(templatePath, reportName, reportSheetName);

            // Fill report title.
            writer.CopyRows(0, 2);
            writer.WriteRow(2, new object[] { reportTitle });

            // Fill report header.
            writer.CopyRows(3, 1);

            FillContentRow(report.MealForecastDetails, ref writer);
            if (isVegetarian == null)
            {
                FillTotalRow(report.TotalNonVegetarian, Lang.TotalNonVegetarian, 5, ref writer);
                FillTotalRow(report.TotalVegetarian, Lang.TotalVegetarian, 5, ref writer);
                FillTotalRow(report.TotalMeal, Lang.TotalMeal, 5, ref writer);
            }
            else if (isVegetarian == true)
            {
                FillTotalRow(report.TotalVegetarian, Lang.TotalVegetarian, 5, ref writer);
            }
            else
            {
                FillTotalRow(report.TotalNonVegetarian, Lang.TotalNonVegetarian, 5, ref writer);
            }

            writer.SyncColumnsWidth(8);
            writer.Render();
        }

        /// <summary>
        /// Get meal forecast detail of paging report.
        /// </summary>
        /// <param name="fullReport">The full report.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The list meal forecast detail.</returns>
        public List<MealForecastDetailModel> GetPagingReportDetail(MealForecastDetailHistoryReport fullReport, int startIndex, int itemsPerPage)
        {
            int numberMealForecasts = fullReport.MealForecastDetails.Count;
            int numberTakenItems = 0;

            if (startIndex + itemsPerPage <= numberMealForecasts)
            {
                numberTakenItems = itemsPerPage;
            }
            else
            {
                numberTakenItems = numberMealForecasts - startIndex;
            }

            return fullReport.MealForecastDetails.Skip(startIndex).Take(numberTakenItems).ToList();
        }

        /// <summary>
        /// Fill content report.
        /// </summary>
        /// <param name="mealForecastDetails">The list meal forecast detail.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="isVegetarian">The is vegetarian.</param>
        private void FillContentRow(List<MealForecastDetailModel> mealForecastDetails, ref ExcelTemplateWriter writer)
        {
            int numberRecords = mealForecastDetails.Count;

            for (int i = 0; i < numberRecords; i++)
            {
                MealForecastDetailModel mealForecastDetail = mealForecastDetails[i];

                var record = new
                {
                    Count = i + 1,
                    MenuVie = mealForecastDetail.MenuVie,
                    MenuEng = mealForecastDetail.MenuEng,
                    Meal = CreateMeal(mealForecastDetail.Meals),
                    DietType = mealForecastDetail.IsVegetarian == true ? Lang.Vegetarian : Lang.NonVegetarian,
                    Registration = mealForecastDetail.Registration,
                    ActualOrder = FormatValueByActualOrder(mealForecastDetail.ActualOrder),
                    OrderRate = FormatValueByOrderRate(mealForecastDetail.OrderRate)
                };

                writer.WriteRow(4, record);
            }
        }

        /// <summary>
        /// Fill total row.
        /// </summary>
        /// <param name="totalMeal">The total meal forecast.</param>
        /// <param name="totalName">The total name.</param>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="writer">The writer.</param>
        private void FillTotalRow(TotalMealForecastModel totalMeal, string totalName, int rowIndex, ref ExcelTemplateWriter writer)
        {
            var record = new
            {
                Count = totalName,
                MenuVie = string.Empty,
                MenuEng = string.Empty,
                Meal = string.Empty,
                DietType = string.Empty,
                Registration = totalMeal.Registration,
                ActualOrder = FormatValueByActualOrder(totalMeal.ActualOrder),
                OrderRate = FormatValueByOrderRate(totalMeal.Efficiency)
            };

            writer.WriteRow(rowIndex, record);
        }

        /// <summary>
        /// Create meal.
        /// </summary>
        /// <param name="meals">The list meal.</param>
        /// <returns>The meal template.</returns>
        private string CreateMeal(List<MealModel> meals)
        {
            string mealTemplate = string.Empty;

            foreach (MealModel meal in meals)
            {
                string mealItem = "- " + meal.MealName;
                mealTemplate += !string.IsNullOrEmpty(mealTemplate) ? Environment.NewLine + mealItem : mealItem;
            }

            return mealTemplate;
        }

        /// <summary>
        /// Format value by actual order.
        /// </summary>
        /// <param name="actualOrder">The actual order.</param>
        /// <returns>The dynamic value.</returns>
        private dynamic FormatValueByActualOrder(int actualOrder)
        {
            if (actualOrder < 0)
            {
                return "-";
            }

            return actualOrder;
        }

        /// <summary>
        /// Format value by order rate.
        /// </summary>
        /// <param name="orderRate">The order rate.</param>
        /// <returns>The dynamic value.</returns>
        private dynamic FormatValueByOrderRate(double orderRate)
        {
            if (orderRate < 0)
            {
                return "-";
            }

            return orderRate.ToString() + "%";
        }
    }
}