'use strict';
/*global reportModule*/

reportModule.controller('MealForecastDetailReportController', mealForecastDetailReportController);
mealForecastDetailReportController.$inject = ['$scope', '$location', '$routeParams', 'reportFactory', 'foodMenuFactory', 'foodMenuUtilities',
    'ErrorFactory', 'reportUtilities', 'commonFactory', 'baseUrl'];

function mealForecastDetailReportController($scope, $location, $routeParams, reportFactory, foodMenuFactory, foodMenuUtilities,
    ErrorFactory, reportUtilities, commonFactory, baseUrl) {
    $scope.filter = {
        date: $routeParams.date,
        companyId: $routeParams.companyId,
        employeeTypeId: $routeParams.employeeTypeId,
        dietType: $routeParams.dietType
    };

    $scope.numberFormat = reportUtilities.getNumberFormat();
    $scope.dateFormat = reportUtilities.getDateFormatEmployeeOrderHistoryReport();
    $scope.originalcompanyId = '';
    $scope.baseUrl = baseUrl;

    $scope.gridOption = {
        itemsPerPage: 50,
        currentPage: 0,
        startIndex: 0
    };

    var resetGridOption = function () {
        $scope.gridOption = {
            itemsPerPage: 50,
            currentPage: 0,
            startIndex: 0
        };
    };

    var resetPagingOption = function (filter) {
        filter.startIndex = 0;
        filter.itemsPerPage = 50;
        filter.isPagingCallback = false;
    };

    reportFactory.getCompanyByUserRole().success(function (companies) {
        $scope.companyOptions = companies;
        $scope.filter.companyId = $routeParams.companyId.split(',').length > 1 ? $routeParams.companyId : parseInt($routeParams.companyId);
        $scope.originalcompanyId = $scope.filter.companyId;
        $scope.getMealForecastDetailReport($scope.filter);
        getEmployeeType($scope.filter.companyId, null);
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    var getEmployeeType = function (companyId, isLocal) {
        foodMenuFactory.getAllEmployeeType(companyId, isLocal).success(function (employeeTypes) {
            $scope.employeeTypesOption = employeeTypes;
            if ($scope.filter.employeeTypeId.length > 0) {
                var ids = $scope.filter.employeeTypeId.split(',');
                angular.forEach($scope.employeeTypesOption, function (employeeType) {
                    angular.forEach(ids, function (id) {
                        if (employeeType.EmployeeTypeId === parseInt(id)) {
                            employeeType.ticked = true;
                        }
                    });
                });
            }
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    $scope.getEmployeeTypeOfcompany = function (companyId) {
        $scope.filter.employeeTypeId = '';

        if (!companyId) {
            $scope.filter.companyId = $scope.originalcompanyId;
            companyId = $scope.originalcompanyId;
        }

        getEmployeeType(companyId, null);
    };

    $scope.getMealForecastDetailReport = function (filter) {
        commonFactory.blockUI();
        reportFactory.getMealForecastDetailReport(filter).success(function (report) {
            $scope.report = report;
            $scope.$broadcast("renderMealForecastDetailReport");

            commonFactory.unblockUI();
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);

            commonFactory.unblockUI();
        });
    };

    $scope.query = function (filter) {
        resetGridOption();
        resetPagingOption(filter);

        filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);

        $scope.getMealForecastDetailReport(filter);
    };

    $scope.exportExcel = function (filter) {
        $scope.criteria = {
            date: filter.date,
            companyId: filter.companyId,
            isVegetarian: filter.dietType === '' ? '' : filter.dietType === '0' ? false : true
        };

        $scope.$broadcast('exportMealForecastDetailReport');
    };

    $scope.goback = function () {
        $location.url('/MealForecastReport/Index');
    };
}