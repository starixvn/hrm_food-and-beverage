'use strict';
/*global reportModule*/

reportModule.controller('EmployeeOrderHistoryReportController',
['$scope', 'reportFactory', 'ErrorFactory',
function ($scope, reportFactory, ErrorFactory) {

	$scope.filter = {
		fromDate: new Date(),
		toDate: new Date(),
		orderBy: 0,
		employeeMenu: 0,
		orderPlatform: 0,
		mealId: '',
		statusId: ''
	};

	reportFactory.getEmployeeOrderHistoryReport($scope.filter).success(function (report) {
		$scope.report = report;
		$scope.$broadcast("renderEmployeeOrderHistoryReport");
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});
} ]);