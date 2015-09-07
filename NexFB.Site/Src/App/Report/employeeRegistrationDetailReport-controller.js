'use strict';
/*global reportModule*/

reportModule.controller('EmployeeRegistrationDetailReport',
['$scope', '$window', '$location', '$routeParams', 'reportFactory', 'foodMenuFactory', 'foodMenuUtilities', 'ErrorFactory', 'reportUtilities',
function ($scope, $window, $location, $routeParams, reportFactory, foodMenuFactory, foodMenuUtilities, ErrorFactory, reportUtilities) {
    $scope.filter = {
        date: $routeParams.date,
        departmentId: $routeParams.departmentId,
        employeeTypeId: $routeParams.employeeTypeId,
        mealId: $routeParams.mealId,
        menuId: $routeParams.menuId,
        dietType: $routeParams.dietType
    };

    $scope.numberFormat = reportUtilities.getNumberFormat();
    $scope.dateFormat = reportUtilities.getDateFormatEmployeeOrderHistoryReport();
    $scope.originalDepartmentId = '';

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

    /*
    ** Get report
    */
    $scope.getEmployeeRegistrationDetailReport = function (filter) {
        reportFactory.getEmployeeRegistrationDetailReport(filter).success(function (report) {
            $scope.report = report;
            $scope.$broadcast("renderEmployeeRegistrationDetailReport");
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };
    $scope.getEmployeeRegistrationDetailReport($scope.filter);
    /*
    **  Export excel
    */
        $scope.exportExcel = function (filter) {
            $scope.criteria = {
                workDate: filter.date,
                departmentId: filter.departmentId,
                employeeTypeId: filter.employeeTypeId,
                mealId: filter.mealId,
                menuId: filter.menuId,
                isVegetarian: filter.dietType.length === 0 ? '' : filter.dietType === '0' ? false : true
            };

            $scope.$broadcast('exportEmployeeRegistrationDetailReport');
        };

    /*
    **  go back
    */
    $scope.goback = function () {
        $window.history.back();
    };
} ]);