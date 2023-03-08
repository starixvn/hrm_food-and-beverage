'use strict';
/*global reportModule*/

reportModule.controller('MonthlyMainDishReportPerDayController', monthlyMainDishReportPerDayController);
monthlyMainDishReportPerDayController.$inject = ['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'userControlFactory',
    'foodMenuFactory', 'foodMenuUtilities', 'commonFactory'];

function monthlyMainDishReportPerDayController($scope, reportFactory, ErrorFactory, reportUtilities, userControlFactory,
    foodMenuFactory, foodMenuUtilities, commonFactory) {
    $scope.filter = {
        reportDate: new Date(),
        companyId: '',
        employeeTypeId: '',
        teamId: '',
        groupBy: 0,
        workDay: '',
        startIndex: 0,
        isExportReport: false,
        isPagingCallback: false
    };

    $scope.originalCompanyId = '';

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
            }
            else {
                angular.forEach(companies, function (company) {
                    companyId = companyId ? companyId + ',' + company.companyId : company.companyId;
                });
            }
        }

        $scope.filter.companyId = companyId;
        $scope.originalCompanyId = companyId;
    };

    reportFactory.getCompanyByUserRole().success(function (companies) {
        $scope.companyOptions = companies;
        getCompanyFilter(companies);
        $scope.getMonthlyMainDishReportPerDay($scope.filter);

        if (companies && companies.length === 1) {
            $scope.getOrgChart($scope.filter.companyId);
        }

        getEmployeeType($scope.filter.companyId, false);
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
            companyId = $scope.originalCompanyId;
        }

        getEmployeeType(companyId, false);
    };

    reportFactory.getDayTypeEnum().success(function (dayTypes) {
        $scope.dayTypeOptions = dayTypes;
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    $scope.getMonthlyMainDishReportPerDay = function (filter) {
        commonFactory.blockUI();
        reportFactory.getMonthlyMainDishReportPerDay(filter).success(function (report) {
            $scope.report = report;
            $scope.$broadcast('renderMonthlyMainDishPerDay');

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
        filter.isPagingCallback = false;

        if (!filter.companyId) {
            filter.companyId = $scope.originalCompanyId;
        }

        $scope.getMonthlyMainDishReportPerDay(filter);
    };

    $scope.exportExcel = function (filter) {
        filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);

        $scope.criteria = {
            reportDate: filter.reportDate.toDateString(),
            companyId: filter.companyId,
            employeeTypeId: filter.employeeTypeId,
            teamId: filter.teamId,
            workDay: filter.workDay,
            groupBy: filter.groupBy
        };

        $scope.$broadcast('exportMonthlyMainDishPerDayReport');
    };
}