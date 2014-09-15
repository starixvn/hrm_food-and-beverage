'use strict';
/*global mealRegistrationModule*/

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

		var deafultHeadingCache = mealRegistrationCacheFactory.get("defaultHeading");

		if (deafultHeadingCache) {
			scope.defaultHeading = deafultHeadingCache;
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

					if (!onDate) {
						onDate = workDate;
					}
					else {
						onDate += ',' + workDate;
					}
				}
			}
		});

		return onDate;
	};

	utility.getRegisterDateInMonth = function (weeks) {
		var onDate = '';

		angular.forEach(weeks, function (week) {
			var registerDateInWeek = utility.getRegisterDateInWeek(week);

			if (!onDate) {
				onDate = registerDateInWeek;
			}
			else {
				onDate += ',' + registerDateInWeek;
			}
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
			registerHour = new Date(currentTimesheet.RegisterTime).getHours(),
			registerMinute = new Date(currentTimesheet.RegisterTime).getMinutes();

		if (currentHour > registerHour) {
			return false;
		}
		else if (currentHour === registerHour && currentMinute > registerMinute) {
			return false;
		}

		return true;
	};

	utility.getCurrentMeal = function (meals, mealId) {
		var meal;

		angular.forEach(meals, function (item) {
			if (item.MealId === mealId) {
				meal = item;
			}
		});

		return meal;
	};

	utility.updateMealForToDay = function (date, meals, mealTimesheet) {
		for (var i = 0; i < date.Meals.length; i++) {
			var meal = date.Meals[i],
				currentTimesheet = utility.getCurrentTimesheet(mealTimesheet, meal.MealId),
				rightMeal = utility.checkRightMeal(currentTimesheet),
				updateMeal = utility.getCurrentMeal(meals, meal.MealId);

			date.Meals[i] = rightMeal ? angular.copy(updateMeal) : angular.copy(meal);
		}
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

	utility.getRegisterDateOfWeek = function (weekDayIndex, weeks) {
		var onDate = '';

		angular.forEach(weeks, function (week) {
			var date = week.WeekMeals[weekDayIndex];

			if (!date.IsDisabled) {
				var workDate = date.WorkDate.toString();

				if (!onDate) {
					onDate = workDate;
				}
				else {
					onDate += ',' + workDate;
				}
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

	return utility;
} ]);