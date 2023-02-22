'use strict';
/*global reportModule*/

reportModule.controller('MonthlyMainDishReportPerDayController',
    ['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'userControlFactory', 'foodMenuFactory',
        'foodMenuUtilities',
        function ($scope, reportFactory, ErrorFactory, reportUtilities, userControlFactory, foodMenuFactory,
            foodMenuUtilities) {

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
            ** Open pop-up calendar.
            */
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

            /*
            ** Get all company.
            */
            reportFactory.getCompanyByUserRole().success(function (companies) {
                $scope.companyOptions = companies;
                getCompanyFilter(companies);
                $scope.getMonthlyMainDishReportPerDay($scope.filter);

                if (companies && companies.length === 1) {
                    $scope.getTeamByCompany($scope.filter.companyId);
                }

                getEmployeeType($scope.filter.companyId, false);
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            /*
            ** Get all team of company.
            */
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

            /*
            ** Get all employee type.
            */
            var getEmployeeType = function (companyId, isLocal) {
                foodMenuFactory.getAllEmployeeType(companyId, isLocal).success(function (employeeTypes) {
                    $scope.employeeTypesOption = employeeTypes;
                }).error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                });
            };

            /*
            ** Get team and employee type by company.
            */
            $scope.getTeamAndEmployeeTypeOfCompany = function (companyId) {
                $scope.getTeamByCompany(companyId);

                if (!companyId) {
                    companyId = $scope.originalCompanyId;
                }

                getEmployeeType(companyId, false);
            };

            /*
            ** Get Day Type.
            */
            reportFactory.getDayTypeEnum().success(function (dayTypes) {
                $scope.dayTypeOptions = dayTypes;
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            /*
            ** Get monthly main dish per day report.
            */
            $scope.getMonthlyMainDishReportPerDay = function (filter) {
                reportFactory.getMonthlyMainDishReportPerDay(filter).success(function (report) {
                    $scope.report = report;
                    $scope.$broadcast('renderMonthlyMainDishPerDay');
                }).error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                });
            };

            /*
            ** Query data.
            */
            $scope.query = function (filter) {
                resetGridOption();
                resetPagingOption(filter);
                filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
                filter.teamId = reportUtilities.getTeamId($scope.teamOptions);
                filter.isPagingCallback = false;

                if (!filter.companyId) {
                    filter.companyId = $scope.originalCompanyId;
                }

                $scope.getMonthlyMainDishReportPerDay(filter);
            };

            /*
            ** Export excel.
            */
            $scope.exportExcel = function (filter) {
                filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
                filter.teamId = reportUtilities.getTeamId($scope.teamOptions);

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
        }]);