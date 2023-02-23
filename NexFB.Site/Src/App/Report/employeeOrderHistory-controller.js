'use strict';
/*global reportModule*/

reportModule.controller('EmployeeOrderHistoryReportController',
['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'foodMenuFactory', 'commonFactory',
'foodMenuUtilities', 'notificationFactory',
function ($scope, reportFactory, ErrorFactory, reportUtilities, foodMenuFactory, commonFactory,
foodMenuUtilities, notificationFactory) {
	$scope.toDate = new Date();

	$scope.orderStatus = {
		New: 1,
		Accepted: 2,
		Reduced: 3,
		Delivered: 4,
		Edited: 5,
		Rejected: 6
	};

	$scope.company = {
		Nexcel: '1',
		SMS: '2',
		HM: '3'
	};

	$scope.menuType = {
		All: 0,
		SideDish: 2,
		HasMainDish: 3,
		HasSideDish: 4
	};

	$scope.filter = {
		fromDate: new Date($scope.toDate.getFullYear(), $scope.toDate.getMonth(), 1),
		toDate: new Date(),
		orderBy: 0,
		employeeMenu: 0,
		orderPlatform: 0,
		mealId: '',
		statusId: $scope.orderStatus.Delivered + ',' + $scope.orderStatus.Edited,
		isPagingCallback: false,
		startIndex: 0,
		itemsPerPage: 50
	};

	var setDefaultStatus = function (orderStatuses) {
		angular.forEach(orderStatuses, function (status) {
			if (status.Value === $scope.orderStatus.Delivered || status.Value === $scope.orderStatus.Edited) {
				status.ticked = true;
			}
		});
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
	** Reset paging option.
	*/
	var resetPagingOption = function (filter) {
		filter.startIndex = 0;
		filter.itemsPerPage = 50;
		filter.isPagingCallback = false;
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
		setDefaultStatus($scope.orderStatusesOptions);
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

	/*
	** If SMS employee, set default of menu is Has Side Dish (Charged).Otherwise, default value is All.
	*/
    reportFactory.getCompanyOfCurrentUser().success(function (companyId) {
		switch (companyId) {
			case $scope.company.SMS:
				$scope.filter.employeeMenu = $scope.menuType.HasSideDish;
				break;
			default:
				$scope.filter.employeeMenu = $scope.menuType.All;
		}

		$scope.getEmployeeOrderHistoryReport($scope.filter);
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

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
		resetPagingOption(filter);

		filter.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
		filter.statusId = getOrderStatusId($scope.orderStatusesOptions);

		var fromDate = new Date(filter.fromDate),
			toDate = new Date(filter.toDate);

		if (fromDate > toDate) {
			notificationFactory.error("DateRangeInvalid");
		}
		else {
			$scope.getEmployeeOrderHistoryReport(filter);
		}
	};

	/*
	** Export excel.
	*/
	$scope.exportExcel = function (filter) {
		filter.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
		filter.statusId = getOrderStatusId($scope.orderStatusesOptions);

		$scope.criteria = {
			fromDate: filter.fromDate.toDateString(),
			toDate: filter.toDate.toDateString(),
			orderBy: filter.orderBy,
			employeeMenu: filter.employeeMenu,
			orderPlatform: filter.orderPlatform,
			mealId: filter.mealId,
			statusId: filter.statusId
		};

		$scope.$broadcast('exportEmployeeOrderHistoryReport');
	};
} ]);