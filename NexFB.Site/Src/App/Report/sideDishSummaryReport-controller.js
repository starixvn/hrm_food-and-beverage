'use strict';
/*global reportModule*/

reportModule.controller('SideDishSummaryReportController',
['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'userControlFactory', 'orderingFactory',
'commonFactory', 'foodMenuFactory', 'foodMenuUtilities', 'notificationFactory',
function ($scope, reportFactory, ErrorFactory, reportUtilities, userControlFactory, orderingFactory,
commonFactory, foodMenuFactory, foodMenuUtilities, notificationFactory) {
	$scope.toDate = new Date();

	$scope.filter = {
		fromDate: new Date($scope.toDate.getFullYear(), $scope.toDate.getMonth(), 1),
		toDate: new Date(),
		departmentId: '',
		teamId: '',
		employeeTypeId: '',
		employee: {},
		pageIndex: 0,
		itemsPerPage: 50
	};

	$scope.numberFormat = reportUtilities.getNumberFormat();
	$scope.dateFormat = reportUtilities.getDateFormatEmployeeOrderHistoryReport();
	$scope.originalDepartmentId = '';
	$scope.teamOptions = [];

	$scope.calendar = {
		startDateOpened: false,
		endDateOpened: false
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
		$scope.getSideDishSummaryReport($scope.filter);

		if (departments && departments.length === 1) {
			$scope.getTeamByCompany($scope.filter.departmentId);
		}

		getEmployeeType($scope.filter.departmentId, null);
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get side dish summary report
	*/
	$scope.getSideDishSummaryReport = function (filter) {
		reportFactory.getSideDishSummaryReport(filter).success(function (report) {
			$scope.report = report;
			$scope.$broadcast("renderSideDishSummaryReport");
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

		return orderingFactory.getEmployees($scope.searchEmployee, departmentId, $scope.filter.teamId, true);
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
	** Filter report.
	*/
	$scope.query = function (filter) {
		filter.pageIndex = 0;
		filter.itemsPerPage = 50;

		filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
		filter.teamId = reportUtilities.getTeamId($scope.teamOptions);
		reportFactory.setHistorySideDishSummary($scope);

		if (!filter.departmentId) {
			filter.departmentId = $scope.originalDepartmentId;
		}

		var fromDate = new Date(filter.fromDate),
			toDate = new Date(filter.toDate),
			maxDate = new Date(fromDate.getFullYear() + 1, fromDate.getMonth(), fromDate.getDate());

		if (fromDate > toDate) {
			notificationFactory.error("DateRangeInvalid");
		}
		else if (toDate > maxDate) {
			notificationFactory.error("DateRangeOneYearWarning");
		}
		else {
			$scope.getSideDishSummaryReport(filter);
		}
	};

	/*
	**  Export excel
	*/
	$scope.exportExcel = function (filter) {
		filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
		filter.teamId = reportUtilities.getTeamId($scope.teamOptions);

		$scope.criteria = {
			departmentId: filter.departmentId,
			employeeTypeId: filter.employeeTypeId,
			teamId: filter.teamId,
			userId: filter.employee ? filter.employee.UserId : '',
			fromDate: filter.fromDate.toDateString(),
			toDate: filter.toDate.toDateString(),
			pageIndex: null,
			itemsPerPage: null
		};

		$scope.$broadcast('exportSideDishSummaryReport');
	};
} ]);