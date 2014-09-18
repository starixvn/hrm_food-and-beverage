'use strict';
/*global mealRegistrationModule*/

mealRegistrationModule.controller('EmployeeRegistrationMealController',
['$scope', 'mealRegistrationFactory', 'mealRegistrationUtilities', 'notificationFactory', '$route',
function ($scope, mealRegistrationFactory, mealRegistrationUtilities, notificationFactory, $route) {
	mealRegistrationUtilities.getTabHeading($scope);

	mealRegistrationFactory.getUserPermissionOnRegistration().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

	$scope.reload = function () {
		$route.reload();
	};
} ]);

mealRegistrationModule.controller('MonthlyRegistrationController',
['$scope', 'mealRegistrationFactory', '$modal', 'notificationFactory', 'ErrorFactory', 'mealRegistrationUtilities',
function ($scope, mealRegistrationFactory, $modal, notificationFactory, ErrorFactory, mealRegistrationUtilities) {
	/*
	** Declare variable.
	*/
	var currentDate = new Date();

	$scope.currentDate = currentDate;
	$scope.format = 'MMM yyyy';
	$scope.popUpMode = 'month';
	$scope.dateOptions = {
		minMode: 'month',
		maxMode: 'month',
		formatMonth: 'MMM'
	};

	/*
	** Open pop-up calendar.
	*/
	$scope.open = function ($event) {
		$event.preventDefault();
		$event.stopPropagation();

		$scope.opened = true;
	};
	/*
	** Get register meal by range date.
	*/
	var getRegisterMealByRangeDate = function (fromDate, toDate, mealId) {
		mealRegistrationFactory.getRegisterMealByRangeDate(fromDate.toDateString(), toDate.toDateString(), mealId).success(function (mealDates) {
			$scope.allWeekMeals = mealDates;
		}).error(function () {
			notificationFactory.error("ErrorMessage");
		});
	};

	$scope.$watch('tabMonthly', function (newValue) {
		if (newValue === true) {
			var changedMonth = $scope.currentDate,
				fromDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth(), 1),
				toDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth() + 1, 0);

			getRegisterMealByRangeDate(fromDate, toDate, null);
		}
	});

	/*
	** Check current date is today or not.
	*/
	$scope.checkCurrentDate = function (date) {
		var toDay = new Date(),
			workDate = new Date(date.WorkDate);

		if (toDay.getYear() === workDate.getYear() && toDay.getMonth() === workDate.getMonth() &&
				toDay.getDate() === workDate.getDate()) {
			return true;
		}

		return false;
	};

	/*
	** Check current date is weekend or not.
	*/
	$scope.checkWeekend = function (date) {
		var workDate = new Date(date.WorkDate);

		if (workDate.getDay() === 0 || workDate.getDay() === 6) {
			return true;
		}

		return false;
	};

	/*
	** Get all meal timesheet.
	*/
	mealRegistrationFactory.getAllMealTimesheet().success(function (mealTimesheet) {
		$scope.mealTimesheet = mealTimesheet;
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

	/*
	** Get next or previous month
	*/
	$scope.changeMonth = function (numberMonth) {
		var currentDate = $scope.currentDate,
			changedMonth = new Date(currentDate.setMonth(currentDate.getMonth() + numberMonth));
		$scope.currentDate = changedMonth;
	};

	/*
	** Get register meal when current date change.
	*/
	$scope.$watch('currentDate', function (newValue, oldValue) {
		if (newValue !== oldValue) {
			var changedMonth = $scope.currentDate,
				fromDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth(), 1),
				toDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth() + 1, 0);

			getRegisterMealByRangeDate(fromDate, toDate, null);
		}
	});

	/*
	** Check if a week has at least one enabled date.
	*/
	$scope.hasDateEnableInWeek = function (week) {
		var hasEnableDate = mealRegistrationUtilities.hasDateEnableInWeek(week, $scope.mealTimesheet);
		return hasEnableDate;
	};

	/*
	** Check if all same date in month has at least one enable date.
	*/
	$scope.hasDateEnabled = function (date, weeks) {
		var hasEnableDate = mealRegistrationUtilities.hasDateEnabled(date, weeks, $scope.mealTimesheet);
		return hasEnableDate;
	};

	/*
	** Check if month has at least one enabled date.
	*/
	$scope.hasDateEnabledInMonth = function (weeks) {
		var hasEnableDate = false;

		angular.forEach(weeks, function (week) {
			var hasEnableDateInWeek = mealRegistrationUtilities.hasDateEnableInWeek(week, $scope.mealTimesheet);

			if (hasEnableDateInWeek) {
				hasEnableDate = true;
			}
		});

		return hasEnableDate;
	};

	/* 
	** Register meal for a date.
	*/
	$scope.registerMeal = function (date) {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/EmployeeRegistration/Templates/registerMeal.html',
			controller: "RegisterMealController",
			resolve: {
				meals: function () {
					return angular.copy(date.Meals);
				},
				workDate: function () {
					return date.WorkDate;
				},
				mealTimesheet: function () {
					return $scope.mealTimesheet;
				},
				isSameMeal: function () {
					return true;
				},
				mode: function () {
					return 'month';
				},
				timePeriod: function () {
					return new Date(date.WorkDate).toString("MMM dd, yyyy");
				},
				isSerialRegistration: function () {
					return false;
				}
			},
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function (registrationInfo) {
			var mealRegistration = {
				meal: registrationInfo.mealsTemplate,
				onDate: date.WorkDate.toString()
			};

			mealRegistrationFactory.registerMealByRangeDate(mealRegistration).success(function (response) {
				if (response.IsSuccessful === true) {
					date.Meals = registrationInfo.meals;
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("DataChangeFail");
				}
			});
		});
	};

	/* 
	** Register meal for a week.
	*/
	var updatedRegistrationMealToWeek = function (week, meals) {
		angular.forEach(week.WeekMeals, function (date) {
			if (!date.IsDisabled) {
				var toDay = new Date(),
					currentDate = new Date(date.WorkDate);

				// Don't update Sunday and Saturday
				if (currentDate.getDay() !== 0 && currentDate.getDay() !== 6) {
					if (toDay.getMonth() === currentDate.getMonth() && toDay.getDate() === currentDate.getDate()) {
						mealRegistrationUtilities.updateMealForToDay(date, meals, $scope.mealTimesheet);
					}
					else {
						date.Meals = angular.copy(meals);
					}
				}
			}
		});
	};

	$scope.registerMealWeek = function (week, index) {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/EmployeeRegistration/Templates/registerMeal.html',
			controller: "RegisterMealController",
			resolve: {
				meals: function () {
					var date = mealRegistrationUtilities.getDateNotToday(week);
					return angular.copy(date.Meals);
				},
				workDate: function () {
					return '';
				},
				mealTimesheet: function () {
					return $scope.mealTimesheet;
				},
				isSameMeal: function () {
					return mealRegistrationUtilities.checkSameMealInWeek(week, $scope.mealTimesheet);
				},
				mode: function () {
					return 'month';
				},
				timePeriod: function () {
					return 'Week ' + index;
				},
				isSerialRegistration: function () {
					return true;
				}
			},
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function (registrationInfo) {
			var mealRegistration = {
				meal: registrationInfo.mealsTemplate,
				onDate: mealRegistrationUtilities.getRegisterDateInWeek(week)
			};

			mealRegistrationFactory.registerMealByRangeDate(mealRegistration).success(function (response) {
				if (response.IsSuccessful === true) {
					updatedRegistrationMealToWeek(week, registrationInfo.meals);
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("DataChangeFail");
				}
			});
		});
	};

	/* 
	** Register meal for a month.
	*/
	var updatedRegistrationMealToMonth = function (weeks, meals) {
		angular.forEach(weeks, function (week) {
			updatedRegistrationMealToWeek(week, meals);
		});
	};

	$scope.registerMealMonth = function (allWeekMeals) {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/EmployeeRegistration/Templates/registerMeal.html',
			controller: "RegisterMealController",
			resolve: {
				meals: function () {
					return angular.copy(allWeekMeals[0].WeekMeals[0].Meals);
				},
				workDate: function () {
					return '';
				},
				mealTimesheet: function () {
					return $scope.mealTimesheet;
				},
				isSameMeal: function () {
					//					return checkSameMealInMonth(weeks);
					return false;
				},
				mode: function () {
					return 'month';
				},
				timePeriod: function () {
					return new Date($scope.currentDate).toString('MMM yyyy');
				},
				isSerialRegistration: function () {
					return true;
				}
			},
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function (registrationInfo) {
			var mealRegistration = {
				meal: registrationInfo.mealsTemplate,
				onDate: mealRegistrationUtilities.getRegisterDateInMonth(allWeekMeals)
			};

			mealRegistrationFactory.registerMealByRangeDate(mealRegistration).success(function (response) {
				if (response.IsSuccessful === true) {
					updatedRegistrationMealToMonth(allWeekMeals, registrationInfo.meals);
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("DataChangeFail");
				}
			});
		});
	};

	/* 
	** Register meal for a same date in month.
	*/
	var updatedRegistrationMealToSameDateInWeek = function (weeks, meals, weekDayIndex) {
		angular.forEach(weeks, function (week) {
			var date = week.WeekMeals[weekDayIndex];

			if (!date.IsDisabled) {
				var toDay = new Date(),
					currentDate = new Date(date.WorkDate);

				if (toDay.getMonth() === currentDate.getMonth() && toDay.getDate() === currentDate.getDate()) {
					mealRegistrationUtilities.updateMealForToDay(date, meals, $scope.mealTimesheet);
				}
				else {
					date.Meals = angular.copy(meals);
				}
			}
		});
	};

	$scope.registerMealDate = function (date, allWeekMeals) {
		var weekDayIndex = mealRegistrationUtilities.getWeekDayIndex(date);

		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/EmployeeRegistration/Templates/registerMeal.html',
			controller: "RegisterMealController",
			resolve: {
				meals: function () {
					return angular.copy(allWeekMeals[0].WeekMeals[0].Meals);
				},
				workDate: function () {
					return '';
				},
				mealTimesheet: function () {
					return $scope.mealTimesheet;
				},
				isSameMeal: function () {
					//					return checkSameMealInSameDate(date, allWeekMeals);
					return false;
				},
				mode: function () {
					return 'month';
				},
				timePeriod: function () {
					return mealRegistrationUtilities.getFullNameWeekday(date);
				},
				isSerialRegistration: function () {
					return true;
				}
			},
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function (registrationInfo) {
			var mealRegistration = {
				meal: registrationInfo.mealsTemplate,
				onDate: mealRegistrationUtilities.getRegisterDateOfWeek(weekDayIndex, allWeekMeals)
			};

			mealRegistrationFactory.registerMealByRangeDate(mealRegistration).success(function (response) {
				if (response.IsSuccessful === true) {
					updatedRegistrationMealToSameDateInWeek(allWeekMeals, registrationInfo.meals, weekDayIndex);
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("DataChangeFail");
				}
			});
		});
	};
} ]);

mealRegistrationModule.controller('DefaultRegisterMeal',
['$scope', 'mealRegistrationFactory', 'notificationFactory', '$modal', 'ErrorFactory', 'mealRegistrationUtilities',
function ($scope, mealRegistrationFactory, notificationFactory, $modal, ErrorFactory, mealRegistrationUtilities) {
	/*
	** Get default register meal.
	*/
	var getDefaultRegisterMeal = function () {
		mealRegistrationFactory.getDefaultRegisterMeal().success(function (weekMeals) {
			$scope.weekMeals = weekMeals;
		}).error(function () {
			notificationFactory.error("ErrorMessage");
		});
	};

	$scope.$watch('tabDefault', function (newValue) {
		if (newValue === true) {
			getDefaultRegisterMeal();
		}
	});

	/* 
	** Register meal for a date.
	*/
	$scope.registerMeal = function (date, index) {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/EmployeeRegistration/Templates/registerMeal.html',
			controller: "RegisterMealController",
			resolve: {
				meals: function () {
					return angular.copy(date.Meals);
				},
				workDate: function () {
					return date.WeekDay;
				},
				mealTimesheet: function () {
					return '';
				},
				isSameMeal: function () {
					return true;
				},
				mode: function () {
					return 'default';
				},
				timePeriod: function () {
					return mealRegistrationUtilities.getFullNameWeekdayFromIndex(index) + ' ' + new Date().getFullYear();
				},
				isSerialRegistration: function () {
					return false;
				}
			},
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function (registrationInfo) {
			var mealRegistration = {
				meal: registrationInfo.mealsTemplate,
				dayofWeek: date.WeekDay
			};

			mealRegistrationFactory.setDefaultRegisterMeal(mealRegistration).success(function (response) {
				if (response.IsSuccessful === true) {
					date.Meals = registrationInfo.meals;
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("DataChangeFail");
				}
			});
		});
	};

	/*
	** Register meal for a week.
	*/
	var updatedDefaultMealToWeek = function (week, meals) {
		angular.forEach(week, function (date) {
			var weekDay = date.WeekDay;

			// Don't update Sunday and Saturday
			if (weekDay !== 1 && weekDay !== 7) {
				date.Meals = angular.copy(meals);
			}
		});
	};

	$scope.registerMealWeek = function (weekMeals) {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/EmployeeRegistration/Templates/registerMeal.html',
			controller: "RegisterMealController",
			resolve: {
				meals: function () {
					return angular.copy(weekMeals[0].Meals);
				},
				workDate: function () {
					return '';
				},
				mealTimesheet: function () {
					return '';
				},
				isSameMeal: function () {
					//					return checkSameMealInWeek(weekMeals);
					return false;
				},
				mode: function () {
					return 'default';
				},
				timePeriod: function () {
					return 'All week ' + new Date().getFullYear();
				},
				isSerialRegistration: function () {
					return true;
				}
			},
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function (registrationInfo) {
			var mealRegistration = {
				meal: registrationInfo.mealsTemplate,
				dayofWeek: mealRegistrationUtilities.getDefaultDateInWeek(weekMeals)
			};

			mealRegistrationFactory.setDefaultRegisterMeal(mealRegistration).success(function (response) {
				if (response.IsSuccessful === true) {
					updatedDefaultMealToWeek(weekMeals, registrationInfo.meals);
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("DataChangeFail");
				}
			});
		});
	};

	/*
	** Apply template.
	*/
	$scope.apply = function () {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/EmployeeRegistration/Templates/modalConfirmApplyTemplate.html',
			controller: "ApplyTemplateConfirmController",
			resolve: {
				applyTemplate: function () {
					return true;
				}
			},
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function () {
			mealRegistrationFactory.applyFromTemplate().success(function (response) {
				if (response.IsSuccessful === true) {
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("DataChangeFail");
				}
			});
		});
	};
} ]);

mealRegistrationModule.controller('RegisterMealController',
['meals', '$scope', 'workDate', '$modalInstance', 'mealTimesheet', 'mode', 'isSameMeal', 'timePeriod',
'isSerialRegistration', '$modal',
function (meals, $scope, workDate, $modalInstance, mealTimesheet, mode, isSameMeal, timePeriod,
isSerialRegistration, $modal) {
	$scope.meals = meals;
	$scope.mode = mode;
	$scope.timePeriod = timePeriod;
	$scope.rightMeal = {
		1: true,
		2: true,
		3: true,
		4: true
	};

	if (mode === 'month') {
		$scope.workDate = workDate;
		$scope.isRegisterDate = workDate !== '' || isSameMeal ? true : false;
		$scope.mealTimesheet = mealTimesheet;
	}
	else if (mode === 'default') {
		$scope.isRegisterDate = isSameMeal ? true : false;
	}

	$scope.save = function (meals, mealsTemplate) {
		if (isSerialRegistration) {
			var modalAssignInstance = $modal.open({
				templateUrl: '/src/app/EmployeeRegistration/Templates/modalConfirmApplyTemplate.html',
				controller: "ApplyTemplateConfirmController",
				resolve: {
					applyTemplate: function () {
						return false;
					}
				},
				windowClass: "modal-confirm-center"
			});

			modalAssignInstance.result.then(function () {
				var registrationInfo = {
					meals: meals,
					mealsTemplate: mealsTemplate
				};
				$modalInstance.close(registrationInfo);
			});
		}
		else {
			var registrationInfo = {
				meals: meals,
				mealsTemplate: mealsTemplate
			};
			$modalInstance.close(registrationInfo);
		}
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);

mealRegistrationModule.controller('ApplyTemplateConfirmController',
['$scope', '$modalInstance', 'applyTemplate',
function ($scope, $modalInstance, applyTemplate) {
	$scope.applyTemplate = applyTemplate;

	$scope.ok = function () {
		$modalInstance.close();
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);