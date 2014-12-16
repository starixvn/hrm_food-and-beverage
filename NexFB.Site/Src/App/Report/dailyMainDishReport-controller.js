'use strict';
/*global reportModule*/

reportModule.controller('DailyMainDishReportController',
['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities',
function ($scope, reportFactory, ErrorFactory, reportUtilities) {
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

	$scope.dateFormat = reportUtilities.getDateFormatDailyReport();
	$scope.numberFormat = reportUtilities.getNumberFormat();

	reportFactory.getDailyMainDishReport($scope.filter).success(function (report) {
		$scope.report = report;
		$scope.$broadcast("renderDailyReport");
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});
} ]);
