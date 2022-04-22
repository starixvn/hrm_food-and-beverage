'use strict';
/*global mealRegistrationModule*/
/*jshint maxdepth:6 */

mealRegistrationModule.factory('mealRegistrationFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
    return {
        getRegisterMealByRangeDate: function (fromDate, toDate, mealId) {
            return $http.get(baseUrl + 'api/MealRegistration/GetRegisterMealByRangeDate',
                { params: { fromDate: fromDate, toDate: toDate, mealId: mealId } });
        },
        registerMealByRangeDate: function (mealRegistration) {
            return $http.post(baseUrl + 'api/MealRegistration/RegisterMealByRangeDate', mealRegistration);
        },
        getAllMealTimesheet: function () {
            return $http.get(baseUrl + 'api/Common/GetAllMealTimesheet');
        },
        getDefaultRegisterMeal: function () {
            return $http.get(baseUrl + 'api/MealRegistration/GetDefaultRegisterMeal');
        },
        setDefaultRegisterMeal: function (mealRegistration) {
            return $http.post(baseUrl + 'api/MealRegistration/RegisterMealByDefault', mealRegistration);
        },
        applyFromTemplate: function () {
            return $http.put(baseUrl + 'api/MealRegistration/ApplyRegisterMealFromTemplate');
        },
        getUserPermissionOnRegistration: function () {
            return $http.get(baseUrl + 'api/MealRegistration/GetUserPermissionOnRegistration');
        },
        getRegisterMealSetting: function () {
            return $http.get(baseUrl + 'api/MealRegistration/GetRegisterMealSetting');
        },
        getCurrentUser: function () {
            return $http.get(baseUrl + 'api/MealRegistration/GetCurrentUser');
        }
    };
}]);

mealRegistrationModule.factory('mealRegistrationCacheFactory', ['$cacheFactory', function ($cacheFactory) {
    return $cacheFactory('mealRegistrationCache');
}]);

mealRegistrationModule.factory("mealRegistrationUtilities",
    ['$translate', 'mealRegistrationFactory', 'mealRegistrationCacheFactory',
        function ($translate, mealRegistrationFactory, mealRegistrationCacheFactory) {
            var utility = {};

            utility.getTabHeading = function (scope) {
                var monthlyHeadingCache = mealRegistrationCacheFactory.get("monthlyHeading");

                if (monthlyHeadingCache) {
                    scope.monthlyHeading = monthlyHeadingCache;
                }
                else {
                    $translate("Monthly").then(function (message) {
                        scope.monthlyHeading = message;
                        mealRegistrationCacheFactory.put("monthlyHeading", message);
                    });
                }

                var defaultHeadingCache = mealRegistrationCacheFactory.get("defaultHeading");

                if (defaultHeadingCache) {
                    scope.defaultHeading = defaultHeadingCache;
                }
                else {
                    $translate("Default").then(function (message) {
                        scope.defaultHeading = message;
                        mealRegistrationCacheFactory.put("defaultHeading", message);
                    });
                }
            };

            utility.getRegisterDateInWeek = function (week) {
                var onDate = '';

                angular.forEach(week.WeekMeals, function (date) {
                    if (!date.IsDisabled) {
                        var currentDate = new Date(date.WorkDate);

                        // Don't effect Sunday and Saturday
                        if (currentDate.getDay() !== 0 && currentDate.getDay() !== 6) {
                            var workDate = !date.IsDisabled ? date.WorkDate.toString() : '';
                            onDate = onDate ? onDate + ',' + workDate : workDate;
                        }
                    }
                });

                return onDate;
            };

            utility.getRegisterDateInMonth = function (weeks) {
                var onDate = '';

                angular.forEach(weeks, function (week) {
                    var registerDateInWeek = utility.getRegisterDateInWeek(week);
                    onDate = onDate ? onDate + ',' + registerDateInWeek : registerDateInWeek;
                });

                return onDate;
            };

            utility.getCurrentTimesheet = function (mealTimesheet, mealId) {
                var currentTimesheet = '';

                angular.forEach(mealTimesheet, function (timesheetItem) {
                    if (timesheetItem.MealId === mealId) {
                        currentTimesheet = timesheetItem;
                    }
                });

                return currentTimesheet;
            };

            utility.checkRightMeal = function (currentTimesheet) {
                var toDay = new Date(),
                    currentHour = toDay.getHours(),
                    currentMinute = toDay.getMinutes(),
                    startHour = new Date(currentTimesheet.StartTime + 'Z').getUTCHours(),
                    startMinute = new Date(currentTimesheet.StartTime + 'Z').getUTCMinutes();

                if (currentHour > startHour) {
                    return false;
                }
                else if (currentHour === startHour && currentMinute > startMinute) {
                    return false;
                }

                return true;
            };

            var updateMealForToDay = function (date, meals, mealTimesheet) {
                var isCurrentVegetarian = false;

                for (var j = 0; j < date.Meals.length; j++) {
                    if (date.Meals[j].IsVegetarian) {
                        isCurrentVegetarian = true;
                        j = date.Meals.length;
                    }
                }

                if (!isCurrentVegetarian) {
                    for (var k = 0; k < date.Meals.length; k++) {
                        var meal = date.Meals[k],
                            updateMeal = meals[k];

                        if (!updateMeal.IsVegetarian) {
                            var currentTimesheet = utility.getCurrentTimesheet(mealTimesheet, meal.MealId),
                                rightMeal = utility.checkRightMeal(currentTimesheet);

                            date.Meals[k].IsVegetarian = rightMeal ? updateMeal.IsVegetarian : meal.IsVegetarian;
                        }
                    }
                }

                return date;
            };

            utility.getWeekDayIndex = function (date) {
                var weekDay = '';

                switch (date) {
                    case 'Sun':
                        weekDay = 0;
                        break;
                    case 'Mon':
                        weekDay = 1;
                        break;
                    case 'Tue':
                        weekDay = 2;
                        break;
                    case 'Wed':
                        weekDay = 3;
                        break;
                    case 'Thu':
                        weekDay = 4;
                        break;
                    case 'Fri':
                        weekDay = 5;
                        break;
                    case 'Sat':
                        weekDay = 6;
                        break;
                }

                return weekDay;
            };

            utility.getFullNameWeekday = function (date) {
                var fullName = '';

                //TODO : get text from resources.
                switch (date) {
                    case 'Sun':
                        fullName = 'Every Sunday';
                        break;
                    case 'Mon':
                        fullName = 'Every Monday';
                        break;
                    case 'Tue':
                        fullName = 'Every Tuesday';
                        break;
                    case 'Wed':
                        fullName = 'Every Wednesday';
                        break;
                    case 'Thu':
                        fullName = 'Every Thursday';
                        break;
                    case 'Fri':
                        fullName = 'Every Friday';
                        break;
                    case 'Sat':
                        fullName = 'Every Saturday';
                        break;
                }

                return fullName;
            };

            utility.getFullNameWeekdayFromIndex = function (weekDayIndex) {
                var fullName = '';

                //TODO : get text from resources.
                switch (weekDayIndex) {
                    case 0:
                        fullName = 'Sundays';
                        break;
                    case 1:
                        fullName = 'Mondays';
                        break;
                    case 2:
                        fullName = 'Tuesdays';
                        break;
                    case 3:
                        fullName = 'Wednesdays';
                        break;
                    case 4:
                        fullName = 'Thursdays';
                        break;
                    case 5:
                        fullName = 'Fridays';
                        break;
                    case 6:
                        fullName = 'Saturdays';
                        break;
                }

                return fullName;
            };

            utility.getRegisterDateOfWeek = function (weekDayIndex, weeks) {
                var onDate = '';

                angular.forEach(weeks, function (week) {
                    var date = week.WeekMeals[weekDayIndex];

                    if (!date.IsDisabled) {
                        var workDate = date.WorkDate.toString();
                        onDate = onDate ? onDate + ',' + workDate : workDate;
                    }
                });

                return onDate;
            };

            utility.getDefaultDateInWeek = function (week) {
                var onDate = '';

                angular.forEach(week, function (date) {
                    var weekDay = date.WeekDay;

                    if (weekDay !== 1 && weekDay !== 7) {
                        if (!onDate) {
                            onDate = date.WeekDay;
                        }
                        else {
                            onDate += ',' + date.WeekDay;
                        }
                    }
                });

                return onDate;
            };

            utility.hasDateEnableInWeek = function (week) {
                var hasEnableDate = false;

                angular.forEach(week.WeekMeals, function (date) {
                    var toDay = new Date(),
                        currentDate = new Date(date.WorkDate);

                    // Don't check Sunday and Saturday
                    if (currentDate.getDay() !== 0 && currentDate.getDay() !== 6) {
                        if (!date.IsDisabled) {
                            if (toDay.getMonth() === currentDate.getMonth() && toDay.getDate() === currentDate.getDate() &&
                                toDay.getFullYear() === currentDate.getFullYear()) {
                                hasEnableDate = false;
                            }
                            else {
                                hasEnableDate = true;
                            }
                        }
                    }
                });

                return hasEnableDate;
            };

            utility.hasDateEnabled = function (date, weeks) {
                var hasEnableDate = false,
                    weekDayIndex = utility.getWeekDayIndex(date),
                    toDay = new Date();

                angular.forEach(weeks, function (week) {
                    var date = week.WeekMeals[weekDayIndex],
                        currentDate = new Date(date.WorkDate);

                    if (!date.IsDisabled) {
                        if (toDay.getMonth() === currentDate.getMonth() && toDay.getDate() === currentDate.getDate() &&
                            toDay.getFullYear() === currentDate.getFullYear()) {
                            hasEnableDate = false;
                        }
                        else {
                            hasEnableDate = true;
                        }
                    }
                });

                return hasEnableDate;
            };

            utility.getDateNotToday = function (week) {
                var allDateInWeek = week.WeekMeals,
                    toDay = new Date(),
                    resultDate = '';

                for (var i = 0; i < allDateInWeek.length; i++) {
                    var date = allDateInWeek[i],
                        currentDate = new Date(date.WorkDate);

                    if (!date.IsDisabled) {
                        if (currentDate.getDay() !== 0 && currentDate.getDay() !== 6) {
                            if (toDay.getFullYear() !== currentDate.getFullYear() || toDay.getMonth() !== currentDate.getMonth() ||
                                toDay.getDate() !== currentDate.getDate()) {
                                resultDate = date;
                                i = allDateInWeek.length;
                            }
                        }
                    }
                }

                return resultDate;
            };

            utility.getDateIsToday = function (week) {
                var allDateInWeek = week.WeekMeals,
                    toDay = new Date(),
                    resultDate = '';

                for (var i = 0; i < allDateInWeek.length; i++) {
                    var date = allDateInWeek[i],
                        currentDate = new Date(date.WorkDate);

                    if (!date.IsDisabled) {
                        if (currentDate.getDay() !== 0 && currentDate.getDay() !== 6) {
                            if (toDay.getFullYear() === currentDate.getFullYear() || toDay.getMonth() === currentDate.getMonth() &&
                                toDay.getDate() === currentDate.getDate()) {
                                resultDate = date;
                                i = allDateInWeek.length;
                            }
                        }
                    }
                }

                return resultDate;
            };

            var checkSameMeal = function (firstDate, secondDate) {
                var isSameMeal = true;

                for (var i = 0; i < 4; i++) {
                    if (firstDate.Meals[i].IsVegetarian !== secondDate.Meals[i].IsVegetarian) {
                        isSameMeal = false;
                    }
                }

                return isSameMeal;
            };

            var checkSameMealWithToday = function (sealedDate, date, mealTimesheet) {
                var isSameMeal = true;

                angular.forEach(date.Meals, function (meal, index) {
                    var currentTimesheet = utility.getCurrentTimesheet(mealTimesheet, meal.MealId),
                        rightMeal = utility.checkRightMeal(currentTimesheet);

                    if (rightMeal) {
                        if (sealedDate.Meals[index].IsVegetarian !== meal.IsVegetarian) {
                            isSameMeal = false;
                        }
                    }
                });

                return isSameMeal;
            };

            utility.checkSameMealInWeek = function (week, mealTimesheet, sealedDate) {
                var isSameMeal = true,
                    length = week.WeekMeals.length,
                    toDay = new Date();

                if (sealedDate) {
                    // Don't check Sunday and Saturday
                    for (var i = 1; i < length - 1; i++) {
                        var date = week.WeekMeals[i],
                            currentDate = new Date(date.WorkDate),
                            checked = false;

                        if (!date.IsDisabled) {
                            if (toDay.getMonth() === currentDate.getMonth() && toDay.getDate() === currentDate.getDate() &&
                                toDay.getFullYear() === currentDate.getFullYear()) {
                                checked = checkSameMealWithToday(sealedDate, date, mealTimesheet);
                            }
                            else {
                                checked = checkSameMeal(sealedDate, date);
                            }

                            if (!checked) {
                                isSameMeal = false;
                            }
                        }
                    }
                }
                else {
                    // Today is the only enable day in current week
                    isSameMeal = true;
                }

                return isSameMeal;
            };

            utility.getDateNotTodayInMonth = function (weeks) {
                var resultDate = '';

                for (var i = 0; i < weeks.length; i++) {
                    var week = weeks[i];
                    resultDate = utility.getDateNotToday(week);

                    if (resultDate) {
                        i = weeks.length;
                    }
                }

                return resultDate;
            };

            utility.getDateIsTodayInMonth = function (weeks) {
                var resultDate = '';

                for (var i = 0; i < weeks.length; i++) {
                    var week = weeks[i];
                    resultDate = utility.getDateIsToday(week);

                    if (resultDate) {
                        i = weeks.length;
                    }
                }

                return resultDate;
            };

            utility.checkSameMealInMonth = function (weeks, mealTimesheet) {
                var isSameMeal = true,
                    sealedDate = '';

                angular.forEach(weeks, function (week, index) {
                    if (index === 0 || !sealedDate) {
                        sealedDate = utility.getDateNotToday(week);
                    }

                    var checked = utility.checkSameMealInWeek(week, mealTimesheet, sealedDate);

                    if (!checked) {
                        isSameMeal = false;
                    }
                    else if (!sealedDate) {
                        sealedDate = utility.getDateIsToday(week);
                    }
                });

                return isSameMeal;
            };

            utility.getWeekDayNotTodayInMonth = function (weeks, weekDayIndex) {
                var resultDate = '',
                    toDay = new Date();

                for (var i = 0; i < weeks.length; i++) {
                    var week = weeks[i],
                        date = week.WeekMeals[weekDayIndex],
                        currentDate = new Date(date.WorkDate);

                    if (!date.IsDisabled) {
                        if (toDay.getFullYear() !== currentDate.getFullYear() || toDay.getMonth() !== currentDate.getMonth() ||
                            toDay.getDate() !== currentDate.getDate()) {
                            resultDate = date;
                            i = weeks.length;
                        }
                    }
                }

                return resultDate;
            };

            utility.checkSameMealWeekDayInMonth = function (weeks, weekDayIndex, mealTimesheet) {
                var isSameMeal = true,
                    toDay = new Date(),
                    sealedDate = utility.getWeekDayNotTodayInMonth(weeks, weekDayIndex);

                if (sealedDate) {
                    for (var i = 0; i < weeks.length; i++) {
                        var week = weeks[i],
                            date = week.WeekMeals[weekDayIndex],
                            currentDate = new Date(date.WorkDate),
                            checked = false;

                        if (!date.IsDisabled) {
                            if (toDay.getMonth() === currentDate.getMonth() && toDay.getDate() === currentDate.getDate() &&
                                toDay.getFullYear() === currentDate.getFullYear()) {
                                checked = checkSameMealWithToday(sealedDate, date, mealTimesheet);
                            }
                            else {
                                checked = checkSameMeal(sealedDate, date);
                            }

                            if (!checked) {
                                isSameMeal = false;
                            }
                        }
                    }
                }
                else {
                    isSameMeal = true;
                }

                return isSameMeal;
            };

            utility.checkSameMealInWeekDefault = function (week) {
                var isSameMeal = true,
                    sealedDate = week[1]; // Monday

                // don't check Sunday and Saturday
                for (var i = 2; i < week.length - 1; i++) {
                    var date = week[i],
                        checked = false;

                    checked = checkSameMeal(sealedDate, date);

                    if (!checked) {
                        isSameMeal = false;
                    }
                }

                return isSameMeal;
            };

            utility.getWeekDayIsToday = function (weeks, weekDayIndex) {
                var resultDate = '',
                    toDay = new Date(),
                    weekLength = weeks.length;

                for (var i = 0; i < weekLength; i++) {
                    var week = weeks[i],
                        date = week.WeekMeals[weekDayIndex],
                        currentDate = new Date(date.WorkDate);

                    if (!date.IsDisabled) {
                        if (toDay.getMonth() === currentDate.getMonth() && toDay.getDate() === currentDate.getDate() &&
                            toDay.getFullYear() === currentDate.getFullYear()) {
                            resultDate = date;
                            i = weekLength;
                        }
                    }
                }

                return resultDate;
            };

            utility.updateMealForDate = function (date, meals, mealTimesheet, registerSetting, isWeekday) {
                var hasDateEffectedVegetarian = false;

                if (!date.IsDisabled) {
                    var toDay = new Date(),
                        sevenDay = new Date().setDate(toDay.getDate() + registerSetting.Order),
                        nextSevenDay = new Date(sevenDay),
                        currentDate = new Date(date.WorkDate),
                        isUpdate = false;

                    toDay.setHours(0, 0, 0, 0);
                    nextSevenDay.setHours(0, 0, 0, 0);
                    currentDate.setHours(0, 0, 0, 0);

                    if (!isWeekday) {
                        // Don't update Sunday and Saturday
                        if (currentDate.getDay() !== 0 && currentDate.getDay() !== 6) {
                            isUpdate = true;
                        }
                    }
                    else {
                        isUpdate = true;
                    }

                    if (isUpdate) {
                        if (toDay.getTime() === currentDate.getTime()) {
                            date = updateMealForToDay(date, meals, mealTimesheet);
                        }
                        else if (currentDate > toDay && currentDate < nextSevenDay) {
                            for (var i = 0; i < meals.length; i++) {
                                var updateMeal = meals[i];

                                if (!updateMeal.IsVegetarian) {
                                    date.Meals[i].IsVegetarian = updateMeal.IsVegetarian;
                                }
                            }
                        }
                        else {
                            angular.forEach(date.Meals, function (meal, index) {
                                meal.IsVegetarian = meals[index].IsVegetarian;

                                if (meal.IsVegetarian) {
                                    hasDateEffectedVegetarian = true;
                                }
                            });
                        }
                    }
                }

                return hasDateEffectedVegetarian;
            };

            utility.resetDisableMeal = function (meals) {
                angular.forEach(meals, function (meal) {
                    meal.IsDisableVegetarian = false;
                    meal.IsDisableNonVegetarian = false;
                });

                return meals;
            };

            utility.checkNonRegisterVegetarianDate = function (date, registerSetting) {
                var hasNonRegisterVegetarianDate = false;

                if (!date.IsDisabled) {
                    var toDay = new Date(),
                        sevenDay = new Date().setDate(toDay.getDate() + registerSetting.Order),
                        nextSevenDay = new Date(sevenDay),
                        currentDate = new Date(date.WorkDate);

                    toDay.setHours(0, 0, 0, 0);
                    nextSevenDay.setHours(0, 0, 0, 0);
                    currentDate.setHours(0, 0, 0, 0);

                    if (+currentDate >= +toDay && currentDate < nextSevenDay) {
                        hasNonRegisterVegetarianDate = true;
                    }
                }

                return hasNonRegisterVegetarianDate;
            };

            utility.checkNonRegisterVegetarianDateInWeek = function (week, registerSetting) {
                var hasNonRegisterVegetarianDate = false;

                for (var i = 0; i < week.WeekMeals.length; i++) {
                    var date = week.WeekMeals[i];
                    hasNonRegisterVegetarianDate = utility.checkNonRegisterVegetarianDate(date, registerSetting);

                    if (hasNonRegisterVegetarianDate) {
                        i = week.WeekMeals.length;
                    }
                }

                return hasNonRegisterVegetarianDate;
            };

            utility.checkNonRegisterVegetarianDateInMonth = function (weeks, registerSetting) {
                var hasNonRegisterVegetarianDate = false;

                for (var i = 0; i < weeks.length; i++) {
                    var week = weeks[i];
                    hasNonRegisterVegetarianDate = utility.checkNonRegisterVegetarianDateInWeek(week, registerSetting);

                    if (hasNonRegisterVegetarianDate) {
                        i = weeks.length;
                    }
                }

                return hasNonRegisterVegetarianDate;
            };

            utility.checkNonRegisterVegetarianSameDateInMonth = function (weeks, weekDayIndex, registerSetting) {
                var hasDateEffectedVegetarian = false;

                for (var i = 0; i < weeks.length; i++) {
                    var week = weeks[i],
                        date = week.WeekMeals[weekDayIndex];

                    hasDateEffectedVegetarian = utility.checkNonRegisterVegetarianDate(date, registerSetting);

                    if (hasDateEffectedVegetarian) {
                        i = weeks.length;
                    }
                }

                return hasDateEffectedVegetarian;
            };

            utility.checkIsUpdateVegetarian = function (meals) {
                var isUpdateVegetarian = false;

                for (var i = 0; i < meals.length; i++) {
                    if (meals[i].IsVegetarian) {
                        isUpdateVegetarian = true;
                        i = meals.length;
                    }
                }

                return isUpdateVegetarian;
            };

            //check the 1st day or the 15th day of the lunar calendar
            var getJuliusFromDate = function (dd, mm, yy) {
                var JD;
                if (yy > 1582 || (yy == 1582 && mm > 10) || (yy == 1582 && mm == 10 && dd > 14)) {
                    JD = 367 * yy - parseInt(7 * (yy + parseInt((mm + 9) / 12)) / 4) - parseInt(3 * (parseInt((yy + (mm - 9) / 7) / 100) + 1) / 4) + parseInt(275 * mm / 9) + dd + 1721028.5;
                } else {
                    JD = 367 * yy - parseInt(7 * (Y + 5001 + parseInt((mm - 9) / 7)) / 4) + parseInt(275 * mm / 9) + dd + 1729776.5;
                }
                return JD;
            }

            var getNewMoonDay = function (k, timeZone) {
                const PI = 3.14159265358979323846;
                var T, T2, T3, dr, Jd1, M, Mpr, F, C1, deltat, JdNew;
                T = k / 1236.85; // Time in Julian centuries from 1900 January 0.5
                T2 = T * T;
                T3 = T2 * T;
                dr = PI / 180;
                Jd1 = 2415020.75933 + 29.53058868 * k + 0.0001178 * T2 - 0.000000155 * T3;
                Jd1 = Jd1 + 0.00033 * Math.sin((166.56 + 132.87 * T - 0.009173 * T2) * dr); // Mean new moon
                M = 359.2242 + 29.10535608 * k - 0.0000333 * T2 - 0.00000347 * T3; // Sun's mean anomaly
                Mpr = 306.0253 + 385.81691806 * k + 0.0107306 * T2 + 0.00001236 * T3; // Moon's mean anomaly
                F = 21.2964 + 390.67050646 * k - 0.0016528 * T2 - 0.00000239 * T3; // Moon's argument of latitude
                C1 = (0.1734 - 0.000393 * T) * Math.sin(M * dr) + 0.0021 * Math.sin(2 * dr * M);
                C1 = C1 - 0.4068 * Math.sin(Mpr * dr) + 0.0161 * Math.sin(dr * 2 * Mpr);
                C1 = C1 - 0.0004 * Math.sin(dr * 3 * Mpr);
                C1 = C1 + 0.0104 * Math.sin(dr * 2 * F) - 0.0051 * Math.sin(dr * (M + Mpr));
                C1 = C1 - 0.0074 * Math.sin(dr * (M - Mpr)) + 0.0004 * Math.sin(dr * (2 * F + M));
                C1 = C1 - 0.0004 * Math.sin(dr * (2 * F - M)) - 0.0006 * Math.sin(dr * (2 * F + Mpr));
                C1 = C1 + 0.0010 * Math.sin(dr * (2 * F - Mpr)) + 0.0005 * Math.sin(dr * (2 * Mpr + M));
                if (T < -11) {
                    deltat = 0.001 + 0.000839 * T + 0.0002261 * T2 - 0.00000845 * T3 - 0.000000081 * T * T3;
                } else {
                    deltat = -0.000278 + 0.000265 * T + 0.000262 * T2;
                };
                JdNew = Jd1 + C1 - deltat;
                return parseInt(JdNew);
            }

            var getSunLongitude = function (jdn, timeZone) {
                const PI = 3.14159265358979323846;
                var T, T2, dr, M, L0, DL, L;
                T = (jdn - 2451545.5 - timeZone / 24) / 36525; // Time in Julian centuries from 2000-01-01 12:00:00 GMT
                T2 = T * T;
                dr = PI / 180; // degree to radian
                M = 357.52910 + 35999.05030 * T - 0.0001559 * T2 - 0.00000048 * T * T2; // mean anomaly, degree
                L0 = 280.46645 + 36000.76983 * T + 0.0003032 * T2; // mean longitude, degree
                DL = (1.914600 - 0.004817 * T - 0.000014 * T2) * Math.sin(dr * M);
                DL = DL + (0.019993 - 0.000101 * T) * Math.sin(dr * 2 * M) + 0.000290 * Math.sin(dr * 3 * M);
                L = L0 + DL; // true longitude, degree
                L = L * dr;
                L = L - PI * 2 * (parseInt(L / (PI * 2))); // Normalize to (0, 2*PI)
                return parseInt(L / PI * 6);
            }

            var getLunarMonth11 = function (yy, timeZone) {
                var k, off, nm, sunLong;
                off = getJuliusFromDate(31, 12, yy) - 2415021;
                k = parseInt(off / 29.530588853);
                nm = getNewMoonDay(k, timeZone);
                sunLong = getSunLongitude(nm, timeZone); // sun longitude at local midnight
                if (sunLong >= 9) {
                    nm = getNewMoonDay(k - 1, timeZone);
                }
                return nm;
            }

            var getLeapMonthOffset = function (a11, timeZone) {
                var k, last, arc, i;
                k = parseInt((a11 - 2415021.076998695) / 29.530588853 + 0.5);
                last = 0;
                i = 1; // We start with the month following lunar month 11
                arc = getSunLongitude(getNewMoonDay(k + i, timeZone), timeZone);
                do {
                    last = arc;
                    i++;
                    arc = getSunLongitude(getNewMoonDay(k + i, timeZone), timeZone);
                } while (arc !== last && i < 14);
                return i - 1;
            }

            utility.checkVegetarianDay = function (dd, mm, yy, timeZone) {
                var k, dayNumber, monthStart, a11, b11, lunarDay, lunarMonth, lunarYear, lunarLeap, diff, leapMonthDiff;
                dayNumber = getJuliusFromDate(dd, mm, yy);
                k = parseInt((dayNumber - 2415021.076998695) / 29.530588853);
                monthStart = getNewMoonDay(k + 1, timeZone);
                if (monthStart > dayNumber) {
                    monthStart = getNewMoonDay(k, timeZone);
                }
                a11 = getLunarMonth11(yy, timeZone);
                b11 = a11;
                if (a11 >= monthStart) {
                    lunarYear = yy;
                    a11 = getLunarMonth11(yy - 1, timeZone);
                } else {
                    lunarYear = yy + 1;
                    b11 = getLunarMonth11(yy + 1, timeZone);
                }
                lunarDay = dayNumber - monthStart + 1;
                diff = parseInt((monthStart - a11) / 29);
                lunarLeap = 0;
                lunarMonth = diff + 11;
                if (b11 - a11 > 365) {
                    leapMonthDiff = getLeapMonthOffset(a11, timeZone);
                    if (diff >= leapMonthDiff) {
                        lunarMonth = diff + 10;
                        if (diff == leapMonthDiff) {
                            lunarLeap = 1;
                        }
                    }
                }
                if (lunarMonth > 12) {
                    lunarMonth = lunarMonth - 12;
                }
                if (lunarMonth >= 11 && diff < 4) {
                    lunarYear -= 1;
                }
                return (parseInt(lunarDay));
            }

            return utility;
        }]);