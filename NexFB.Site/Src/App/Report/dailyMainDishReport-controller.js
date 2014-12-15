'use strict';
/*global reportModule*/

reportModule.controller('DailyMainDishReportController',
['$scope', 'reportFactory', 'ErrorFactory',
function ($scope, reportFactory, ErrorFactory) {
	var filterDate = new Date();
	filterDate.setDate(filterDate.getDate() - 4);

	$scope.filter = {
		fromDate: filterDate,
		toDate: filterDate,
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
