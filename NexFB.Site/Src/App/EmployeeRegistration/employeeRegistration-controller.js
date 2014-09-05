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

	mealRegistrationFactory.getRegisterMealByRangeDate(194, new Date().toDateString(), new Date().toDateString(), null).success(function (mealDates) {
		$scope.mealDates = mealDates;
	}).error(function () {
		// TODO
	});

	$scope.uiConfig = {
		calendar: {
			height: 450,
			editable: true,
			header: {
				left: 'month week',
				center: 'title',
				right: 'prev,next'
			},
			dayClick: $scope.alertEventOnClick,
			eventDrop: $scope.alertOnDrop,
			eventResize: $scope.alertOnResize
		}
	};

	$scope.eventSources = [];
} ]);