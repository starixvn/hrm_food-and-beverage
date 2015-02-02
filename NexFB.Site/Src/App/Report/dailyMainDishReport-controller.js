'use strict';
/*global reportModule*/

reportModule.controller('DailyMainDishReportController',
['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'userControlFactory',
'foodMenuFactory', 'commonFactory', 'foodMenuUtilities', 'notificationFactory', 'groupControlFactory',
function ($scope, reportFactory, ErrorFactory, reportUtilities, userControlFactory,
foodMenuFactory, commonFactory, foodMenuUtilities, notificationFactory, groupControlFactory) {
	var toDay = new Date(),
		fromDate = new Date(toDay.getFullYear(), toDay.getMonth(), 1),
		toDate = new Date();

	$scope.filter = {
		fromDate: fromDate,
		toDate: toDate,
		orderCount: 0,
		departmentId: '',
		employeeTypeId: '',
		teamId: '',
		mealId: '',
		userId: '',
		query: '',
		startIndex: 0,
		itemsPerPage: 50,
		isExportReport: false,
		isPagingCallback: false
	};

	$scope.gridOption = {
		itemsPerPage: 50,
		currentPage: 0,
		startIndex: 0
	};

	$scope.calendar = {
		startDateOpened: false,
		endDateOpened: false
	};


	$scope.dateFormat = reportUtilities.getDateFormatDailyReport();
	$scope.numberFormat = reportUtilities.getNumberFormat();
	$scope.originalDepartmentId = '';
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
	** Reset paging option.
	*/
	var resetPagingOption = function (filter) {
		filter.startIndex = 0;
		filter.itemsPerPage = 50;
		filter.isPagingCallback = false;
	};

	/*
	** Open calendar.
	*/
	$scope.openPopupStartDate = function ($event) {
		commonFactory.openPopupStartDate($event, $scope);
	};

	$scope.openPopupEndDate = function ($event) {
		commonFactory.openPopupEndDate($event, $scope);
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
		$scope.getDailyMainDishReport($scope.filter);

		if (departments && departments.length === 1) {
			$scope.getTeamByCompany($scope.filter.departmentId);
		}

		getEmployeeType($scope.filter.departmentId, null);		
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get daily main dish report.
	*/
	$scope.getDailyMainDishReport = function (filter) {
		reportFactory.getDailyMainDishReport(filter).success(function (report) {
			$scope.report = report;
			$scope.$broadcast("renderDailyReport");
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

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
	** Get all employees.
	*/
	$scope.getEmployee = function () {
		var departmentId = $scope.filter.departmentId;

		if (!departmentId) {
			departmentId = $scope.originalDepartmentId;
		}

		return groupControlFactory.getEmployees($scope.searchEmployee, departmentId, null, true);
	};

	$scope.unSelectEmployee = function () {
		if ($scope.searchEmployee.trim() === '') {
			if ($scope.filter && $scope.filter.employee) {
				$scope.filter.employee = null;
			}
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
	** Get team and employee type when department change.
	*/
	$scope.getTeamAndEmployeeTypeOfDepartment = function (departmentId) {
		$scope.getTeamByCompany(departmentId);

		if (!departmentId) {
			departmentId = $scope.originalDepartmentId;
		}

		getEmployeeType(departmentId, null);		
	};

	/*
	** Query data.
	*/
	var checkDateRangeInYear = function (fromDate, toDate) {
		fromDate.setFullYear(fromDate.getFullYear() + 1);
		fromDate.setHours(0, 0, 0, 0);
		toDate.setHours(0, 0, 0, 0);

		if (+fromDate < toDate) {
			return false;
		}

		return true;
	};

	$scope.query = function (filter) {
		resetGridOption();
		resetPagingOption(filter);

		filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
		filter.teamId = reportUtilities.getTeamId($scope.teamOptions);
		filter.userId = filter.employee ? filter.employee.UserId : '';

		if (!filter.departmentId) {
			filter.departmentId = $scope.originalDepartmentId;
		}

		var fromDate = new Date(filter.fromDate),
			toDate = new Date(filter.toDate);

		if (fromDate > toDate) {
			notificationFactory.error("DateRangeInvalid");
		}
		else if (!checkDateRangeInYear(angular.copy(fromDate), angular.copy(toDate))) {
			notificationFactory.error("DateRangeOneYearWarning");
		}
		else {
			$scope.getDailyMainDishReport(filter);
		}
	};

	/*
	** Export excel.
	*/
	$scope.exportExcel = function (filter) {
		filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
		filter.teamId = reportUtilities.getTeamId($scope.teamOptions);
		filter.userId = filter.employee ? filter.employee.UserId : '';

		$scope.criteria = {
			fromDate: filter.fromDate.toDateString(),
			toDate: filter.toDate.toDateString(),
			orderCount: filter.orderCount,
			departmentId: filter.departmentId,
			employeeTypeId: filter.employeeTypeId,
			teamId: filter.teamId,
			mealId: filter.mealId,
			userId: filter.userId,
			query: filter.query
		};

		$scope.$broadcast('exportDailyMainDishReport');
	};
} ]);
