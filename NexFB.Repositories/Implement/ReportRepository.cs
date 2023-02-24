
namespace NexFB.Repositories.Implement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NexFB.Models;
    using NexFB.Models.Employee;
    using NexFB.Models.Entities;
    using NexFB.Models.MealRegistration;
    using NexFB.Models.Report.DailyMainDishPricePerMealReport;
    using NexFB.Models.Report.MonthlyMainDishPerDay;
    using NexFB.Models.Report.MonthlyMainDishReportLocal;
    using NexFB.Models.Report.SideDishReport;
    using NexFB.Models.Report.SideDishSummaryReport;
    using NexFB.Models.Report.ViewMenuReport;
    using NexFB.Repositories.Interface;
    public class ReportRepository : IReportRepository
    {
        public MonthlyLocalReportModel GetMonthlyMainDishReportLocal(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var report = new MonthlyLocalReportModel();

                IEnumerable<dynamic> rawReport = db.Query<dynamic>(criteria.GetSpParams());
                if (rawReport != null)
                {
                    report.Meals = GetMealsInMonth(rawReport);
                    report.WeekDays = GetWeekDay(rawReport);
                    report.Total = CalculateTotalMeals(report.Meals);
                }

                return report;
            }
        }

        public DailyMainDishReportModel GetDailyMainDishReport(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (GridReader multi = db.QueryMultiple(criteria.GetSpParams()))
                {
                    var report = new DailyMainDishReportModel();
                    IEnumerable<dynamic> rawReport = multi.Read<dynamic>();
                    report.MainDishes = GroupMainDishByCompanyTeam(rawReport);

                    report.Total = multi.Read<TotalPriceModel>().FirstOrDefault();

                    if (report.Total != null)
                    {
                        report.GrandTotal = CalculateTotalAmount(report.Total);
                    }

                    return report;
                }
            }
        }

        public MonthlyMainDishPerDayModel GetMonthlyMainDishPerDayReport(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var report = new MonthlyMainDishPerDayModel();

                IEnumerable<dynamic> rawReport = db.Query<dynamic>(criteria.GetSpParams());

                if (rawReport != null)
                {
                    report.TypeDays = GetMealsInMonthPerDay(rawReport);
                    report.WeekDays = GetWeekDayPerDay(rawReport);
                    report.Total = CalculateTotalMealsPerDay(report.TypeDays);
                }

                return report;
            }
        }

        public SideDishReportModel GetSideDishReport(ICriteria criteria, int? pageIndex = null, int? itemsPerPage = null)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                SideDishReportModel reportData = new SideDishReportModel();

                List<SideDishItemModel> rawReport = db.Query<SideDishItemModel>(criteria.GetSpParams()).ToList();

                var outParams = db.GetOuputParams();

                if (rawReport != null && rawReport.Count > 0)
                {
                    reportData.TotalItem = (int)outParams["totalitem"];
                    reportData.TotalQuantity = (decimal)outParams["totalquantity"];
                    reportData.TotalAmount = (decimal)outParams["totalamount"];

                    reportData.SideDishItems = rawReport;
                }

                return reportData;
            }
        }

        public ViewMenuReportModel GetViewMenuReport(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var report = new ViewMenuReportModel();
                IEnumerable<ViewMenuItemModel> rawReport = db.Query<ViewMenuItemModel>(criteria.GetSpParams());
                if (rawReport != null)
                {
                    report.Dates = rawReport.OrderBy(m => m.OnDate).GroupBy(m => m.OnDate).Select(g => g.Key);

                    report.Menus = rawReport.OrderBy(m => m.OnDate).GroupBy(x => x.MenuName)
                                    .Select(group => new MenuTypeModel
                                    {
                                        MenuName = group.Key,
                                        MaxMenuItemPerMeal = 0,
                                        MenuForEachDay = group.GroupBy(g => g.OnDate).Select(m => new MenuItemPerDateModel
                                        {
                                            OnDate = m.Key,
                                            Menus = m.OrderBy(s => s.OnDate).ToList()
                                        }).ToList()
                                    })
                                    .ToList();
                }

                foreach (var item in report.Menus)
                {
                    item.MaxMenuItemPerMeal = item.MenuForEachDay.Max(x => x.Menus.Count());
                    AddNullListWhenNoMealMenuItem(item, report);
                }

                return report;
            }
        }

        private void AddNullListWhenNoMealMenuItem(MenuTypeModel menu, ViewMenuReportModel report)
        {
            foreach (var date in report.Dates)
            {
                if (!menu.MenuForEachDay.Any(x => x.OnDate == date))
                {
                    var obj = new MenuItemPerDateModel
                    {
                        OnDate = date,
                        Menus = new List<ViewMenuItemModel>()
                    };
                    menu.MenuForEachDay.Add(obj);
                    menu.MenuForEachDay = menu.MenuForEachDay.OrderBy(x => x.OnDate).ToList();
                }
            }
        }
        public SideDishSummaryReportModel GetSideDishSummaryReport(ICriteria criteria, int? pageIndex = null, int? itemsPerPage = null)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                SideDishSummaryReportModel report = new SideDishSummaryReportModel();

                List<SideDishSummaryItemModel> rawReport = db.Query<SideDishSummaryItemModel>(criteria.GetSpParams()).ToList();

                var outParams = db.GetOuputParams();

                if (report != null && rawReport.Count > 0)
                {
                    report.SideDishSummarys = rawReport;

                    report.TotalItem = (int)outParams["totalitem"];
                    report.TotalQuantity = (decimal)outParams["totalquantity"];
                    report.TotalAmount = (decimal)outParams["totalamount"];
                }

                return report;
            }
        }

        #region Monthly Main Dish Per Day Report

        private List<MainDishPerDayModel> GetMealsInMonthPerDay(IEnumerable<dynamic> rawReport)
        {
            var meals = new List<MainDishPerDayModel>();
            IEnumerable<dynamic> offDayRows = rawReport.Where(x => x.IsOffDay == true);
            IEnumerable<dynamic> wordDayRows = rawReport.Where(x => x.IsOffDay == false);

            if (wordDayRows != null && wordDayRows.Any())
            {
                var wordDayMeals = new MainDishPerDayModel
                {
                    IsOffDay = false,
                    MealItems = CreateMealItemPerDay(wordDayRows)
                };

                meals.Add(wordDayMeals);
            }

            if (offDayRows != null && offDayRows.Any())
            {
                var offDayMeals = new MainDishPerDayModel
                {
                    IsOffDay = true,
                    MealItems = CreateMealItemPerDay(offDayRows)
                };

                meals.Add(offDayMeals);
            }

            return meals;
        }
        private List<MealItemMonthModel> CreateMealItemPerDay(IEnumerable<dynamic> rawMeals)
        {
            var mealItems = new List<MealItemMonthModel>();

            foreach (dynamic rawMeal in rawMeals)
            {
                MealItemMonthModel mealItem = GetItemOfMeal(rawMeal);
                mealItems.Add(mealItem);
            }

            return mealItems;
        }

        private MealDayOfMonthModel GetWeekDayPerDay(IEnumerable<dynamic> rawReport)
        {
            var weekDays = new MealDayOfMonthModel();

            dynamic rawWeekDay = rawReport.Where(x => x.IsOffDay == null).First();
            weekDays.NumberMeals = GetNumberMealsInMonth(rawWeekDay);

            return weekDays;
        }

        private MealDayOfMonthModel CalculateTotalMealsPerDay(List<MainDishPerDayModel> meals)
        {
            var total = new MealDayOfMonthModel();

            foreach (MainDishPerDayModel meal in meals)
            {
                foreach (MealItemMonthModel mealItem in meal.MealItems)
                {
                    List<int> numberMeals = mealItem.NumberMealsMonth.NumberMeals;

                    for (int i = 0; i < numberMeals.Count(); i++)
                    {
                        if (total.NumberMeals.Count < numberMeals.Count())
                        {
                            total.NumberMeals.Add(numberMeals[i]);
                        }
                        else
                        {
                            total.NumberMeals[i] += numberMeals[i];
                        }
                    }

                    total.TotalQuantity += mealItem.NumberMealsMonth.TotalQuantity;
                    total.Amount += mealItem.NumberMealsMonth.Amount;
                }
            }

            return total;
        }

        #endregion Monthly Main Dish Per Day Report

        private int CalculateTotalAmount(TotalPriceModel totalPriceModel)
        {
            int amount = totalPriceModel.Breakfast + totalPriceModel.Lunch + totalPriceModel.Dinner +
                            totalPriceModel.Supper;
            return amount;
        }

        private List<DailyMainDishCompanyTeamModel> GroupMainDishByCompanyTeam(IEnumerable<dynamic> rawReport)
        {
            var mainDishes = new List<DailyMainDishCompanyTeamModel>();
            IEnumerable<dynamic> rawCompanyIds = rawReport.Select(x => x.CompanyID).Distinct();

            foreach (dynamic rawCompanyId in rawCompanyIds)
            {
                IEnumerable<dynamic> rowCompanies = rawReport.Where(x => x.CompanyID == rawCompanyId);
                IEnumerable<dynamic> rawTeamIds = rowCompanies.Select(x => x.EmpTeamID).Distinct();

                foreach (dynamic rawTeamId in rawTeamIds)
                {
                    IEnumerable<dynamic> rowCompanyTeams = rowCompanies.Where(x => x.EmpTeamID == rawTeamId);
                    DailyMainDishCompanyTeamModel mainDish = CreateMainDishCompanyTeam(rowCompanyTeams);
                    mainDishes.Add(mainDish);
                }
            }

            return mainDishes;
        }

        private DailyMainDishCompanyTeamModel CreateMainDishCompanyTeam(
            IEnumerable<dynamic> rowCompanyTeams)
        {
            var mainDish = new DailyMainDishCompanyTeamModel();
            int index = 0;

            foreach (dynamic rowCompanyTeam in rowCompanyTeams)
            {
                if (index == 0)
                {
                    mainDish.Company = CreateMainDishCompany(rowCompanyTeam);
                    mainDish.Team = CreateMainDishTeam(rowCompanyTeam);
                }

                DailyEmployeeMainDishModel employeeDish = CreateEmployeeDish(rowCompanyTeam);
                mainDish.EmployeeDishes.Add(employeeDish);
            }

            return mainDish;
        }

        private Company CreateMainDishCompany(dynamic rowCompanyTeam)
        {
            int? companyId = rowCompanyTeam.CompanyID;

            if (companyId.HasValue)
            {
                var company = new Company
                {
                    CompanyId = (short)companyId,
                    Name = rowCompanyTeam.CompanyName
                };

                return company;
            }

            return null;
        }

        private Team CreateMainDishTeam(dynamic rowCompanyTeam)
        {
            int? teamId = rowCompanyTeam.EmpTeamID;

            if (teamId.HasValue)
            {
                var team = new Team
                {
                    TeamId = (int)teamId,
                    Code = rowCompanyTeam.EmpTeamCode,
                    Name = rowCompanyTeam.EmpTeamName
                };

                return team;
            }

            return null;
        }

        private DailyEmployeeMainDishModel CreateEmployeeDish(dynamic rowCompanyTeam)
        {
            Shift shift = CreateEmployeeShift(rowCompanyTeam);
            Employee employee = CreateEmployeeInfo(rowCompanyTeam);

            var employeeDish = new DailyEmployeeMainDishModel
            {
                WorkDate = rowCompanyTeam.WorkDate,
                Shift = shift,
                Employee = employee,
                Breakfast = rowCompanyTeam.Breakfast,
                Lunch = rowCompanyTeam.Lunch,
                Dinner = rowCompanyTeam.Dinner,
                Supper = rowCompanyTeam.Supper
            };

            return employeeDish;
        }

        private Shift CreateEmployeeShift(dynamic rowCompanyTeam)
        {
            int? shiftId = rowCompanyTeam.ShiftId;

            if (shiftId.HasValue)
            {
                var shift = new Shift
                {
                    ShiftId = (int)shiftId,
                    ShiftCode = rowCompanyTeam.ShiftCode
                };

                return shift;
            }

            return null;
        }

        private Employee CreateEmployeeInfo(dynamic rowCompanyTeam)
        {
            int? userId = rowCompanyTeam.SysID;

            if (userId.HasValue)
            {
                var employee = new Employee
                {
                    UserId = (int)userId,
                    DomainUsername = rowCompanyTeam.DomainUsername,
                    Code = rowCompanyTeam.Code,
                    FullName = rowCompanyTeam.FullName,
                    NickName = rowCompanyTeam.NickName
                };

                return employee;
            }

            return null;
        }

        private List<MealMonthModel> GetMealsInMonth(IEnumerable<dynamic> rawReport)
        {
            var meals = new List<MealMonthModel>();
            IEnumerable<dynamic> rawMealIds = rawReport.Where(x => x.MealID != null).Select(x => x.MealID).Distinct();

            foreach (dynamic mealId in rawMealIds)
            {
                IEnumerable<dynamic> rawMeals = rawReport.Where(x => x.MealID == mealId);
                MealMonthModel meal = CreateMealInMonth(rawMeals);

                meals.Add(meal);
            }

            return meals;
        }

        private MealMonthModel CreateMealInMonth(IEnumerable<dynamic> rawMeals)
        {
            var mealReport = new MealMonthModel();

            foreach (dynamic rawMeal in rawMeals)
            {
                if (mealReport.Meal == null)
                {
                    mealReport.Meal = GetMeal(rawMeal);
                }

                MealItemMonthModel mealItem = GetItemOfMeal(rawMeal);
                mealReport.MealItems.Add(mealItem);
            }

            mealReport.TotalQuantityPerDays = CalculateTotalQuantityPerDay(rawMeals);
            mealReport.TotalQuantityOfMeal = mealReport.MealItems.Sum(x => x.NumberMealsMonth.TotalQuantity);
            mealReport.TotalAmountOfMeal = mealReport.MealItems.Sum(x => x.NumberMealsMonth.Amount);

            return mealReport;
        }

        private List<int> CalculateTotalQuantityPerDay(IEnumerable<dynamic> rawMeals)
        {
            var listTotalQty = new List<int>();
            listTotalQty.Add(rawMeals.Sum(m => m.D1));
            listTotalQty.Add(rawMeals.Sum(m => m.D2));
            listTotalQty.Add(rawMeals.Sum(m => m.D3));
            listTotalQty.Add(rawMeals.Sum(m => m.D4));
            listTotalQty.Add(rawMeals.Sum(m => m.D5));
            listTotalQty.Add(rawMeals.Sum(m => m.D6));
            listTotalQty.Add(rawMeals.Sum(m => m.D7));
            listTotalQty.Add(rawMeals.Sum(m => m.D8));
            listTotalQty.Add(rawMeals.Sum(m => m.D9));
            listTotalQty.Add(rawMeals.Sum(m => m.D10));
            listTotalQty.Add(rawMeals.Sum(m => m.D11));
            listTotalQty.Add(rawMeals.Sum(m => m.D12));
            listTotalQty.Add(rawMeals.Sum(m => m.D13));
            listTotalQty.Add(rawMeals.Sum(m => m.D14));
            listTotalQty.Add(rawMeals.Sum(m => m.D15));
            listTotalQty.Add(rawMeals.Sum(m => m.D16));
            listTotalQty.Add(rawMeals.Sum(m => m.D17));
            listTotalQty.Add(rawMeals.Sum(m => m.D18));
            listTotalQty.Add(rawMeals.Sum(m => m.D19));
            listTotalQty.Add(rawMeals.Sum(m => m.D20));
            listTotalQty.Add(rawMeals.Sum(m => m.D21));
            listTotalQty.Add(rawMeals.Sum(m => m.D22));
            listTotalQty.Add(rawMeals.Sum(m => m.D23));
            listTotalQty.Add(rawMeals.Sum(m => m.D24));
            listTotalQty.Add(rawMeals.Sum(m => m.D25));
            listTotalQty.Add(rawMeals.Sum(m => m.D26));
            listTotalQty.Add(rawMeals.Sum(m => m.D27));
            listTotalQty.Add(rawMeals.Sum(m => m.D28));
            listTotalQty.Add(rawMeals.Sum(m => m.D29 ?? 0));
            listTotalQty.Add(rawMeals.Sum(m => m.D30 ?? 0));
            listTotalQty.Add(rawMeals.Sum(m => m.D31 ?? 0));
            return listTotalQty;
        }

        private MealModel GetMeal(dynamic rawMeal)
        {
            var meal = new MealModel
            {
                MealId = rawMeal.MealID,
                MealEngName = rawMeal.MealEngName,
                MealVnmName = rawMeal.MealVNMName
            };

            return meal;
        }
        private MealItemMonthModel GetItemOfMeal(dynamic rawMeal)
        {
            var mealItem = new MealItemMonthModel
            {
                Employee = GetEmployeeOfMealItem(rawMeal),
                EmployeeType = GetEmloyeeTypeOfMealItem(rawMeal),
                Company = GetCompanyOfMealItem(rawMeal),
                Team = GetTeamOfMealItem(rawMeal),
                CostCenter = rawMeal.CostCenter ?? string.Empty,
                NumberMealsMonth = GetNumberMealsOfItem(rawMeal)
            };

            return mealItem;
        }

        private Employee GetEmployeeOfMealItem(dynamic rawMeal)
        {
            int? userId = rawMeal.SysID;

            if (userId.HasValue)
            {
                var employee = new Employee
                {
                    UserId = (int)userId,
                    FullName = rawMeal.FullName,
                    NickName = rawMeal.NickName,
                    Code = rawMeal.Code,
                    DomainUsername = rawMeal.DomainUsername
                };

                return employee;
            }

            return null;
        }

        private EmployeeTypeModel GetEmloyeeTypeOfMealItem(dynamic rawMeal)
        {
            int? employeeTypeId = rawMeal.EmployeeTypeID;

            if (employeeTypeId.HasValue)
            {
                var employeeType = new EmployeeTypeModel
                {
                    EmployeeTypeId = (int)employeeTypeId,
                    EmployeeTypeName = rawMeal.EmployeeTypeName
                };

                return employeeType;
            }

            return null;
        }

        private Company GetCompanyOfMealItem(dynamic rawMeal)
        {
            int? companyId = rawMeal.CompanyID;

            if (companyId.HasValue)
            {
                var company = new Company
                {
                    CompanyId = (short)companyId,
                    Name = rawMeal.CompanyName
                };

                return company;
            }

            return null;
        }
        private Team GetTeamOfMealItem(dynamic rawMeal)
        {
            int? teamId = rawMeal.EmpTeamID;

            if (teamId.HasValue)
            {
                var team = new Team
                {
                    TeamId = (int)teamId,
                    Code = rawMeal.EmpTeamCode,
                    Name = rawMeal.EmpTeamName
                };

                return team;
            }

            return null;
        }

        private MealDayOfMonthModel GetNumberMealsOfItem(dynamic rawMeal)
        {
            var mealDayOfMonth = new MealDayOfMonthModel();

            mealDayOfMonth.NumberMeals = GetNumberMealsInMonth(rawMeal);
            mealDayOfMonth.TotalQuantity = rawMeal.TotalQty != null ? rawMeal.TotalQty : 0;
            mealDayOfMonth.Amount = rawMeal.Amount != null ? rawMeal.Amount : 0;

            return mealDayOfMonth;
        }

        private MealDayOfMonthModel GetWeekDay(IEnumerable<dynamic> rawReport)
        {
            var weekDays = new MealDayOfMonthModel();

            dynamic rawWeekDay = rawReport.Where(x => x.MealID == null).First();
            weekDays.NumberMeals = GetNumberMealsInMonth(rawWeekDay);

            return weekDays;
        }

        private MealDayOfMonthModel CalculateTotalMeals(List<MealMonthModel> meals)
        {
            var total = new MealDayOfMonthModel();

            foreach (MealMonthModel meal in meals)
            {
                foreach (MealItemMonthModel mealItem in meal.MealItems)
                {
                    List<int> numberMeals = mealItem.NumberMealsMonth.NumberMeals;

                    for (int i = 0; i < numberMeals.Count(); i++)
                    {
                        if (total.NumberMeals.Count < numberMeals.Count())
                        {
                            total.NumberMeals.Add(numberMeals[i]);
                        }
                        else
                        {
                            total.NumberMeals[i] += numberMeals[i];
                        }
                    }

                    total.TotalQuantity += mealItem.NumberMealsMonth.TotalQuantity;
                    total.Amount += mealItem.NumberMealsMonth.Amount;
                }
            }

            return total;
        }

        private List<int> GetNumberMealsInMonth(dynamic rawWeekDay)
        {
            var numberMeals = new List<int>();

            numberMeals = GetNumberMealsFirstWeek(rawWeekDay, numberMeals);
            numberMeals = GetNumberMealsSecondWeek(rawWeekDay, numberMeals);
            numberMeals = GetNumberMealsThridWeek(rawWeekDay, numberMeals);
            numberMeals = GetNumberMealsFourthWeek(rawWeekDay, numberMeals);
            numberMeals = GetNumberMealsFifthWeek(rawWeekDay, numberMeals);

            return numberMeals;
        }

        private List<int> GetNumberMealsFirstWeek(dynamic rawWeekDay, List<int> numberMeals)
        {
            numberMeals.Add(rawWeekDay.D1);
            numberMeals.Add(rawWeekDay.D2);
            numberMeals.Add(rawWeekDay.D3);
            numberMeals.Add(rawWeekDay.D4);
            numberMeals.Add(rawWeekDay.D5);
            numberMeals.Add(rawWeekDay.D6);
            numberMeals.Add(rawWeekDay.D7);

            return numberMeals;
        }

        private List<int> GetNumberMealsSecondWeek(dynamic rawWeekDay, List<int> numberMeals)
        {
            numberMeals.Add(rawWeekDay.D8);
            numberMeals.Add(rawWeekDay.D9);
            numberMeals.Add(rawWeekDay.D10);
            numberMeals.Add(rawWeekDay.D11);
            numberMeals.Add(rawWeekDay.D12);
            numberMeals.Add(rawWeekDay.D13);
            numberMeals.Add(rawWeekDay.D14);

            return numberMeals;
        }
        private List<int> GetNumberMealsThridWeek(dynamic rawWeekDay, List<int> numberMeals)
        {
            numberMeals.Add(rawWeekDay.D15);
            numberMeals.Add(rawWeekDay.D16);
            numberMeals.Add(rawWeekDay.D17);
            numberMeals.Add(rawWeekDay.D18);
            numberMeals.Add(rawWeekDay.D19);
            numberMeals.Add(rawWeekDay.D20);
            numberMeals.Add(rawWeekDay.D21);

            return numberMeals;
        }

        private List<int> GetNumberMealsFourthWeek(dynamic rawWeekDay, List<int> numberMeals)
        {
            numberMeals.Add(rawWeekDay.D22);
            numberMeals.Add(rawWeekDay.D23);
            numberMeals.Add(rawWeekDay.D24);
            numberMeals.Add(rawWeekDay.D25);
            numberMeals.Add(rawWeekDay.D26);
            numberMeals.Add(rawWeekDay.D27);
            numberMeals.Add(rawWeekDay.D28);

            return numberMeals;
        }

        private List<int> GetNumberMealsFifthWeek(dynamic rawWeekDay, List<int> numberMeals)
        {
            int? day29 = rawWeekDay.D29;

            if (day29.HasValue)
            {
                numberMeals.Add((int)day29);
            }
            else
            {
                numberMeals.Add(0);
            }

            int? day30 = rawWeekDay.D30;

            if (day30.HasValue)
            {
                numberMeals.Add((int)day30);
            }
            else
            {
                numberMeals.Add(0);
            }

            int? day31 = rawWeekDay.D31;

            if (day31.HasValue)
            {
                numberMeals.Add((int)day31);
            }
            else
            {
                numberMeals.Add(0);
            }

            return numberMeals;
        }
    }
}