'use strict';
/*global reportModule*/

reportModule.controller('MealForecastReportController',
['$scope', 'baseUrl', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'commonFactory', 'notificationFactory',
function ($scope, baseUrl, reportFactory, ErrorFactory, reportUtilities, commonFactory, notificationFactory) {
	$scope.toDate = new Date();
	$scope.history = reportFactory.getHistory();

	$scope.filter = {
		fromDate: $scope.history.fromDate,
		toDate: $scope.history.toDate,
		departmentId: $scope.history.departmentId,
		vegetarianType: $scope.history.vegetarianType,
		isPagingCallback: $scope.history.isPagingCallback,
		startIndex: $scope.history.startIndex,
		itemsPerPage: $scope.history.itemsPerPage
	};

	$scope.baseUrl = baseUrl;
	$scope.numberFormat = reportUtilities.getNumberFormat();
	$scope.dateFormat = reportUtilities.getDateFormatEmployeeOrderHistoryReport();

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

	$scope.gridOption = {
		itemsPerPage: $scope.history.itemsPerPage,
		currentPage: $scope.history.currentPage,
		startIndex: $scope.history.startIndex
	};

	/*
	** Reset grid option.
	*/
	var resetGridOption = function () {
		$scope.gridOption = {
			itemsPerPage: 50,
			currentPage: 0,
			startIndex: 0
		};
	};

	/*
	** Reset paging option.
	*/
	var resetPagingOption = function (filter) {
		filter.startIndex = 0;
		filter.itemsPerPage = 50;
		filter.isPagingCallback = false;
	};

	var getDepartmentFilter = function (departments) {
		var departmentId = '';

		if (departments.length === 1) {
			departmentId = departments[0].DepartmentId;
		}
		else {
			angular.forEach(departments, function (department) {
				departmentId = departmentId ? departmentId + ',' + department.DepartmentId : department.DepartmentId;
			});
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

		if ($scope.history.isCallBack) {
			$scope.filter.departmentId = angular.isNumber($scope.history.departmentId) ? parseInt($scope.history.departmentId) : $scope.history.departmentId;
		}

		$scope.getMealForecastReport($scope.filter);
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get report
	*/
	$scope.getMealForecastReport = function (filter) {
		reportFactory.getMealForecastReport(filter).success(function (report) {
			$scope.report = report;
			$scope.$broadcast("renderMealForecastReport");
			$scope.setHistory();
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	$scope.setHistory = function () {
		var departmentId = $scope.filter.departmentId;

		if (!departmentId) {
			$scope.filter.departmentId = $scope.originalDepartmentId;
		}
		reportFactory.setHistory($scope, true);
	};

	/*
	** Filter report.
	*/
	$scope.query = function (filter) {
		resetGridOption();
		resetPagingOption(filter);

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
			$scope.getMealForecastReport(filter);
		}
	};

	/*
	**  Export excel
	*/
	$scope.exportExcel = function (filter) {
		$scope.criteria = {
			fromDate: filter.fromDate.toDateString(),
			toDate: filter.toDate.toDateString(),
			departmentId: filter.departmentId,
			isVegetarian: filter.vegetarianType.length === 0 ? '' : filter.vegetarianType === '0' ? false : true
		};
		$scope.$broadcast('exportMealForecastReport');
	};
} ]);