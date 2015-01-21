'use strict';
/*global reportModule*/

reportModule.controller('MealForecastReportController',
['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'commonFactory', 'notificationFactory',
function ($scope, reportFactory, ErrorFactory, reportUtilities, commonFactory, notificationFactory) {
    $scope.toDate = new Date();

    $scope.filter = {
        fromDate: new Date($scope.toDate.getFullYear(), $scope.toDate.getMonth(), 1),
        toDate: new Date(),
        departmentId: '',
        vegetarianType: '',
        isPagingCallback: false,
        startIndex: 0,
        itemsPerPage: 50
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
    };

    /*
    ** Get all department.
    */
    reportFactory.getDepartmentByUserRole().success(function (departments) {
        $scope.departmentOptions = departments;
        getDepartmentFilter(departments);
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
            $scope.$broadcast("mealForecastDetail");
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    /*
    ** Filter report.
    */
    $scope.query = function (filter) {
        resetGridOption();
        resetPagingOption(filter);

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