'use strict';
/*global reportModule*/

reportModule.controller('MonthlyMainDishReportLocalController',
['$scope', 'reportFactory', 'ErrorFactory', 'userControlFactory', 'foodMenuFactory', 'foodMenuUtilities',
'reportUtilities',
function ($scope, reportFactory, ErrorFactory, userControlFactory, foodMenuFactory, foodMenuUtilities,
reportUtilities) {

	/*
	** Declare variable.
	*/
	var currentDate = new Date();

	$scope.filter = {
		reportDate: currentDate,
		departmentId: '',
		mealId: '',
		employeeTypeId: '',
		teamId: '',
		groupBy: 0,
		startIndex: 0,
		itemsPerPage: 50,
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
		$scope.getMonthlyReportLocal($scope.filter);

		if (departments && departments.length === 1) {
			$scope.getTeamByCompany($scope.filter.departmentId);
			getEmployeeType($scope.filter.departmentId, true);
		}
		else {
			getEmployeeType(null, true);
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
	var getEmployeeType = function (departmentId, isLocal) {
		foodMenuFactory.getAllEmployeeType(departmentId, isLocal).success(function (employeeTypes) {
			$scope.employeeTypesOption = employeeTypes;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	/*
	** 
	*/
	$scope.getTeamAndEmployeeTypeOfDepartment = function (departmentId) {
		$scope.getTeamByCompany(departmentId);
		getEmployeeType(departmentId, true);
	};

	/*
	** Get monthly main dish report by local.
	*/
	$scope.getMonthlyReportLocal = function (filter) {
		reportFactory.getMonthlyMainDishReportByLocal(filter)
			.success(function (report) {
				$scope.localReport = report;
				$scope.$broadcast('renderMonthlyReport');
			})
			.error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
	};

	/*
	** Query data.
	*/
	$scope.query = function (filter) {
		resetGridOption();
		filter.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
		filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
		filter.teamId = reportUtilities.getTeamId($scope.teamOptions);

		if (!filter.departmentId) {
			filter.departmentId = $scope.originalDepartmentId;
		}

		$scope.getMonthlyReportLocal(filter);
	};

	/*
	** Export excel.
	*/
	$scope.exportExcel = function (filter) {
		filter.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
		filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
		filter.teamId = reportUtilities.getTeamId($scope.teamOptions);

		$scope.criteria = {
			reportDate: filter.reportDate.toDateString(),
			departmentId: filter.departmentId,
			employeeTypeId: filter.employeeTypeId,
			teamId: filter.teamId,
			mealId: filter.mealId,
			groupBy: filter.groupBy,
			isLocal: 1
		};

		$scope.$broadcast('exportMonthlyMainDishLocalReport');
	};
} ]);