'use strict';
/*global reportModule*/

reportModule.controller('MonthlyMainDishReportPerDayController',
['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'userControlFactory', 'foodMenuFactory',
'foodMenuUtilities',
function ($scope, reportFactory, ErrorFactory, reportUtilities, userControlFactory, foodMenuFactory,
foodMenuUtilities) {

	$scope.filter = {
		reportDate: new Date(),
		departmentId: '',
		employeeTypeId: '',
		teamId: '',
		groupBy: 0,
		workDay: '',
		startIndex: 0,
		isExportReport: false,
		isPagingCallback: false
	};

	$scope.originalDepartmentId = '';

	$scope.format = 'MMM yyyy';
	$scope.popUpMode = 'month';
	$scope.dateOptions = {
		minMode: 'month',
		maxMode: 'month',
		formatMonth: 'MMM'
	};

	$scope.gridOption = {
		itemsPerPage: 50,
		currentPage: 0,
		startIndex: 0
	};

	$scope.numberFormat = reportUtilities.getNumberFormat();
	$scope.monthFormat = reportUtilities.getMonthFormat();
	$scope.teamOptions = [];

	/*
	** Reset grid option.
	*/
	var resetGridOption = function () {
		$scope.gridOption.itemsPerPage = 50;
		$scope.gridOption.currentPage = 0;
		$scope.gridOption.startIndex = 0;
	};

	/*
	** Open pop-up calendar.
	*/
	$scope.open = function ($event) {
		$event.preventDefault();
		$event.stopPropagation();

		$scope.opened = true;
	};

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
		$scope.getMonthlyMainDishReportPerDay($scope.filter);

		if (departments && departments.length === 1) {
			$scope.getTeamByCompany($scope.filter.departmentId);
			getEmployeeType($scope.filter.departmentId, null);
		}
		else {
			getEmployeeType(null, null);
		}
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
	** Get all employee type.
	*/
	var getEmployeeType = function (departmentId, isLocal) {
		foodMenuFactory.getAllEmployeeType(departmentId, isLocal).success(function (employeeTypes) {
			$scope.employeeTypesOption = employeeTypes;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	/*
	** Get team and employee type by department.
	*/
	$scope.getTeamAndEmployeeTypeOfDepartment = function (departmentId) {
		$scope.getTeamByCompany(departmentId);
		getEmployeeType(departmentId, null);
	};

	/*
	** Get Day Type.
	*/
	reportFactory.getDayTypeEnum().success(function (dayTypes) {
		$scope.dayTypeOptions = dayTypes;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get monthly main dish per day report.
	*/
	$scope.getMonthlyMainDishReportPerDay = function (filter) {
		reportFactory.getMonthlyMainDishReportPerDay(filter).success(function (report) {
			$scope.report = report;
			$scope.$broadcast('renderMonthlyMainDishPerDay');
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	/*
	** Query data.
	*/
	$scope.query = function (filter) {
		resetGridOption();
		filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
		filter.teamId = reportUtilities.getTeamId($scope.teamOptions);
		filter.isPagingCallback = false;

		if (!filter.departmentId) {
			filter.departmentId = $scope.originalDepartmentId;
		}

		$scope.getMonthlyMainDishReportPerDay(filter);
	};

	/*
	** Export excel.
	*/
	$scope.exportExcel = function (filter) {
		filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
		filter.teamId = reportUtilities.getTeamId($scope.teamOptions);

		$scope.criteria = {
			reportDate: filter.reportDate.toDateString(),
			departmentId: filter.departmentId,
			employeeTypeId: filter.employeeTypeId,
			teamId: filter.teamId,
			workDay: filter.workDay,
			groupBy: filter.groupBy
		};

		$scope.$broadcast('exportMonthlyMainDishPerDayReport');
	};
} ]);