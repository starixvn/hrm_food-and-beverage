'use strict';
/*global reportModule*/

reportModule.controller('DailyMainDishReportController', dailyMainDishReportController);
dailyMainDishReportController.$inject = ['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'userControlFactory',
    'foodMenuFactory', 'commonFactory', 'foodMenuUtilities', 'notificationFactory', 'groupControlFactory'];

function dailyMainDishReportController($scope, reportFactory, ErrorFactory, reportUtilities, userControlFactory,
    foodMenuFactory, commonFactory, foodMenuUtilities, notificationFactory, groupControlFactory) {
    var toDay = new Date(),
        fromDate = new Date(toDay.getFullYear(), toDay.getMonth(), 1),
        toDate = new Date();

    $scope.filter = {
        fromDate: fromDate,
        toDate: toDate,
        orderCount: 0,
        companyId: '',
        employeeTypeId: '',
        teamId: '',
        mealId: '',
        userId: '',
        query: '',
        startIndex: 0,
        itemsPerPage: 50,
        isExportReport: false,
        isPagingCallback: false
    };

    $scope.gridOption = {
        itemsPerPage: 50,
        currentPage: 0,
        startIndex: 0
    };

    $scope.calendar = {
        startDateOpened: false,
        endDateOpened: false
    };

    $scope.dateFormat = reportUtilities.getDateFormatDailyReport();
    $scope.numberFormat = reportUtilities.getNumberFormat();
    $scope.originalCompanyId = '';
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

    $scope.openPopupStartDate = function ($event) {
        commonFactory.openPopupStartDate($event, $scope);
    };

    $scope.openPopupEndDate = function ($event) {
        commonFactory.openPopupEndDate($event, $scope);
    };

    var getCompanyFilter = function (companies) {
        var companyId = '';

        if (companies) {
            if (companies.length === 1) {
                companyId = companies[0].CompanyId;
            }
            else {
                angular.forEach(companies, function (company) {
                    companyId = companyId ? companyId + ',' + company.CompanyId : company.CompanyId;
                });
            }
        }

        $scope.filter.companyId = companyId;
        $scope.originalCompanyId = companyId;
    };

    reportFactory.getCompanyByUserRole().success(function (companies) {
        $scope.companyOptions = companies;
        getCompanyFilter(companies);
        $scope.getDailyMainDishReport($scope.filter);

        if (companies && companies.length === 1) {
            $scope.getTeamByCompany($scope.filter.companyId);
        }

        getEmployeeType($scope.filter.companyId, null);
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    $scope.getDailyMainDishReport = function (filter) {
        commonFactory.blockUI();
        reportFactory.getDailyMainDishReport(filter).success(function (report) {
            $scope.report = report;
            $scope.$broadcast("renderDailyReport");

            commonFactory.unblockUI();
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);

            commonFactory.unblockUI();
        });
    };

    $scope.getTeamByCompany = function (companyId) {
        if (companyId) {
            userControlFactory.getAllTeamByCompany(companyId).success(function (teams) {
                $scope.teamOptions = teams;
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });
        }
        else {
            $scope.teamOptions = '';
        }
    };

    $scope.getEmployee = function () {
        var companyId = $scope.filter.companyId;

        if (!companyId) {
            companyId = $scope.originalCompanyId;
        }

        return groupControlFactory.getEmployees($scope.searchEmployee, companyId, null, true);
    };

    $scope.unSelectEmployee = function () {
        if ($scope.searchEmployee.trim() === '') {
            if ($scope.filter && $scope.filter.employee) {
                $scope.filter.employee = null;
            }
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
        $scope.getTeamByCompany(companyId);

        if (!companyId) {
            companyId = $scope.originalCompanyId;
        }

        getEmployeeType(companyId, null);
    };

    var checkDateRangeInYear = function (fromDate, toDate) {
        fromDate.setFullYear(fromDate.getFullYear() + 1);
        fromDate.setHours(0, 0, 0, 0);
        toDate.setHours(0, 0, 0, 0);

        if (+fromDate < toDate) {
            return false;
        }

        return true;
    };

    $scope.query = function (filter) {
        resetGridOption();
        resetPagingOption(filter);

        filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
        filter.teamId = reportUtilities.getTeamId($scope.teamOptions);
        filter.userId = filter.employee ? filter.employee.UserId : '';

        if (!filter.companyId) {
            filter.companyId = $scope.originalCompanyId;
        }

        var fromDate = new Date(filter.fromDate),
            toDate = new Date(filter.toDate);

        if (fromDate > toDate) {
            notificationFactory.error("DateRangeInvalid");
        }
        else if (!checkDateRangeInYear(angular.copy(fromDate), angular.copy(toDate))) {
            notificationFactory.error("DateRangeOneYearWarning");
        }
        else {
            $scope.getDailyMainDishReport(filter);
        }
    };

    $scope.exportExcel = function (filter) {
        filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
        filter.teamId = reportUtilities.getTeamId($scope.teamOptions);
        filter.userId = filter.employee ? filter.employee.UserId : '';

        $scope.criteria = {
            fromDate: filter.fromDate.toDateString(),
            toDate: filter.toDate.toDateString(),
            orderCount: filter.orderCount,
            companyId: filter.companyId,
            employeeTypeId: filter.employeeTypeId,
            teamId: filter.teamId,
            mealId: filter.mealId,
            userId: filter.userId,
            query: filter.query
        };

        $scope.$broadcast('exportDailyMainDishReport');
    };
}