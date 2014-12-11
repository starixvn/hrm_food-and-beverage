'use strict';
/*global mealRegistrationModule*/
/*jshint maxdepth:6 */

mealRegistrationModule.factory('mealRegistrationFactory', ['$http', function ($http) {
	return {
		getRegisterMealByRangeDate: function (fromDate, toDate, mealId) {
			return $http.get('/api/MealRegistration/GetRegisterMealByRangeDate',
				{ params: { fromDate: fromDate, toDate: toDate, mealId: mealId} });
		},
		registerMealByRangeDate: function (mealRegistration) {
			return $http.post('/api/MealRegistration/RegisterMealByRangeDate', mealRegistration);
		},
		getAllMealTimesheet: function () {
			return $http.get('/api/Common/GetAllMealTimesheet');
		},
		getDefaultRegisterMeal: function () {
			return $http.get('/api/MealRegistration/GetDefaultRegisterMeal');
		},
		setDefaultRegisterMeal: function (mealRegistration) {
			return $http.post('/api/MealRegistration/RegisterMealByDefault', mealRegistration);
		},
		applyFromTemplate: function () {
			return $http.put('/api/MealRegistration/ApplyRegisterMealFromTemplate');
		},
		getUserPermissionOnRegistration: function () {
			return $http.get('/api/MealRegistration/GetUserPermissionOnRegistration');
		},
		getRegisterMealSetting: function () {
			return $http.get('/api/MealRegistration/GetRegisterMealSetting');
		}
	};
} ]);

mealRegistrationModule.factory('mealRegistrationCacheFactory', ['$cacheFactory', function ($cacheFactory) {
	return $cacheFactory('mealRegistrationCache');
} ]);

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

	utility.hasDateEnableInWeek = function (week, mealTimesheet) {
		var hasEnableDate = false;

		angular.forEach(week.WeekMeals, function (date) {
			var toDay = new Date(),
				currentDate = new Date(date.WorkDate);

			// Don't check Sunday and Saturday
			if (currentDate.getDay() !== 0 && currentDate.getDay() !== 6) {
				if (!date.IsDisabled) {
					if (toDay.getMonth() === currentDate.getMonth() && toDay.getDate() === currentDate.getDate() &&
							toDay.getFullYear() === currentDate.getFullYear()) {
						angular.forEach(date.Meals, function (meal) {
							var currentTimesheet = utility.getCurrentTimesheet(mealTimesheet, meal.MealId),
								rightMeal = utility.checkRightMeal(currentTimesheet);

							if (rightMeal) {
								hasEnableDate = true;
							}
						});
					}
					else {
						hasEnableDate = true;
					}
				}
			}
		});

		return hasEnableDate;
	};

	utility.hasDateEnabled = function (date, weeks, mealTimesheet) {
		var hasEnableDate = false,
			weekDayIndex = utility.getWeekDayIndex(date),
			toDay = new Date();

		angular.forEach(weeks, function (week) {
			var date = week.WeekMeals[weekDayIndex],
				currentDate = new Date(date.WorkDate);

			if (!date.IsDisabled) {
				if (toDay.getMonth() === currentDate.getMonth() && toDay.getDate() === currentDate.getDate() &&
						toDay.getFullYear() === currentDate.getFullYear()) {
					angular.forEach(date.Meals, function (meal) {
						var currentTimesheet = utility.getCurrentTimesheet(mealTimesheet, meal.MealId),
								rightMeal = utility.checkRightMeal(currentTimesheet);

						if (rightMeal) {
							hasEnableDate = true;
						}
					});
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

	return utility;
} ]);