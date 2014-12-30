'use strict';
/*global reportModule*/

reportModule.controller('DailyMainDishReportController',
['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'userControlFactory', 'orderingFactory',
'foodMenuFactory', 'commonFactory', 'foodMenuUtilities', 'notificationFactory',

function ($scope, reportFactory, ErrorFactory, reportUtilities, userControlFactory, orderingFactory,
foodMenuFactory, commonFactory, foodMenuUtilities, notificationFactory) {
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
			getEmployeeType($scope.filter.departmentId, null);
		}
		else {
			getEmployeeType(null, null);
		}
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
	$scope.getEmployee = function (searchKey) {
		$scope.filter.employee = '';

		orderingFactory.getEmployees(searchKey).then(
		function (employees) {
			$scope.searchEmployees = employees;
		},
		function (response) {
			ErrorFactory.errorHandle(response.status);
		});
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
		getEmployeeType(departmentId, null);
	};

	/*
	** Query data.
	*/
	$scope.query = function (filter) {
		resetGridOption();
		filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
		filter.teamId = reportUtilities.getTeamId($scope.teamOptions);
		filter.userId = filter.employee ? filter.employee.UserId : '';

		var fromDate = new Date(filter.fromDate),
			toDate = new Date(filter.toDate);

		if (fromDate > toDate) {
			notificationFactory.error("DateRangeInvalid");
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
