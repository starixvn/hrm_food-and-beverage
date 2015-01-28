// -----------------------------------------------------------------------
// <copyright file="MealForecastReportService.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Tim.Trinh" date="2015/1/7">Created.</change>
// </history>

namespace NexFB.Services.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using NexFB.Core.Services;
    using NexFB.Core.Utilities;
    using NexFB.Criteria.Report;
    using NexFB.Models.Report.MealForecastReport;
    using NexFB.Resources;
    using NexFB.Services.Interface.Report;

    /// <summary>
    /// The implement of interface meal forecast report service.
    /// </summary>
    public class MealForecastReportService : BaseService, IMealForecastReportService
    {
        /// <summary>
        /// Get meal forecast report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The meal forecast report.</returns>
        public MealForecastReportModel GetMealForecastReport(GetMealForecastReportCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var report = new MealForecastReportModel();

            if (criteria.IsValid())
            {
                report.MealForecasts = this.GetData<MealForecastItemModel>(criteria).ToList();

                report = CalculateTotalMeal(report);

                if (report.TotalNonVegetarian.ActualOrder < 0 && report.TotalVegetarian.ActualOrder < 0)
                {
                    report.TotalMeal.ActualOrder = -1;
                }

                report.TotalItem = report.MealForecasts.Count;
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
        public MealForecastReportModel GetPagingReport(MealForecastReportModel fullReport, int startIndex, int itemsPerPage)
        {
            var pagingReport = new MealForecastReportModel();

            if (fullReport.MealForecasts.Count > 0)
            {
                pagingReport.MealForecasts = GetPagingReportDetail(fullReport, startIndex, itemsPerPage);
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
        public void ExportMealForecastReport(MealForecastReportModel report, string templatePath, bool? isVegetarian)
        {
            string reportName = Lang.MealForecastReportName;
            string reportTitle = Lang.MealForecastReportTitle;
            string reportSheetName = Lang.MealForecastReportSheetName;
            var writer = new ExcelTemplateWriter(templatePath, reportName, reportSheetName);

            // Fill report title.
            writer.CopyRows(0, 2);
            writer.WriteRow(2, new object[] { reportTitle });

            // Fill report header.
            writer.CopyRows(3, 1);

            FillContentRow(report.MealForecasts, ref writer, isVegetarian);
            if (isVegetarian == null)
            {
                FillTotalRow(report.TotalNonVegetarian, Lang.TotalNonVegetarian, 6, ref writer);
                FillTotalRow(report.TotalVegetarian, Lang.TotalVegetarian, 6, ref writer);
                FillTotalRow(report.TotalMeal, Lang.TotalMeal, 6, ref writer);
            }
            else if (isVegetarian == true)
            {
                FillTotalRow(report.TotalVegetarian, Lang.TotalVegetarian, 5, ref writer);
            }
            else
            {
                FillTotalRow(report.TotalNonVegetarian, Lang.TotalNonVegetarian, 5, ref writer);
            }

            writer.SyncColumnsWidth(6);
            writer.Render();
        }

        /// <summary>
        /// Get meal forecast of paging report.
        /// </summary>
        /// <param name="fullReport">The full report.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The list meal forecast.</returns>
        public List<MealForecastItemModel> GetPagingReportDetail(MealForecastReportModel fullReport, int startIndex, int itemsPerPage)
        {
            int numberMealForecasts = fullReport.MealForecasts.Count;
            int numberTakenItems = 0;

            if (startIndex + itemsPerPage <= numberMealForecasts)
            {
                numberTakenItems = itemsPerPage;
            }
            else
            {
                numberTakenItems = numberMealForecasts - startIndex;
            }

            return fullReport.MealForecasts.Skip(startIndex).Take(numberTakenItems).ToList();
        }

        /// <summary>
        /// Calculate total meal.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns>The meal forecast report.</returns>
        private MealForecastReportModel CalculateTotalMeal(MealForecastReportModel report)
        {
            int numberActualOrderVegNA = 0,
                numberActualOrderNonVegNA = 0;

            foreach (var item in report.MealForecasts)
            {
                report.TotalNonVegetarian.Registration += item.RegistrationNonVegetarian;
                report.TotalVegetarian.Registration += item.RegistrationVegetarian;
                if (item.ActualOrderNonVegetarian < 0)
                {
                    numberActualOrderNonVegNA++;
                }
                else
                {
                    report.TotalNonVegetarian.ActualOrder += item.ActualOrderNonVegetarian;
                }

                if (item.ActualOrderVegetarian < 0)
                {
                    numberActualOrderVegNA++;
                }
                else
                {
                    report.TotalVegetarian.ActualOrder += item.ActualOrderVegetarian;
                }
            }

            report.TotalMeal.Registration += report.TotalNonVegetarian.Registration;
            report.TotalMeal.Registration += report.TotalVegetarian.Registration;

            if (numberActualOrderNonVegNA.Equals(report.MealForecasts.Count))
            {
                report.TotalNonVegetarian.ActualOrder = -1;
            }
            else
            {
                report.TotalMeal.ActualOrder += report.TotalNonVegetarian.ActualOrder;
            }

            if (numberActualOrderVegNA.Equals(report.MealForecasts.Count))
            {
                report.TotalVegetarian.ActualOrder = -1;
            }
            else
            {
                report.TotalMeal.ActualOrder += report.TotalVegetarian.ActualOrder;
            }

            return report;
        }

        /// <summary>
        /// Fill content report.
        /// </summary>
        /// <param name="mealForecasts">The list meal forecast.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="isVegetarian">The is vegetarian.</param>
        private void FillContentRow(List<MealForecastItemModel> mealForecasts, ref ExcelTemplateWriter writer, bool? isVegetarian)
        {
            int numberRecords = mealForecasts.Count;
            if (isVegetarian == null)
            {
                for (int i = 0; i < numberRecords; i++)
                {
                    MealForecastItemModel mealForecast = mealForecasts[i];
                    var recordNonVeg = new
                    {
                        Count = i + 1,
                        Date = mealForecast.OnDate,
                        DietType = Lang.NonVegetarian,
                        Registration = mealForecast.RegistrationNonVegetarian,
                        ActualOrder = FormatNumber(mealForecast.ActualOrderNonVegetarian, false),
                        Efficiency = FormatNumber(mealForecast.EfficiencyNonVegetarian, true)
                    };
                    var recordVeg = new
                    {
                        Count = i + 1,
                        Date = mealForecast.OnDate,
                        DietType = Lang.Vegetarian,
                        Registration = mealForecast.RegistrationVegetarian,
                        ActualOrder = FormatNumber(mealForecast.ActualOrderVegetarian, false),
                        Efficiency = FormatNumber(mealForecast.EfficiencyVegetarian, true)
                    };
                    writer.WriteRow(4, recordNonVeg);
                    writer.WriteRow(5, recordVeg);
                }
            }
            else
            {
                for (int i = 0; i < numberRecords; i++)
                {
                    MealForecastItemModel mealForecast = mealForecasts[i];
                    var record = new
                    {
                        Count = i + 1,
                        Date = mealForecast.OnDate,
                        DietType = isVegetarian == true ? Lang.Vegetarian : Lang.NonVegetarian,
                        Registration = isVegetarian == true ? mealForecast.RegistrationVegetarian : mealForecast.RegistrationNonVegetarian,
                        ActualOrder = isVegetarian == true ? FormatNumber(mealForecast.ActualOrderVegetarian, false) :
                            FormatNumber(mealForecast.ActualOrderNonVegetarian, false),
                        Efficiency = isVegetarian == true ? FormatNumber(mealForecast.EfficiencyVegetarian, true) :
                            FormatNumber(mealForecast.EfficiencyNonVegetarian, true)
                    };
                    writer.WriteRow(4, record);
                }
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
                Date = string.Empty,
                DietType = string.Empty,
                Registration = totalMeal.Registration,
                ActualOrder = FormatNumber(totalMeal.ActualOrder, false),
                Efficiency = FormatNumber(totalMeal.Efficiency, true)
            };

            writer.WriteRow(rowIndex, record);
        }

        /// <summary>
        /// Format number.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="isPercent">Is percent.</param>
        /// <returns>The dynamic type.</returns>
        private dynamic FormatNumber(dynamic number, bool isPercent)
        {
            if (number < 0)
            {
                return "-";
            }
            else if (isPercent)
            {
                return number + "%";
            }

            return number;
        }
    }
}