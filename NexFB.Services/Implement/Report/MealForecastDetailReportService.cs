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
    using NexFB.Criteria.MealRegistration;
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
        /// Get meal forecast detail report.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The meal forecast detail history report.</returns>
        public MealForecastDetailReport GetMealForecastDetailReport(GetMealForecastDetailReportCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var report = new MealForecastDetailReport();
            if (criteria.IsValid())
            {
                report = _mealForecastDetailReportRepository.GetMealForecastDetailReport(criteria);

                report = CalculateTotalMeal(report);

                report.TotalMeal.Registration += report.TotalNonVegetarian.Registration;
                report.TotalMeal.Registration += report.TotalVegetarian.Registration;
                report.TotalMeal.NonRegistration += report.TotalNonVegetarian.NonRegistration;
                report.TotalMeal.NonRegistration += report.TotalVegetarian.NonRegistration;

                if (report.TotalNonVegetarian.ActualOrder < 0 && report.TotalVegetarian.ActualOrder < 0)
                {
                    report.TotalMeal.ActualOrder = -1;
                }

                report.TotalItem = report.MealForecastDetails.Count;
            }

            return report;
        }

        public EmployeeRegistrationReportModel GetEmployeeRegistration(GetEmployeeRegistrationCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var report = new EmployeeRegistrationReportModel();

            if (criteria.IsValid())
            {
                var employeeMeals = _mealForecastDetailReportRepository.GetEmployeeRegistration(criteria);
                report.EmployeeRegistrations = employeeMeals;
                report.TotalItem = employeeMeals.Count;
                report.TotalMeal = GetTotalMeal(report);
            }

            return report;
        }
        public EmployeeRegistrationReportModel GetEmployeeNonRegistration(GetEmployeeNonRegistrationCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var report = new EmployeeRegistrationReportModel();

            if (criteria.IsValid())
            {
                var employeeMeals = _mealForecastDetailReportRepository.GetEmployeeNonRegistration(criteria);
                report.EmployeeNonRegistrations = employeeMeals;
                report.TotalItem = employeeMeals.Count;
                report.TotalMeal = GetTotalNonRegMeal(report);
            }

            return report;
        }

        public EmployeeRegistrationReportModel GetPagingEmployeeNonRegistration(EmployeeRegistrationReportModel fullReport, int startIndex, int itemsPerPage)
        {
            var pagingReport = new EmployeeRegistrationReportModel();

            if (fullReport.EmployeeNonRegistrations.Count > 0)
            {
                pagingReport.EmployeeNonRegistrations = GetPagingEmployeeNonRegistrationDetail(fullReport, startIndex, itemsPerPage);
                pagingReport.TotalItem = fullReport.TotalItem;
                pagingReport.TotalMeal = fullReport.TotalMeal;
            }

            return pagingReport;
        }
        /// <summary>
        /// Get paging report.
        /// </summary>
        /// <param name="fullReport">The full report.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>The paging report.</returns>
        public MealForecastDetailReport GetPagingReport(
            MealForecastDetailReport fullReport,
            int startIndex,
            int itemsPerPage)
        {
            var pagingReport = new MealForecastDetailReport();

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

        public EmployeeRegistrationReportModel GetPagingEmployeeRegistration(
           EmployeeRegistrationReportModel fullReport,
           int startIndex,
           int itemsPerPage)
        {
            var pagingReport = new EmployeeRegistrationReportModel();

            if (fullReport.EmployeeRegistrations.Count > 0)
            {
                pagingReport.EmployeeRegistrations = GetPagingEmployeeRegistrationDetail(fullReport, startIndex, itemsPerPage);
                pagingReport.TotalItem = fullReport.TotalItem;
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
        /// <param name="dateSelected">The date selected.</param>
        public void ExportMealForecastDetailReport(
            MealForecastDetailReport report,
            string templatePath,
            bool? isVegetarian,
            DateTime dateSelected)
        {
            string reportName = Lang.MealForecastDetailReportName + dateSelected.ToString("dd_MMM_yyyy");
            string reportTitle = Lang.MealForecastDetailReportTitle + dateSelected.ToString("dd MMM, yyyy");
            string reportSheetName = Lang.MealForecastDetailReportSheetName + dateSelected.ToString("dd MMM, yyyy");
            var writer = new ExcelTemplateWriter(templatePath, reportName, reportSheetName);

            // Fill report title.
            writer.CopyRows(0, 2);
            writer.WriteRow(2, new object[] { reportTitle });

            // Fill report header.
            writer.CopyRows(3, 1);

            FillContentRow(report.MealForecastDetails, ref writer);

            if (isVegetarian == null)
            {
                FillTotalRow(report.TotalNonVegetarian, Lang.TotalNonVegetarian, ref writer);
                FillTotalRow(report.TotalVegetarian, Lang.TotalVegetarian, ref writer);
                FillTotalRow(report.TotalMeal, Lang.TotalMeal, ref writer);
            }
            else if (isVegetarian == true)
            {
                FillTotalRow(report.TotalVegetarian, Lang.TotalVegetarian, ref writer);
            }
            else
            {
                FillTotalRow(report.TotalNonVegetarian, Lang.TotalNonVegetarian, ref writer);
            }

            writer.SyncColumnsWidth(9);
            writer.Render();
        }

        public void ExportEmployeeRegistrationReport(
            EmployeeRegistrationReportModel report,
            string templatePath,
            bool? isVegetarian,
            DateTime dateSelected)
        {
            string reportName = Lang.MealRegistrationDetailReportFileName + dateSelected.ToString("dd_MMM_yyyy");
            string reportTitle = Lang.EmployeeRegistrationDetail + " " + dateSelected.ToString("dd MMM, yyyy");
            string reportSheetName = dateSelected.ToString("dd MMM, yyyy");
            var writer = new ExcelTemplateWriter(templatePath, reportName, reportSheetName);

            // Fill report title.
            writer.CopyRows(0, 2);
            writer.WriteRow(2, new object[] { reportTitle });

            // Fill report header.
            writer.CopyRows(3, 1);

            FillContentRowForEmployeeRegistration(report.EmployeeRegistrations, ref writer);

            FillTotalRowForEmployeeRegistration(report.TotalMeal, Lang.TotalMeal, ref writer);

            writer.SyncColumnsWidth(9);
            writer.Render();
        }
        public void ExportEmployeeNonRegistrationReport(
            EmployeeRegistrationReportModel report,
            string templatePath,
            bool? isVegetarian,
            DateTime dateSelected)
        {
            string reportName = Lang.MealNonRegistrationDetailReportFileName + dateSelected.ToString("dd_MMM_yyyy");
            string reportTitle = Lang.EmployeeNonRegistrationDetail + " " + dateSelected.ToString("dd MMM, yyyy");
            string reportSheetName = dateSelected.ToString("dd MMM, yyyy");
            var writer = new ExcelTemplateWriter(templatePath, reportName, reportSheetName);

            // Fill report title.
            writer.CopyRows(0, 2);
            writer.WriteRow(2, new object[] { reportTitle });

            // Fill report header.
            writer.CopyRows(3, 1);

            FillContentRowForEmployeeNonRegistration(report.EmployeeNonRegistrations, ref writer);

            FillTotalRowForEmployeeNonRegistration(report.TotalMeal, Lang.TotalMeal, ref writer);

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
        public List<MealForecastDetailModel> GetPagingReportDetail(
            MealForecastDetailReport fullReport,
            int startIndex,
            int itemsPerPage)
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

        public IList<EmployeeRegistrationModel> GetPagingEmployeeRegistrationDetail(
            EmployeeRegistrationReportModel fullReport,
            int startIndex,
            int itemsPerPage)
        {
            int numberRegistrations = fullReport.EmployeeRegistrations.Count;
            int numberTakenItems = 0;

            if (startIndex + itemsPerPage <= numberRegistrations)
            {
                numberTakenItems = itemsPerPage;
            }
            else
            {
                numberTakenItems = numberRegistrations - startIndex;
            }

            return fullReport.EmployeeRegistrations.Skip(startIndex).Take(numberTakenItems).ToList();
        }
        public IList<EmployeeRegistrationModel> GetPagingEmployeeNonRegistrationDetail(
            EmployeeRegistrationReportModel fullReport,
            int startIndex,
            int itemsPerPage)
        {
            int numberNonRegistrations = fullReport.EmployeeNonRegistrations.Count;
            int numberTakenItems = 0;

            if (startIndex + itemsPerPage <= numberNonRegistrations)
            {
                numberTakenItems = itemsPerPage;
            }
            else
            {
                numberTakenItems = numberNonRegistrations - startIndex;
            }

            return fullReport.EmployeeNonRegistrations.Skip(startIndex).Take(numberTakenItems).ToList();
        }

        /// <summary>
        /// Calculate total meal.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns>The meal forecast detail history report.</returns>
        private MealForecastDetailReport CalculateTotalMeal(MealForecastDetailReport report)
        {
            foreach (var item in report.MealForecastDetails)
            {
                if (item.IsVegetarian == false)
                {
                    report.TotalNonVegetarian.Registration += item.Registration;
                    report.TotalNonVegetarian.NonRegistration += item.NonRegistration;
                    if (item.ActualOrder >= 0)
                    {
                        report.TotalNonVegetarian.ActualOrder += item.ActualOrder;
                    }
                }
                else
                {
                    report.TotalVegetarian.Registration += item.Registration;
                    report.TotalVegetarian.NonRegistration += item.NonRegistration;
                    if (item.ActualOrder >= 0)
                    {
                        report.TotalVegetarian.ActualOrder += item.ActualOrder;
                    }
                }
            }

            int numberActualOrderVeg = report.MealForecastDetails.Where(x => x.IsVegetarian == true).ToList().Count,
                numberActualOrderNonVeg = report.MealForecastDetails.Where(x => x.IsVegetarian == false).ToList().Count,
                numberActualOrderVegNA = report.MealForecastDetails.Where(x => x.IsVegetarian == true && x.ActualOrder < 0).ToList().Count,
                numberActualOrderNonVegNA = report.MealForecastDetails.Where(x => x.IsVegetarian == false && x.ActualOrder < 0).ToList().Count;

            if (numberActualOrderNonVegNA.Equals(numberActualOrderNonVeg))
            {
                report.TotalNonVegetarian.ActualOrder = -1;
            }
            else
            {
                report.TotalMeal.ActualOrder += report.TotalNonVegetarian.ActualOrder;
            }

            if (numberActualOrderVegNA.Equals(numberActualOrderVeg))
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
        /// <param name="mealForecastDetails">The list meal forecast detail.</param>
        /// <param name="writer">The writer.</param>
        private void FillContentRow(List<MealForecastDetailModel> mealForecastDetails, ref ExcelTemplateWriter writer)
        {
            int numberRecords = mealForecastDetails.Count, rowIndex = 4;

            for (int i = 0; i < numberRecords; i++)
            {
                MealForecastDetailModel mealForecastDetail = mealForecastDetails[i];

                if (mealForecastDetail.OrderRate < 0)
                {
                    rowIndex = 7;
                }
                else if (mealForecastDetail.OrderRate < 100)
                {
                    rowIndex = 5;
                }
                else if (mealForecastDetail.OrderRate.Equals(100))
                {
                    rowIndex = 4;
                }
                else
                {
                    rowIndex = 6;
                }

                var record = new
                {
                    Count = i + 1,
                    MenuVie = mealForecastDetail.MenuVie,
                    MenuEng = mealForecastDetail.MenuEng,
                    Meal = CreateMeal(mealForecastDetail.Meals),
                    DietType = mealForecastDetail.IsVegetarian == true ? Lang.Vegetarian : Lang.NonVegetarian,
                    NonRegistration = mealForecastDetail.NonRegistration,
                    Registration = mealForecastDetail.Registration,
                    ActualOrder = FormatNumber(mealForecastDetail.ActualOrder, false),
                    OrderRate = FormatNumber(mealForecastDetail.OrderRate, true)
                };

                writer.WriteRow(rowIndex, record);
            }
        }

        private void FillContentRowForEmployeeRegistration(IList<EmployeeRegistrationModel> registrations, ref ExcelTemplateWriter writer)
        {
            int rowIndex = 4;
            int i = 0;
            foreach (EmployeeRegistrationModel registration in registrations)
            {
                i++;
                var record = new
                {
                    Count = i,
                    Code = registration.Employee.Code,
                    Deparment = registration.CompanyName,
                    Shift = registration.ShiftName,
                    NickName = registration.Employee.NickName,
                    FullName = registration.Employee.FullName,
                    Meal = registration.Meal.MealName,
                    DietType = registration.IsVegetarian == true ? Lang.Vegetarian : (registration.IsVegetarian == false ? Lang.NonVegetarian : "-"),
                    ActualOrder = registration.ActualOrder
                };

                writer.WriteRow(rowIndex, record);
            }
        }
        private void FillContentRowForEmployeeNonRegistration(IList<EmployeeRegistrationModel> nonregistrations, ref ExcelTemplateWriter writer)
        {
            int rowIndex = 4;
            int i = 0;
            foreach (EmployeeRegistrationModel nonregistration in nonregistrations)
            {
                i++;
                var record = new
                {
                    Count = i,
                    Code = nonregistration.Employee.Code,
                    Deparment = nonregistration.CompanyName,
                    Shift = nonregistration.ShiftName,
                    NickName = nonregistration.Employee.NickName,
                    FullName = nonregistration.Employee.FullName,
                    Meal = nonregistration.Meal.MealName,
                    DietType = nonregistration.IsVegetarian == true ? Lang.Vegetarian : (nonregistration.IsVegetarian == false ? Lang.NonVegetarian : "-")
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
        private void FillTotalRow(TotalMealForecastModel totalMeal, string totalName, ref ExcelTemplateWriter writer)
        {
            int rowIndex = 8;
            if (totalMeal.Efficiency < 0)
            {
                rowIndex = 11;
            }
            else if (totalMeal.Efficiency < 100)
            {
                rowIndex = 9;
            }
            else if (totalMeal.Efficiency.Equals(100))
            {
                rowIndex = 8;
            }
            else
            {
                rowIndex = 10;
            }

            var record = new
            {
                Count = totalName,
                MenuVie = string.Empty,
                MenuEng = string.Empty,
                Meal = string.Empty,
                DietType = string.Empty,
                NonRegistration = totalMeal.NonRegistration,
                Registration = totalMeal.Registration,
                ActualOrder = FormatNumber(totalMeal.ActualOrder, false),
                OrderRate = FormatNumber(totalMeal.Efficiency, true)
            };

            writer.WriteRow(rowIndex, record);
        }

        private void FillTotalRowForEmployeeRegistration(TotalMealForecastModel totalMeal, string totalName, ref ExcelTemplateWriter writer)
        {
            int rowIndex = 6;
            var record = new
            {
                Count = totalName,
                Code = string.Empty,
                Company = string.Empty,
                Shift = string.Empty,
                NickName = string.Empty,
                FullName = string.Empty,
                Meal = string.Empty,
                DietType = FormatNumber(totalMeal.Registration, false),
                ActualOrder = FormatNumber(totalMeal.ActualOrder, false)
            };

            writer.WriteRow(rowIndex, record);
        }
        private void FillTotalRowForEmployeeNonRegistration(TotalMealForecastModel totalMeal, string totalName, ref ExcelTemplateWriter writer)
        {
            int rowIndex = 6;
            var record = new
            {
                Count = totalName,
                Code = string.Empty,
                Company = string.Empty,
                Shift = string.Empty,
                NickName = string.Empty,
                FullName = string.Empty,
                Meal = string.Empty,
                DietType = FormatNumber(totalMeal.NonRegistration, false),
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

        private TotalMealForecastModel GetTotalMeal(EmployeeRegistrationReportModel registrations)
        {
            TotalMealForecastModel total = new TotalMealForecastModel();

            total.Registration = registrations.EmployeeRegistrations.Where(t => t.IsVegetarian != null).Count();
            total.ActualOrder = registrations.EmployeeRegistrations.Sum(t => t.ActualOrder);

            return total;
        }
        private TotalMealForecastModel GetTotalNonRegMeal(EmployeeRegistrationReportModel Nonregistrations)
        {
            TotalMealForecastModel total = new TotalMealForecastModel();

            total.NonRegistration = Nonregistrations.EmployeeNonRegistrations.Where(t => t.IsVegetarian != null).Count();
            return total;
        }

    }
}