// ----------------------------------------------------------------------
// <copyright file="MealRegistrationController.cs" company="Nexcel Solutions Viet Nam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Josh.Phan" date="8/22/2014 2:42:38 PM">Created.</change>
// </history>

using System.Globalization;
using System.Xml;

namespace NexFB.Site.Controllers.API
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using NexFB.Core.Constants;
    using NexFB.Core.Enums;
    using NexFB.Core.Models;
    using NexFB.Criteria.MealRegistration;
    using NexFB.Models.Authority;
    using NexFB.Models.MealRegistration;
    using NexFB.Services.Implement;
    using NexFB.Services.Interface;
    using NexFB.Web.Attributes;
    using NexFB.Web.Services.Implement;
    using NexFB.Web.Services.Interface;

    /// <summary>
    /// Registration Meal controller.
    /// </summary>
    public class MealRegistrationController : BaseController
    {
        private readonly IMealRegistrationService _mealRegistrationService;
        private readonly IWebCommonService _webCommonService;
        private const string IsVeg = "IsVeg";

        public MealRegistrationController()
            : this(null, null)
        {
        }

        public MealRegistrationController(
            IMealRegistrationService mealRegistrationService,
            IWebCommonService webCommonService)
        {
            _mealRegistrationService = mealRegistrationService ?? new MealRegistrationService();
            _webCommonService = webCommonService ?? new WebCommonService();
        }

        /// <summary>
        /// Get register meal by range date.
        /// </summary>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">The to date.</param>
        /// <param name="mealId">The meal Id.</param>
        /// <returns>The list meal date model.</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Read)]
        public IList<MealRegisterModel> GetRegisterMealByRangeDate(
            DateTime fromDate,
            DateTime toDate,
            short? mealId = null)
        {
            var criteria = new GetRegisterMealByRangeDateCriteria
            {
                SystemId = this.CurrentUser.UserId,
                MealId = mealId,
                FromDate = fromDate,
                ToDate = toDate
            };

            return _mealRegistrationService.GetRegisterMealByRangeDate(criteria);
        }

        /// <summary>
        /// Get Employee Register Food By Range Date.
        /// </summary>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">The to date.</param>
        /// <param name="mealId">The meal Id.</param>
        /// <returns>The list food employee register</returns>
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Read)]
        public IEnumerable<EmployeeRegisterFood> GetEmployeeRegisterFoodByRangeDate(
            DateTime fromDate,
            DateTime toDate,
            short? mealId = null)
        {
            var criteria = new GetEmployeeRegisterFoodByRangeDateCriteria
            {
                SystemId = this.CurrentUser.UserId,
                MealId = mealId,
                FromDate = fromDate,
                ToDate = toDate
            };

            return _mealRegistrationService.GetEmployeeRegisterFoodByRangeDate(criteria);
        }

        /// <summary>
        /// Get Vegetarian Management By Range Date.
        /// </summary>
        /// <param name="fromDate">The from date.</param>
        /// <param name="toDate">The to date.</param>
        /// <param name="mealId">The meal Id.</param>
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Read)]
        public IEnumerable<VegetarianManagement> GetVegetarianManagementByRangeDate(
            DateTime fromDate,
            DateTime toDate,
            short? mealId = null)
        {
            var criteria = new GetVegetarianManagementByRangeDateCriteria
            {
                MealId = mealId,
                FromDate = fromDate,
                ToDate = toDate
            };

            return _mealRegistrationService.GetVegetarianManagementByRangeDate(criteria);
        }

        /// <summary>
        /// Register meal by range date.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Host)]
        public CommandResult RegisterMealByRangeDate(RegisterMealByRangeDateCriteria criteria)
        {
            bool isValidRegister = true;

            bool isValidDate = DateTime.TryParse(criteria.OnDate, out var onDate);
            if (isValidDate)
            {
                var isAllowRegisterVegetarian = GetVegetarianManagementByRangeDate(onDate, onDate).ToList<VegetarianManagement>();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(criteria.Meal);
                XmlNodeList mealList = xml.GetElementsByTagName("Meal");

                for (int i = 0; i < 4; i++)
                {
                    if (!CheckRegisterMeal(mealList, isAllowRegisterVegetarian, i))
                    {
                        isValidRegister = false;
                    }
                }
            }

            if (isValidRegister)
            {
                criteria.SystemId = this.CurrentUser.UserId;
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _mealRegistrationService.RegisterMealByRangeDate(criteria);
        }

        private static bool CheckRegisterMeal(XmlNodeList mealList, List<VegetarianManagement> isAllowRegisterVegetarian, int index)
        {
            if (mealList[index].Attributes?[IsVeg]?.Value == "1")
            {
                if (isAllowRegisterVegetarian.Count == 0)
                {
                    return false;
                }
                else if ((bool)!isAllowRegisterVegetarian[index].IsAllowVegetarian)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        /// <summary>
        /// Set Vegetarian Management By Single Date
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Host)]
        public CommandResult SetVegetarianManagementBySingleDate(SetVegetarianManagementBySingleDateCriteria criteria)
        {
            criteria.ModifiedBy = this.CurrentUser.UserId;
            criteria.ModifiedDate = DateTime.Now;

            return _mealRegistrationService.SetVegetarianManagementBySingleDate(criteria);
        }

        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Read)]
        public IList<MealDateModel> GetDefaultRegisterMeal()
        {
            var criteria = new GetRegisterMealByDefaultCriteria
            {
                SystemId = this.CurrentUser.UserId
            };

            return _mealRegistrationService.GetDefaultRegisterMeal(criteria);
        }

        [HttpPost]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Host)]
        public CommandResult RegisterMealByDefault(SetRegisterMealByDefaultCriteria criteria)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(criteria.Meal);
            XmlNodeList mealList = xml.GetElementsByTagName("Meal");

            bool isVeg = false;
            for (int i = 0; i < 4; i++)
            {
                if (mealList[i].Attributes?[IsVeg]?.Value == "1")
                {
                    isVeg = true;
                    break;
                }
            }

            if (isVeg)
                return _mealRegistrationService.RegisterMealByDefault(criteria);

            if (criteria != null)
            {
                criteria.UserId = this.CurrentUser.UserId;
                criteria.ModifiedBy = this.CurrentUser.UserId;
                criteria.ModifiedDate = DateTime.Now;
            }

            return _mealRegistrationService.RegisterMealByDefault(criteria);
        }

        [HttpPut]
        [ApiAuthorize(FunctionKey = FBFunctionKey.Registration, Policy = Policy.Host)]
        public CommandResult ApplyRegisterMealFromTemplate()
        {
            var criteria = new ApplyRegisterMealFromTemplateCriteria
            {
                UserId = this.CurrentUser.UserId,
                ModifiedBy = this.CurrentUser.UserId,
                ModifiedDate = DateTime.Now
            };

            return _mealRegistrationService.ApplyRegisterMealFromTemplate(criteria);
        }

        public int GetUserPermissionOnRegistration()
        {
            int policy = _webCommonService.GetPermission(FBFunctionKey.Registration);
            return policy;
        }

        public RegisterMealSetting GetRegisterMealSetting()
        {
            var criteria = new GetRegisterMealSettingCriteria();
            RegisterMealSetting setting = _mealRegistrationService.GetRegisterMealSetting(criteria);

            return setting;
        }

        public UserSession GetCurrentUser()
        {
            return this.CurrentUser;
        }

        public double GetJuliusFromDate(int dd, int mm, int yy)
        {
            double JD;
            if (yy > 1582 || (yy == 1582 && mm > 10) || (yy == 1582 && mm == 10 && dd > 14))
            {
                JD = 367 * yy - (int)(7 * (yy + (int)((mm + 9) / 12)) / 4) - (int)(3 * ((int)((yy + (mm - 9) / 7) / 100) + 1) / 4) + (int)(275 * mm / 9) + dd + 1721028.5;
            }
            else
            {
                JD = 367 * yy - (int)(7 * (yy + 5001 + (int)((mm - 9) / 7)) / 4) + (int)(275 * mm / 9) + dd + 1729776.5;
            }
            return JD;
        }

        public int GetNewMoonDay(int k, double timeZone)
        {
            const double PI = 3.14159265358979323846;
            double deltat, JdNew;
            double T = k / 1236.85; // Time in Julian centuries from 1900 January 0.5
            double T2 = T * T;
            double T3 = T2 * T;
            double dr = PI / 180;
            double Jd1 = 2415020.75933 + 29.53058868 * k + 0.0001178 * T2 - 0.000000155 * T3;
            Jd1 += 0.00033 * Math.Sin((166.56 + 132.87 * T - 0.009173 * T2) * dr); // Mean new moon
            double M = 359.2242 + 29.10535608 * k - 0.0000333 * T2 - 0.00000347 * T3; // Sun's mean anomaly
            double Mpr = 306.0253 + 385.81691806 * k + 0.0107306 * T2 + 0.00001236 * T3; // Moon's mean anomaly
            double F = 21.2964 + 390.67050646 * k - 0.0016528 * T2 - 0.00000239 * T3; // Moon's argument of latitude
            double C1 = (0.1734 - 0.000393 * T) * Math.Sin(M * dr) + 0.0021 * Math.Sin(2 * dr * M);
            C1 = C1 - 0.4068 * Math.Sin(Mpr * dr) + 0.0161 * Math.Sin(dr * 2 * Mpr);
            C1 = C1 - 0.0004 * Math.Sin(dr * 3 * Mpr);
            C1 = C1 + 0.0104 * Math.Sin(dr * 2 * F) - 0.0051 * Math.Sin(dr * (M + Mpr));
            C1 = C1 - 0.0074 * Math.Sin(dr * (M - Mpr)) + 0.0004 * Math.Sin(dr * (2 * F + M));
            C1 = C1 - 0.0004 * Math.Sin(dr * (2 * F - M)) - 0.0006 * Math.Sin(dr * (2 * F + Mpr));
            C1 = C1 + 0.0010 * Math.Sin(dr * (2 * F - Mpr)) + 0.0005 * Math.Sin(dr * (2 * Mpr + M));
            if (T < -11)
            {
                deltat = 0.001 + 0.000839 * T + 0.0002261 * T2 - 0.00000845 * T3 - 0.000000081 * T * T3;
            }
            else
            {
                deltat = -0.000278 + 0.000265 * T + 0.000262 * T2;
            };
            JdNew = Jd1 + C1 - deltat;
            return (int)(JdNew);
        }

        public double GetSunLongitude(double jdn, double timeZone)
        {
            const double PI = 3.14159265358979323846;
            double T, T2, dr, M, L0, DL, L;
            T = (jdn - 2451545.5 - timeZone / 24) / 36525; // Time in Julian centuries from 2000-01-01 12:00:00 GMT
            T2 = T * T;
            dr = PI / 180; // degree to radian
            M = 357.52910 + 35999.05030 * T - 0.0001559 * T2 - 0.00000048 * T * T2; // mean anomaly, degree
            L0 = 280.46645 + 36000.76983 * T + 0.0003032 * T2; // mean longitude, degree
            DL = (1.914600 - 0.004817 * T - 0.000014 * T2) * Math.Sin(dr * M);
            DL = DL + (0.019993 - 0.000101 * T) * Math.Sin(dr * 2 * M) + 0.000290 * Math.Sin(dr * 3 * M);
            L = L0 + DL; // true longitude, degree
            L = L * dr;
            L = L - PI * 2 * ((int)(L / (PI * 2))); // Normalize to (0, 2*PI)
            return (int)(L / PI * 6);
        }

        public double GetLunarMonth11(int yy, double timeZone)
        {
            double off = GetJuliusFromDate(31, 12, yy) - 2415021;
            int k = (int)(off / 29.530588853);
            int nm = GetNewMoonDay(k, timeZone);
            double sunLong = GetSunLongitude(nm, timeZone); // sun longitude at local midnight
            if (sunLong >= 9)
            {
                nm = GetNewMoonDay(k - 1, timeZone);
            }
            return nm;
        }

        public double GetLeapMonthOffset(double a11, double timeZone)
        {
            int i;
            int k = (int)((a11 - 2415021.076998695) / 29.530588853 + 0.5);
            double last = 0;
            i = 1; // We start with the month following lunar month 11
            double arc = GetSunLongitude(GetNewMoonDay(k + i, timeZone), timeZone);
            do
            {
                last = arc;
                i++;
                arc = GetSunLongitude(GetNewMoonDay(k + i, timeZone), timeZone);
            } while (arc != last && i < 14);
            return i - 1;
        }

        public bool CheckVegetarianDay(int dd, int mm, int yy, double timeZone)
        {
            const int middleLunarMonth = 15;
            double leapMonthDiff;
            int lunarYear, lunarLeap;
            double dayNumber = GetJuliusFromDate(dd, mm, yy);
            int k = (int)((dayNumber - 2415021.076998695) / 29.530588853);
            int monthStart = GetNewMoonDay(k + 1, timeZone);
            if (monthStart > dayNumber)
            {
                monthStart = GetNewMoonDay(k, timeZone);
            }
            double a11 = GetLunarMonth11(yy, timeZone);
            double b11 = a11;
            if (a11 >= monthStart)
            {
                lunarYear = yy;
                a11 = GetLunarMonth11(yy - 1, timeZone);
            }
            else
            {
                lunarYear = yy + 1;
                b11 = GetLunarMonth11(yy + 1, timeZone);
            }
            double lunarDay = dayNumber - monthStart + 1;
            double diff = (int)((monthStart - a11) / 29);
            lunarLeap = 0;
            double lunarMonth = diff + 11;
            if (b11 - a11 > 365)
            {
                leapMonthDiff = GetLeapMonthOffset(a11, timeZone);
                if (diff >= leapMonthDiff)
                {
                    lunarMonth = diff + 10;
                    if (diff == leapMonthDiff)
                    {
                        lunarLeap = 1;
                    }
                }
            }
            if (lunarMonth > 12)
            {
                lunarMonth = lunarMonth - 12;
            }
            if (lunarMonth >= 11 && diff < 4)
            {
                lunarYear -= 1;
            }
            return ((int)(lunarDay) == 1 || (int)(lunarDay) == middleLunarMonth);
        }

        public int GetLunarMonth(int dd, int mm, int yy, double timeZone)
        {
            const int middleLunarMonth = 15;
            double leapMonthDiff;
            int lunarYear, lunarLeap;
            double dayNumber = GetJuliusFromDate(dd, mm, yy);
            int k = (int)((dayNumber - 2415021.076998695) / 29.530588853);
            int monthStart = GetNewMoonDay(k + 1, timeZone);
            if (monthStart > dayNumber)
            {
                monthStart = GetNewMoonDay(k, timeZone);
            }
            double a11 = GetLunarMonth11(yy, timeZone);
            double b11 = a11;
            if (a11 >= monthStart)
            {
                lunarYear = yy;
                a11 = GetLunarMonth11(yy - 1, timeZone);
            }
            else
            {
                lunarYear = yy + 1;
                b11 = GetLunarMonth11(yy + 1, timeZone);
            }
            double lunarDay = dayNumber - monthStart + 1;
            double diff = (int)((monthStart - a11) / 29);
            lunarLeap = 0;
            double lunarMonth = diff + 11;
            if (b11 - a11 > 365)
            {
                leapMonthDiff = GetLeapMonthOffset(a11, timeZone);
                if (diff >= leapMonthDiff)
                {
                    lunarMonth = diff + 10;
                    if (diff == leapMonthDiff)
                    {
                        lunarLeap = 1;
                    }
                }
            }
            if (lunarMonth > 12)
            {
                lunarMonth = lunarMonth - 12;
            }
            if (lunarMonth >= 11 && diff < 4)
            {
                lunarYear -= 1;
            }
            return (int)(lunarMonth);
        }
    }
}