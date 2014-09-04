'use strict';
/*global mealRegistrationModule*/

mealRegistrationModule.controller('EmployeeRegistrationMealController',
['$scope', 'mealRegistrationFactory', 'mealRegistrationUtilities',
function ($scope, mealRegistrationFactory, mealRegistrationUtilities) {
	mealRegistrationUtilities.getTabHeading($scope);

	mealRegistrationFactory.getRegisterMealByRangeDate(194, new Date().toDateString(), new Date().toDateString(), null).success(function (mealDates) {
		$scope.mealDates = mealDates;
	}).error(function () {
		// TODO
	});
} ]);