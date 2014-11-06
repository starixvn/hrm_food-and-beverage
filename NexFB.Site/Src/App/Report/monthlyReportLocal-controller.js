'use strict';
/*global reportModule*/

reportModule.controller('MonthlyMainDishReportLocalController',
['$scope', 'reportFactory', 'ErrorFactory', 'userControlFactory', 'foodMenuFactory',
function ($scope, reportFactory, ErrorFactory, userControlFactory, foodMenuFactory) {

	/*
	** Declare variable.
	*/
	var currentDate = new Date();

	$scope.filter = {
		currentDate: currentDate,
		groupBy: 0
	};

	$scope.format = 'MMM yyyy';
	$scope.popUpMode = 'month';
	$scope.dateOptions = {
		minMode: 'month',
		maxMode: 'month',
		formatMonth: 'MMM'
	};

	/*
	** Open pop-up calendar.
	*/
	$scope.open = function ($event) {
		$event.preventDefault();
		$event.stopPropagation();

		$scope.opened = true;
	};

	/*
	** Get all department.
	*/
	userControlFactory.getAllDepartment().success(function (departments) {
		$scope.departmentOptions = departments;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all team of department.
	*/
	$scope.getTeamByCompany = function (departmentId) {
		if (departmentId) {
			userControlFactory.getAllTeamByCompany(departmentId).success(function (teams) {
				$scope.teamOptions = teams;
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		}
		else {
			$scope.teamOptions = '';
		}
	};

	/*
	** Get all meal timesheet.
	*/
	foodMenuFactory.getAllMealTimesheet().success(function (meals) {
		$scope.mealsOption = meals;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all employee type.
	*/
	foodMenuFactory.getAllEmployeeType().success(function (employeeTypes) {
		$scope.employeeTypesOption = employeeTypes;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

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