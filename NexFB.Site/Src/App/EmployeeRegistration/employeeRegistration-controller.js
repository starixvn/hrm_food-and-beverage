'use strict';
/*global mealRegistrationModule*/

mealRegistrationModule.controller('EmployeeRegistrationMealController',
['$scope', 'mealRegistrationFactory', 'mealRegistrationUtilities',
function ($scope, mealRegistrationFactory, mealRegistrationUtilities) {
	mealRegistrationUtilities.getTabHeading($scope);
} ]);

mealRegistrationModule.controller('MonthlyRegistrationController',
['$scope', 'mealRegistrationFactory',
function ($scope, mealRegistrationFactory) {
	var date = new Date(),
		fromDate = new Date(date.getFullYear(), date.getMonth(), 1),
		toDate = new Date(date.getFullYear(), date.getMonth() + 1, 0);


	mealRegistrationFactory.getRegisterMealByRangeDate(194, fromDate.toDateString(), toDate.toDateString(), null).success(function (mealDates) {
		$scope.weekMeals = mealDates;
	}).error(function () {
		// TODO
	});
} ]);