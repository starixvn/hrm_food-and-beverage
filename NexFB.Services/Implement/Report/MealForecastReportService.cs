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

            for (int i = 0; i < report.MealForecasts.Count; i++)
            {
                MealForecastItemModel mealForecast = report.MealForecasts[i];
                FillContentRow(mealForecast, i + 1, ref writer, isVegetarian);
            }

            if (isVegetarian == null)
            {
                FillTotalRow(report.TotalNonVegetarian, Lang.TotalNonVegetarian, ref writer, isVegetarian);
                FillTotalRow(report.TotalVegetarian, Lang.TotalVegetarian, ref writer, isVegetarian);
                FillTotalRow(report.TotalMeal, Lang.TotalMeal, ref writer, isVegetarian);
            }
            else if (isVegetarian == true)
            {
                FillTotalRow(report.TotalVegetarian, Lang.TotalVegetarian, ref writer, isVegetarian);
            }
            else
            {
                FillTotalRow(report.TotalNonVegetarian, Lang.TotalNonVegetarian, ref writer, isVegetarian);
            }

            writer.SyncColumnsWidth(7);
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

                report.TotalNonVegetarian.NonRegistration += item.NonRegistrationNonVegetarian;
                report.TotalVegetarian.NonRegistration += item.NonRegistrationVegetarian;

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
            report.TotalMeal.NonRegistration += report.TotalNonVegetarian.NonRegistration;
            report.TotalMeal.NonRegistration += report.TotalVegetarian.NonRegistration;

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
        /// <param name="mealForecast">The meal forecast.</param>
        /// <param name="index">The index.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="isVegetarian">Is vegetarian.</param>
        private void FillContentRow(MealForecastItemModel mealForecast, int index, ref ExcelTemplateWriter writer, bool? isVegetarian)
        {
            if (isVegetarian == null)
            {
                int rowIndexNonVeg = GetRowStyleByOrderRate(mealForecast.EfficiencyNonVegetarian, true, false),
                    rowIndexVeg = GetRowStyleByOrderRate(mealForecast.EfficiencyVegetarian, true, true);

                var recordNonVeg = new
                {
                    Count = index,
                    Date = mealForecast.OnDate,
                    DietType = Lang.NonVegetarian,
                    NonRegistration = mealForecast.NonRegistrationNonVegetarian,
                    Registration = mealForecast.RegistrationNonVegetarian,
                    ActualOrder = FormatNumber(mealForecast.ActualOrderNonVegetarian, false),
                    Efficiency = FormatNumber(mealForecast.EfficiencyNonVegetarian, true)
                };
                var recordVeg = new
                {
                    Count = index,
                    Date = mealForecast.OnDate,
                    DietType = Lang.Vegetarian,
                    NonRegistration = mealForecast.NonRegistrationVegetarian,
                    Registration = mealForecast.RegistrationVegetarian,
                    ActualOrder = FormatNumber(mealForecast.ActualOrderVegetarian, false),
                    Efficiency = FormatNumber(mealForecast.EfficiencyVegetarian, true)
                };

                writer.WriteRow(rowIndexNonVeg, recordNonVeg);
                writer.WriteRow(rowIndexVeg, recordVeg);
            }
            else
            {
                int rowIndex = isVegetarian == true ? GetRowStyleByOrderRate(mealForecast.EfficiencyVegetarian, false, true) :
                    GetRowStyleByOrderRate(mealForecast.EfficiencyNonVegetarian, false, false);

                var record = new
                {
                    Count = index,
                    Date = mealForecast.OnDate,
                    DietType = isVegetarian == true ? Lang.Vegetarian : Lang.NonVegetarian,
                    NonRegistration = isVegetarian == true ? mealForecast.NonRegistrationVegetarian : mealForecast.RegistrationNonVegetarian,
                    Registration = isVegetarian == true ? mealForecast.RegistrationVegetarian : mealForecast.RegistrationNonVegetarian,
                    ActualOrder = isVegetarian == true ? FormatNumber(mealForecast.ActualOrderVegetarian, false) :
                        FormatNumber(mealForecast.ActualOrderNonVegetarian, false),
                    Efficiency = isVegetarian == true ? FormatNumber(mealForecast.EfficiencyVegetarian, true) :
                        FormatNumber(mealForecast.EfficiencyNonVegetarian, true)
                };
                writer.WriteRow(rowIndex, record);
            }
        }

        /// <summary>
        /// Fill total row.
        /// </summary>
        /// <param name="totalMeal">The total meal forecast.</param>
        /// <param name="totalName">The total name.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="isVegetarian">Is vegetarian.</param>
        private void FillTotalRow(TotalMealForecastModel totalMeal, string totalName, ref ExcelTemplateWriter writer, bool? isVegetarian)
        {
            int rowIndex = 8;

            if (totalMeal.Efficiency < 0)
            {
                rowIndex = isVegetarian == null ? 15 : 11;
            }
            else if (totalMeal.Efficiency < 100)
            {
                rowIndex = isVegetarian == null ? 13 : 9;
            }
            else if (totalMeal.Efficiency == 100)
            {
                rowIndex = isVegetarian == null ? 12 : 8;
            }
            else
            {
                rowIndex = isVegetarian == null ? 14 : 10;
            }

            var record = new
            {
                Count = totalName,
                Date = string.Empty,
                DietType = string.Empty,
                NonRegistration = totalMeal.NonRegistration,
                Registration = totalMeal.Registration,
                ActualOrder = FormatNumber(totalMeal.ActualOrder, false),
                Efficiency = FormatNumber(totalMeal.Efficiency, true)
            };

            writer.WriteRow(rowIndex, record);
        }

        /// <summary>
        /// Get row index by status Id.
        /// </summary>
        /// <param name="orderRate">The order rate.</param>
        /// <param name="isAllType">Is all type.</param>
        /// <param name="isVegetarian">Is vegetarian.</param>
        /// <returns>The row index.</returns>
        private int GetRowStyleByOrderRate(double orderRate, bool isAllType, bool isVegetarian)
        {
            int rowIndex = 4;
            if (isAllType)
            {
                if (orderRate < 0)
                {
                    rowIndex = isVegetarian == false ? 10 : 11;
                }
                else if (orderRate < 100)
                {
                    rowIndex = isVegetarian == false ? 6 : 7;
                }
                else if (orderRate == 100)
                {
                    rowIndex = isVegetarian == false ? 4 : 5;
                }
                else
                {
                    rowIndex = isVegetarian == false ? 8 : 9;
                }
            }
            else
            {
                if (orderRate < 0)
                {
                    rowIndex = 7;
                }
                else if (orderRate < 100)
                {
                    rowIndex = 5;
                }
                else if (orderRate == 100)
                {
                    rowIndex = 4;
                }
                else
                {
                    rowIndex = 6;
                }
            }

            return rowIndex;
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