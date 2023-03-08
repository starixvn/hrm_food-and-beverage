'use strict';
/*global reportModule*/

reportModule.controller('MonthlyMainDishReportLocalController', monthlyMainDishReportLocalController);
monthlyMainDishReportLocalController.$inject = ['$scope', 'reportFactory', 'ErrorFactory', 'userControlFactory', 'foodMenuFactory',
    'groupControlFactory', 'foodMenuUtilities', 'reportUtilities', 'commonFactory'];

function monthlyMainDishReportLocalController($scope, reportFactory, ErrorFactory, userControlFactory, foodMenuFactory,
    groupControlFactory, foodMenuUtilities, reportUtilities, commonFactory) {
    var currentDate = new Date();
    $scope.searchEmployee = '';

    $scope.company = {
        Nexcel: '1',
        SMS: '2',
        HM: '3',
        Luminet: '4'
    };

    $scope.filter = {
        reportDate: currentDate,
        companyId: '',
        userId: null,
        mealId: '',
        employeeTypeId: '',
        teamId: null,
        groupBy: 0,
        startIndex: 0,
        itemsPerPage: 50,
        isExportReport: false,
        isPagingCallback: false
    };

    $scope.originalCompanyd = '';

    $scope.format = 'MMM yyyy';
    $scope.popUpMode = 'month';
    $scope.dateOptions = {
        minMode: 'month',
        maxMode: 'month',
        formatMonth: 'MMM'
    };

    $scope.gridOption = {
        itemsPerPage: 50,
        currentPage: 0,
        startIndex: 0
    };

    $scope.numberFormat = reportUtilities.getNumberFormat();
    $scope.monthFormat = reportUtilities.getMonthFormat();
    $scope.teamOptions = [];

    $scope.getEmployee = function () {
        var companyId = $scope.company.Nexcel + "," + $scope.company.SMS + "," + $scope.company.HM + "," + $scope.company.Luminet;

        return groupControlFactory.getEmployees($scope.searchEmployee, companyId, null, true);
    };

    $scope.unSelectEmployee = function () {
        if ($scope.searchEmployee.trim() === '') {
            if ($scope.filter && $scope.filter.employee) {
                $scope.filter.employee = null;
            }
        }
    };

    var resetGridOption = function () {
        $scope.gridOption.itemsPerPage = 50;
        $scope.gridOption.currentPage = 0;
        $scope.gridOption.startIndex = 0;
    };

    var resetPagingOption = function (filter) {
        filter.startIndex = 0;
        filter.itemsPerPage = 50;
        filter.isPagingCallback = false;
    };

    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.opened = true;
    };

    var getCompanyFilter = function (companies) {
        var companyId = '';

        if (companies) {
            if (companies.length === 1) {
                companyId = companies[0].companyId;
            } else {
                angular.forEach(companies, function (company) {
                    companyId = companyId ? companyId + ',' + company.companyId : company.companyId;
                });
            }
        }

        $scope.filter.companyId = companyId;
        $scope.originalCompanyd = companyId;
    };

    reportFactory.getCompanyByUserRole().success(function (companies) {
        $scope.companyOptions = companies;
        getCompanyFilter(companies);
        $scope.getMonthlyReportLocal($scope.filter);

        if (companies && companies.length === 1) {
            $scope.getOrgChart($scope.filter.companyId);
        }

        getEmployeeType($scope.filter.companyId, null);
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    $scope.onSelectTeam = function (teamIds) {
        $scope.filter.teamId = teamIds;
    };

    $scope.getOrgChart = function (companyId) {
        if (companyId) {
            userControlFactory.getOrgChart(companyId).success(function (teams) {
                $scope.teamOptions = teams;
                $('multiselect-treeview-2-selectAll').val = false;
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });
        }
        else {
            $scope.teamOptions = '';
        }
    };

    foodMenuFactory.getAllMealTimesheet().success(function (meals) {
        $scope.mealsOption = meals;
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    var getEmployeeType = function (companyId, isLocal) {
        foodMenuFactory.getAllEmployeeType(companyId, isLocal).success(function (employeeTypes) {
            $scope.employeeTypesOption = employeeTypes;
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    $scope.getTeamAndEmployeeTypeOfCompany = function (companyId) {
        $scope.getOrgChart(companyId);

        if (!companyId) {
            companyId = $scope.originalCompanyd;
        }

        getEmployeeType(companyId, null);
    };

    $scope.getMonthlyReportLocal = async function (filter) {
        commonFactory.blockUI();
        reportFactory.getMonthlyMainDishReportByLocal(filter).success(function (report) {
            $scope.localReport = report;
            $scope.$broadcast('renderMonthlyReport');

            commonFactory.unblockUI();
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);

            commonFactory.unblockUI();
        });
    };

    $scope.query = function (filter) {
        resetGridOption();
        resetPagingOption(filter);
        filter.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
        filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
        filter.userId = filter.employee ? filter.employee.UserId : '';

        if (!filter.companyId) {
            filter.companyId = $scope.originalCompanyd;
        }

        $scope.getMonthlyReportLocal(filter);
    };

    $scope.exportExcel = function (filter) {
        filter.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
        filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
        filter.userId = filter.employee ? filter.employee.UserId : '';

        $scope.criteria = {
            reportDate: filter.reportDate.toDateString(),
            companyId: filter.companyId,
            userId: filter.userId,
            employeeTypeId: filter.employeeTypeId,
            teamId: filter.teamId,
            mealId: filter.mealId,
            groupBy: filter.groupBy,
            isLocal: 1
        };

        $scope.$broadcast('exportMonthlyMainDishLocalReport');
    };
}