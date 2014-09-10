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
				}
			}
		});

		modalAssignInstance.result.then(function (registrationInfo) {
			var mealRegistration = {
				meal: registrationInfo.mealsTemplate,
				onDate: '2014-9-10'
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

	$scope.registerMealWeek = function (week) {
	};

	$scope.registerMealMonth = function (weekMeals) {
	};

	$scope.registerMealDate = function (date) {
	};
} ]);

mealRegistrationModule.controller('RegisterMealController',
['meals', '$scope', 'isRegisterDate', '$modalInstance',
function (meals, $scope, isRegisterDate, $modalInstance) {
	$scope.meals = meals;
	$scope.isRegisterDate = isRegisterDate;

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