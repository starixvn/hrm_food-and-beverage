'use strict';
/*global reportModule*/

reportModule.controller('MealForecastReportController',
['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'commonFactory', 'notificationFactory',
function ($scope, reportFactory, ErrorFactory, reportUtilities, commonFactory, notificationFactory) {
    $scope.toDate = new Date();
    $scope.mealForecastFilter = reportFactory.getMealForecastFilter();

    $scope.filter = {
        fromDate: $scope.mealForecastFilter.fromDate,
        toDate: $scope.mealForecastFilter.toDate,
        departmentId: $scope.mealForecastFilter.departmentId,
        vegetarianType: $scope.mealForecastFilter.vegetarianType,
        isPagingCallback: $scope.mealForecastFilter.isPagingCallback,
        startIndex: $scope.mealForecastFilter.startIndex,
        itemsPerPage: $scope.mealForecastFilter.itemsPerPage
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
        itemsPerPage: $scope.mealForecastFilter.itemsPerPage,
        currentPage: $scope.mealForecastFilter.currentPage,
        startIndex: $scope.mealForecastFilter.startIndex
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

        $scope.filter.departmentId = departmentId;
    };

    /*
    ** Get all department.
    */
    reportFactory.getDepartmentByUserRole().success(function (departments) {
        $scope.departmentOptions = departments;
        getDepartmentFilter(departments);
        $scope.filter.departmentId = parseInt($scope.mealForecastFilter.departmentId);
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
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    $scope.history = function () {
        reportFactory.setMealForecastFilter($scope);
    };

    /*
    ** Filter report.
    */
    $scope.query = function (filter) {
        resetGridOption();
        resetPagingOption(filter);
        $scope.history();

        var fromDate = new Date(filter.fromDate),
			toDate = new Date(filter.toDate);

        if (fromDate > toDate) {
            notificationFactory.error("DateRangeInvalid");
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
            isVegetarian: filter.vegetarianType === "" ? "" : filter.vegetarianType === '0' ? false : true
        };
        $scope.$broadcast('exportMealForecastReport');
    };
} ]);