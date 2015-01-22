'use strict';
/*global reportModule*/

reportModule.controller('SideDishReportController',
['$scope', 'reportFactory', 'notificationFactory', 'ErrorFactory', 'userControlFactory', 'foodMenuFactory', 'foodMenuUtilities',
'reportUtilities', 'sideDishManagementFactory', 'baseUrl',
function ($scope, reportFactory, notificationFactory, ErrorFactory, userControlFactory, foodMenuFactory, foodMenuUtilities,
reportUtilities, sideDishManagementFactory, baseUrl) {
	var self = this;
	var date = new Date(),
		year = date.getFullYear(),
		month = date.getMonth(),
		day = date.getDate();

	// $scope region.
	$scope.departments = [];
	$scope.teams = [];
	$scope.employeeTypes = [];
	$scope.sideDishItems = [];

	$scope.criteria = {
		departmentId: null,
		departmentIds: '',
		teamIds: '',
		employeeTypeIds: '',
		fromDate: new Date(year, month, 1),
		toDate: new Date(year, month, day),
		sideDishItemIds: '',
		pageIndex: 0,
		itemsPerPage: 50
	};

	$scope.fromDateOptions = {
		minDate: null,
		maxDate: null
	};

	$scope.toDateOptions = {
		minDate: null,
		maxDate: null
	};

	$scope.fromOpened = false;
	$scope.toOpened = false;

	$scope.open = function ($event, dateType) {
		$event.preventDefault();
		$event.stopPropagation();

		if (dateType === "from") {
			$scope.fromOpened = true;
			$scope.toOpened = false;
		} else if (dateType === "to") {
			$scope.toOpened = true;
			$scope.fromOpened = false;
		}
	};

	$scope.getTeamAndEmployeeType = function () {
		self.getTeams();
		self.getEmployeeTypes();
	};

	$scope.search = function () {
		var isValid = self.validateFilterOptions();

		if (isValid) {
			$scope.criteria.pageIndex = 0;

			$scope.getReportData();
		}
	};

	$scope.exportExcel = function () {
		self.getCriteria();

		var criteria = {
			departmentId: $scope.criteria.departmentIds,
			employeeTypeId: $scope.criteria.employeeTypeIds,
			teamId: $scope.criteria.teamIds,
			fromDate: $scope.criteria.fromDate.toString("MM/dd/yyyy"),
			toDate: $scope.criteria.toDate.toString("MM/dd/yyyy"),
			sideDishItemId: $scope.criteria.sideDishItemIds,
			pageIndex: null,
			itemsPerPage: null
		};

		var data = $.param(criteria),
			url = baseUrl + 'ExportReport/ExportSideDishReport',
			downloadUrl = url + "?" + data,
			iframe = $('body iframe');

		if (iframe.length === 0) {
			iframe = $('<iframe width="0" height="0" class="display-none"></iframe>');
			iframe.appendTo('body');
		}

		iframe.attr('src', downloadUrl);
	};

	$scope.getReportData = function () {
		self.getCriteria();

		reportFactory.getSideDishReport($scope.criteria).success(function (reportData) {
			$scope.reportData = reportData;
			$scope.$broadcast('renderSideDishReport');
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	$scope.getAllSideDish = function () {
		$scope.criteria.employeeTypeIds = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypes);

		if ($scope.criteria.employeeTypeIds.length === 0) {
			var ids = [];

			angular.forEach($scope.employeeTypes, function (employeeType) {
				ids.push(employeeType.EmployeeTypeId);
			});

			$scope.criteria.employeeTypeIds = ids.join(',');
		}

		reportFactory.getAllSideDish(null, null, $scope.criteria.employeeTypeIds).success(function (sideDishes) {
			$scope.sideDishItems = sideDishes;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	// self region.
	self.getTeams = function () {
		if ($scope.criteria.departmentId > 0) {
			userControlFactory.getAllTeamByCompany($scope.criteria.departmentId).success(function (teams) {
				$scope.teams = teams;
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		} else {
			$scope.teams = [];
		}
	};

	self.getDepartmentIds = function () {
		if ($scope.criteria.departmentId === -1) {
			var ids = [];

			angular.forEach($scope.departments, function (department) {
				if (department.DepartmentId !== -1) {
					ids.push(department.DepartmentId);
				}
			});

			$scope.criteria.departmentIds = ids.join(',');
		} else {
			$scope.criteria.departmentIds = $scope.criteria.departmentId;
		}
	};

	self.getEmployeeTypes = function () {
		self.getDepartmentIds();

		foodMenuFactory.getAllEmployeeType($scope.criteria.departmentIds).success(function (employeeTypes) {
			$scope.employeeTypes = employeeTypes;
			$scope.getAllSideDish();
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	self.getDepartment = function (callback) {
		reportFactory.getDepartmentByUserRole().success(function (departments) {
			if (departments.length > 1) {
				departments.splice(0, 0, { DepartmentId: -1, Name: '-- All --' });
			}

			$scope.departments = departments;

			if (departments.length > 0) {
				$scope.criteria.departmentId = departments[0].DepartmentId;
			}

			if (angular.isFunction(callback)) {
				callback();
			}

		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	self.loadData = function () {
		self.getDepartment(function () {
			$scope.getTeamAndEmployeeType($scope.criteria.departmentId);

			$scope.getReportData();
		});
	};

	self.validateFilterOptions = function () {
		var isValid = true;
		var fromDate = $scope.criteria.fromDate;
		var toDate = $scope.criteria.toDate;
		var maxDate = new Date(fromDate.getFullYear() + 1, fromDate.getMonth(), fromDate.getDate());

		if (fromDate > toDate) {
			isValid = false;
			notificationFactory.error("ValidateFromToDate");
		}
		else if (toDate > maxDate) {
			isValid = false;
			notificationFactory.error("ValidateMaximumOneYear");
		}

		return isValid;
	};

	self.getCriteria = function () {
		$scope.criteria.employeeTypeIds = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypes);
		$scope.criteria.teamIds = reportUtilities.getTeamId($scope.teams);
		$scope.criteria.sideDishItemIds = reportUtilities.getSideDishIds($scope.sideDishItems);
		self.getDepartmentIds();
	};

	self.loadData();
} ]);
