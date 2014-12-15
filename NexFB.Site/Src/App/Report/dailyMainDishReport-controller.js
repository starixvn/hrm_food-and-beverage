'use strict';
/*global reportModule*/

reportModule.controller('DailyMainDishReportController',
['$scope', 'reportFactory', 'ErrorFactory',
function ($scope, reportFactory, ErrorFactory) {
	$scope.filter = {
		formDate: new Date(),
		toDate: new Date(),
		orderCount: 0,
		departmentId: '',
		teamId: '',
		mealId: '',
		userId: '',
		query: ''
	};

	reportFactory.getDailyMainDishReport($scope.filter).success(function (report) {
		$scope.report = report;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});
} ]);
