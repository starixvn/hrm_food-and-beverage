'use strict';
/*global reportModule*/

reportModule.controller('MonthlyMainDishReportLocalController',
['$scope', 'reportFactory', 'ErrorFactory',
function ($scope, reportFactory, ErrorFactory) {

	/*
	** Get monthly main dish report by local.
	*/
	var getMonthlyReportLocal = function (reportDate, departmentId, employeeTypeId, teamId, mealId, groupBy) {
		reportFactory.getMonthlyMainDishReportByLocal(reportDate, departmentId, employeeTypeId, teamId, mealId, groupBy)
			.success(function (report) {
				$scope.localReport = report;
			})
			.error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
	};

	getMonthlyReportLocal('2014 Nov', "", "", "", "", 0);
} ]);