'use strict';
/*global reportModule*/

reportModule.controller('EmployeeOrderHistoryReportController',
['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'foodMenuFactory', 'commonFactory',
'foodMenuUtilities',
function ($scope, reportFactory, ErrorFactory, reportUtilities, foodMenuFactory, commonFactory,
foodMenuUtilities) {
	$scope.toDate = new Date();

	$scope.filter = {
		fromDate: new Date($scope.toDate.getFullYear(), $scope.toDate.getMonth(), 1),
		toDate: new Date(),
		orderBy: 0,
		employeeMenu: 0,
		orderPlatform: 0,
		mealId: '',
		statusId: '',
		isPagingCallback: false,
		startIndex: 0,
		itemsPerPage: 50
	};

	$scope.orderStatus = {
		New : 1,
		Accepted: 2,
		Reduced: 3,
		Delivered: 4,
		Edited: 5,
		Rejected: 6
	};

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
		itemsPerPage: 50,
		currentPage: 0,
		startIndex: 0
	};

	/*
	** Reset grid option.
	*/
	var resetGridOption = function () {
		$scope.gridOption.itemsPerPage = 50;
		$scope.gridOption.currentPage = 0;
		$scope.gridOption.startIndex = 0;
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
	** Get Ordered By.
	*/
	reportFactory.getOrderedByEnum().success(function (orderedByOptions) {
		$scope.orderedByOptions = orderedByOptions;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get Order Plaform.
	*/
	reportFactory.getOrderPlatformEnum().success(function (orderPlatformOptions) {
		$scope.orderPlatformOptions = orderPlatformOptions;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get Order Status.
	*/
	reportFactory.getOrderStatusEnum().success(function (orderStatuses) {
		$scope.orderStatusesOptions = orderStatuses;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get Menu Type.
	*/
	reportFactory.getMenuTypeEnum().success(function (menuTypes) {
		$scope.menuTypeOptions = menuTypes;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get report.
	*/
	$scope.getEmployeeOrderHistoryReport = function (filter) {
		reportFactory.getEmployeeOrderHistoryReport(filter).success(function (report) {
			$scope.report = report;
			$scope.$broadcast("renderEmployeeOrderHistoryReport");
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	$scope.getEmployeeOrderHistoryReport($scope.filter);

	var getOrderStatusId = function (orderStatusesOptions) {
		var statusId = '';

		angular.forEach(orderStatusesOptions, function (value) {
			if (value.ticked) {
				var id = value.Value.toString();
				statusId = statusId !== '' ? statusId + ',' + id : id;
			}
		});

		return statusId;
	};

	/*
	** Filter report.
	*/
	$scope.query = function (filter) {
		resetGridOption();
		filter.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
		filter.statusId = getOrderStatusId($scope.orderStatusesOptions);

		$scope.getEmployeeOrderHistoryReport(filter);
	};
} ]);