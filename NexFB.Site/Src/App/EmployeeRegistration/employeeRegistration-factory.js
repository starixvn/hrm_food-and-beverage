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
		},
		getUserPermissionOnRegistration: function () {
			return $http.get('/api/MealRegistration/GetUserPermissionOnRegistration');
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
			registerHour = new Date(currentTimesheet.RegisterTime + 'Z').getUTCHours(),
			registerMinute = new Date(currentTimesheet.RegisterTime + 'Z').getUTCMinutes();

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

	utility.getFullNameWeekday = function (date) {
		var fullName = '';

		switch (date) {
			case 'Sun':
				fullName = 'Sundays';
				break;
			case 'Mon':
				fullName = 'Mondays';
				break;
			case 'Tue':
				fullName = 'Tuesdays';
				break;
			case 'Wed':
				fullName = 'Wednesdays';
				break;
			case 'Thu':
				fullName = 'Thursdays';
				break;
			case 'Fri':
				fullName = 'Fridays';
				break;
			case 'Sat':
				fullName = 'Saturdays';
				break;
		}

		return fullName;
	};

	utility.getFullNameWeekdayFromIndex = function (weekDayIndex) {
		var fullName = '';

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

	utility.hasDateEnableInWeek = function (week, mealTimesheet) {
		var hasEnableDate = false;

		angular.forEach(week.WeekMeals, function (date) {
			var toDay = new Date(),
				currentDate = new Date(date.WorkDate);

			// Don't check Sunday and Saturday
			if (currentDate.getDay() !== 0 && currentDate.getDay() !== 6) {
				if (!date.IsDisabled) {
					if (toDay.getMonth() === currentDate.getMonth() && toDay.getDate() === currentDate.getDate()) {
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
				if (toDay.getMonth() === currentDate.getMonth() && toDay.getDate() === currentDate.getDate()) {
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

//	utility.checkSameMeal = function (firstDate, secondDate) {
//	};

//	utility.checkSameMealInWeek = function (week) {
//		var isSameMeal = true,
//			length = week.WeelMeals.length;

//		for (var i = 0; i < length - 1; i++) {
//			var date = week.WeelMeals[i],
//				nextDate = week.WeelMeals[i + 1];
//			currentDate = new Date(date.WorkDate);

//			// Don't check Sunday and Saturday
//			if (currentDate.getDay() !== 0 && currentDate.getDay() !== 6) {
//				if (!date.IsDisabled) {
//					var checked = checkSameMeal(date, nextDate);

//					if (!checked) {
//						isSameMeal = false;
//					}
//				}
//			}
//		}

//		return isSameMeal;
//	};

	return utility;
} ]);