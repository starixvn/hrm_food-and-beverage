'use strict';
/*global reportModule*/

reportModule.controller('MealForecastDetailReportController',
['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'userControlFactory', 'commonFactory', 'notificationFactory',
function ($scope, reportFactory, ErrorFactory, reportUtilities, userControlFactory, commonFactory, notificationFactory) {
    $scope.filter = {
        date: '',
        departmentId: '',
        dietType: ''
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
        $scope.getMealForecastDetailReport($scope.filter);
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    /*
    ** Get report
    */
    $scope.getMealForecastReport = function (filter) {
        reportFactory.getMealForecastDetailReport(filter).success(function (report) {
            $scope.report = report;
            $scope.$broadcast("renderMealForecastDetailReport");
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

        $scope.getMealForecastReport(filter);
    };

    /*
    **  Export excel
    */
    $scope.exportExcel = function (filter) {
        $scope.criteria = {
            date: filter.date,
            departmentId: filter.departmentId,
            isVegetarian: filter.dietType === '' ? '' : filter.dietType === '0' ? false : true
        };
        debugger;
        $scope.$broadcast('exportMealForecastDetailReport');
    };
} ]);