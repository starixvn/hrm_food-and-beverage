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
	var currentDate = new Date(),
		fromDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1),
		toDate = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0);

	$scope.currentDate = currentDate;

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
			changedMonth = new Date(currentDate.setMonth(currentDate.getMonth() + numberMonth)),
			fromDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth(), 1),
			toDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth() + 1, 0);

		getRegisterMealByRangeDate(fromDate, toDate, null);
		$scope.currentDate = changedMonth;
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
				mode: function () {
					return 'month';
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

	$scope.registerMealWeek = function (week) {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/EmployeeRegistration/Templates/registerMeal.html',
			controller: "RegisterMealController",
			resolve: {
				meals: function () {
					return angular.copy(week.WeekMeals[0].Meals);
				},
				workDate: function () {
					return '';
				},
				mealTimesheet: function () {
					return $scope.mealTimesheet;
				},
				mode: function () {
					return 'month';
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
				mode: function () {
					return 'month';
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
				mode: function () {
					return 'month';
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
	$scope.registerMeal = function (date) {
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
				mode: function () {
					return 'default';
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
				mode: function () {
					return 'default';
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
	};
} ]);

mealRegistrationModule.controller('RegisterMealController',
['meals', '$scope', 'workDate', '$modalInstance', 'mealTimesheet', 'mode',
function (meals, $scope, workDate, $modalInstance, mealTimesheet, mode) {
	$scope.meals = meals;
	$scope.mode = mode;
	$scope.rightMeal = {
		1: true,
		2: true,
		3: true,
		4: true
	};

	if (mode === 'month') {
		$scope.workDate = workDate;
		$scope.isRegisterDate = workDate !== '' ? true : false;
		$scope.mealTimesheet = mealTimesheet;
	}
	else if (mode === 'default' && workDate) {
		$scope.isRegisterDate = true;
	}

	$scope.save = function (meals, mealsTemplate) {
		var registrationInfo = {
			meals: meals,
			mealsTemplate: mealsTemplate
		};
		$modalInstance.close(registrationInfo);
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);