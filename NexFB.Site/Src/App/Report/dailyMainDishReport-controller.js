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
	$scope.originalDepartmentId = '';

	var getDepartmentFilter = function (departments) {
		var departmentId = '';

		if (departments) {
			if (departments.length === 1) {
				departmentId = departments[0].DepartmentId;
			}
			else {
				angular.forEach(departments, function (department) {
					departmentId = departmentId ? departmentId + ',' + department.DepartmentId : department.DepartmentId;
				});
			}
		}

		$scope.filter.departmentId = departmentId;
		$scope.originalDepartmentId = departmentId;
	};

	/*
	** Get all department.
	*/
	reportFactory.getDepartmentByUserRole().success(function (departments) {
		$scope.departmentOptions = departments;
		getDepartmentFilter(departments);
		$scope.getDailyMainDishReport($scope.filter);
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	$scope.getDailyMainDishReport = function (filter) {
		reportFactory.getDailyMainDishReport(filter).success(function (report) {
			$scope.report = report;
			$scope.$broadcast("renderDailyReport");
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};
} ]);
