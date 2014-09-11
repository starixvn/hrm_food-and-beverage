'use strict';
/*global mealRegistrationModule*/

mealRegistrationModule.controller('EmployeeRegistrationMealController',
['$scope', 'mealRegistrationFactory', 'mealRegistrationUtilities',
function ($scope, mealRegistrationFactory, mealRegistrationUtilities) {
	mealRegistrationUtilities.getTabHeading($scope);
} ]);

mealRegistrationModule.controller('MonthlyRegistrationController',
['$scope', 'mealRegistrationFactory', '$modal', 'notificationFactory', 'ErrorFactory',
function ($scope, mealRegistrationFactory, $modal, notificationFactory, ErrorFactory) {
	var date = new Date(),
		fromDate = new Date(date.getFullYear(), date.getMonth(), 1),
		toDate = new Date(date.getFullYear(), date.getMonth() + 1, 0);


	mealRegistrationFactory.getRegisterMealByRangeDate(194, fromDate.toDateString(), toDate.toDateString(), null).success(function (mealDates) {
		$scope.weekMeals = mealDates;
	}).error(function () {
		// TODO
	});

	mealRegistrationFactory.getAllMealTimesheet().success(function (mealTimesheet) {
		$scope.mealTimesheet = mealTimesheet;
	}).error(function () {
		// TODO
	});

	$scope.checkCurrentDate = function (date) {
		var toDay = new Date(),
			currentDate = new Date(date.WorkDate);

		if (toDay.getMonth() === currentDate.getMonth() && toDay.getDate() === currentDate.getDate()) {
			return true;
		}

		return false;
	};

	$scope.registerMeal = function (date) {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/EmployeeRegistration/Templates/registerMeal.html',
			controller: "RegisterMealController",
			resolve: {
				meals: function () {
					return date.Meals;
				},
				isRegisterDate: function () {
					return true;
				},
				mealTimesheet: function () {
					return $scope.mealTimesheet;
				}
			}
		});

		modalAssignInstance.result.then(function (registrationInfo) {
			var mealRegistration = {
				meal: registrationInfo.mealsTemplate,
				onDate: date.WorkDate.toString()
			};

			mealRegistrationFactory.registerMealByRangeDate(mealRegistration).success(function (response) {
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

	$scope.registerMealWeek = function (week) {
	};

	$scope.registerMealMonth = function (weekMeals) {
	};

	$scope.registerMealDate = function (date) {
	};
} ]);

mealRegistrationModule.controller('RegisterMealController',
['meals', '$scope', 'isRegisterDate', '$modalInstance', 'mealTimesheet',
function (meals, $scope, isRegisterDate, $modalInstance, mealTimesheet) {
	$scope.meals = meals;
	$scope.isRegisterDate = isRegisterDate;
	$scope.mealTimesheet = mealTimesheet;
	$scope.rightMeal = {
		1: true,
		2: true,
		3: true,
		4: true
	};

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