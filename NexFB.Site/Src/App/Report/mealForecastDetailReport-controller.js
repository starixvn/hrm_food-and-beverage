'use strict';
/*global reportModule*/

reportModule.controller('MealForecastDetailReportController',
['$scope', '$location', '$routeParams', 'reportFactory', 'ErrorFactory', 'reportUtilities',
function ($scope, $location, $routeParams, reportFactory, ErrorFactory, reportUtilities) {
    $scope.filter = {
        date: $routeParams.date,
        departmentId: $routeParams.departmentId,
        dietType: $routeParams.dietType
    };

    $scope.numberFormat = reportUtilities.getNumberFormat();
    $scope.dateFormat = reportUtilities.getDateFormatEmployeeOrderHistoryReport();

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
        $scope.filter.departmentId = $routeParams.departmentId === '' ? '' : parseInt($routeParams.departmentId);
        $scope.getMealForecastDetailReport($scope.filter);
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    /*
    ** Get report
    */
    $scope.getMealForecastDetailReport = function (filter) {
        reportFactory.getMealForecastDetailReport(filter).success(function (report) {
            $scope.report = report;
            $scope.$broadcast("renderMealForecastDetailReport");
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

        $scope.getMealForecastDetailReport(filter);
    };

    /*
    **  Export excel
    */
    $scope.exportExcel = function (filter) {
        $scope.criteria = {
            onDate: filter.date,
            departmentId: filter.departmentId,
            isVegetarian: filter.dietType === '' ? '' : filter.dietType === '0' ? false : true
        };

        $scope.$broadcast('exportMealForecastDetailReport');
    };

    /*
    **  go back
    */
    $scope.goback = function () {
        $location.url('/MealForecastReport/Index');
    };
} ]);