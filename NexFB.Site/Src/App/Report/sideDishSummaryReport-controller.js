'use strict';
/*global reportModule*/

reportModule.controller('SideDishSummaryReportController',
    ['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'userControlFactory', 'orderingFactory',
        'commonFactory', 'foodMenuFactory', 'foodMenuUtilities', 'notificationFactory',
        function ($scope, reportFactory, ErrorFactory, reportUtilities, userControlFactory, orderingFactory,
            commonFactory, foodMenuFactory, foodMenuUtilities, notificationFactory) {
            $scope.history = reportFactory.getHistorySideDishSummary();

            $scope.filter = {
                fromDate: $scope.history.fromDate,
                toDate: $scope.history.toDate,
                companyId: '',
                teamId: $scope.history.teamId,
                employeeTypeId: $scope.history.employeeTypeId,
                employee: $scope.history.employee,
                pageIndex: $scope.history.pageIndex,
                itemsPerPage: $scope.history.itemsPerPage
            };

            $scope.numberFormat = reportUtilities.getNumberFormat();
            $scope.dateFormat = reportUtilities.getDateFormatEmployeeOrderHistoryReport();
            $scope.originalCompanyId = '';
            $scope.teamOptions = [];

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

            /*
            ** Get all company.
            */
            reportFactory.getCompanyByUserRole().success(function (companies) {
                $scope.companyOptions = companies;
                getCompanyFilter(companies);
                if ($scope.history.companyId > 0 || $scope.history.employee.UserId) {
                    $scope.filter.companyId = $scope.history.companyId;
                    if ($scope.history.employee && $scope.history.employee.UserId) {
                        $scope.filter.employee = $scope.history.employee;
                        $scope.searchEmployee = $scope.history.employee.NickName + ' (' + $scope.history.employee.FullName + ')';
                    }
                    $scope.getSideDishSummaryReport($scope.filter);
                    $scope.getTeamByCompany($scope.filter.companyId);
                    getEmployeeType($scope.filter.companyId, null);
                } else {
                    $scope.getSideDishSummaryReport($scope.filter);

                    if (companies && companies.length === 1) {
                        $scope.getTeamByCompany($scope.filter.companyId);
                    }

                    getEmployeeType($scope.filter.companyId, null);
                }
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            /*
            ** Get side dish summary report
            */
            $scope.getSideDishSummaryReport = function (filter) {
                reportFactory.getSideDishSummaryReport(filter).success(function (report) {
                    $scope.report = report;
                    $scope.$broadcast("renderSideDishSummaryReport");
                }).error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                });
            };

            /*
            ** Get all team of company.
            */
            $scope.getTeamByCompany = function (companyId) {
                if (companyId > 0) {
                    userControlFactory.getAllTeamByCompany(companyId).success(function (teams) {
                        $scope.teamOptions = teams;
                        if ($scope.history.teamId.length > 0) {
                            var ids = $scope.history.teamId.split(',');
                            angular.forEach($scope.teamOptions, function (team) {
                                angular.forEach(ids, function (id) {
                                    if (team.TeamId === parseInt(id)) {
                                        team.ticked = true;
                                    }
                                });
                            });
                        }
                    }).error(function (response, status) {
                        ErrorFactory.errorHandle(status);
                    });
                }
                else {
                    $scope.teamOptions = [];
                }
            };

            /*
            ** Get all employees.
            */
            $scope.getEmployee = function () {
                var companyId = $scope.filter.companyId;

                if (!companyId) {
                    companyId = $scope.originalCompanyId;
                }

                return orderingFactory.getEmployees($scope.searchEmployee, companyId, $scope.filter.teamId, true);
            };

            $scope.unSelectEmployee = function () {
                if ($scope.searchEmployee.trim() === '') {
                    if ($scope.filter && $scope.filter.employee) {
                        $scope.filter.employee = null;
                        $scope.history.employee = {};
                    }
                }
            };

            /*
            ** Get all employee type.
            */
            var getEmployeeType = function (companyId, isLocal) {
                foodMenuFactory.getAllEmployeeType(companyId, isLocal).success(function (employeeTypes) {
                    $scope.employeeTypesOption = employeeTypes;
                    if ($scope.history.employeeTypeId.length > 0) {
                        var ids = $scope.history.employeeTypeId.split(',');
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

            /*
            ** Get team and employee type when company change.
            */
            $scope.getTeamAndEmployeeTypeOfCompany = function (companyId) {
                $scope.history.companyId = companyId;
                $scope.history.teamId = '';
                $scope.history.employeeTypeId = '';

                $scope.getTeamByCompany(companyId);

                if (!companyId) {
                    companyId = $scope.originalCompanyId;
                }

                getEmployeeType(companyId, null);
            };

            /*
            ** Filter report.
            */
            $scope.query = function (filter) {
                filter.pageIndex = 0;
                filter.itemsPerPage = 50;

                filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
                filter.teamId = reportUtilities.getTeamId($scope.teamOptions);
                reportFactory.setHistorySideDishSummary($scope);

                if (!filter.companyId) {
                    filter.companyId = $scope.originalCompanyId;
                }

                var fromDate = new Date(filter.fromDate),
                    toDate = new Date(filter.toDate),
                    maxDate = new Date(fromDate.getFullYear() + 1, fromDate.getMonth(), fromDate.getDate());

                if (fromDate > toDate) {
                    notificationFactory.error("DateRangeInvalid");
                }
                else if (toDate > maxDate) {
                    notificationFactory.error("DateRangeOneYearWarning");
                }
                else {
                    $scope.getSideDishSummaryReport(filter);
                }
            };

            /*
            **  Export excel
            */
            $scope.exportExcel = function (filter) {
                filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
                filter.teamId = reportUtilities.getTeamId($scope.teamOptions);

                $scope.criteria = {
                    companyId: filter.companyId,
                    employeeTypeId: filter.employeeTypeId,
                    teamId: filter.teamId,
                    userId: filter.employee ? filter.employee.UserId : '',
                    fromDate: filter.fromDate.toDateString(),
                    toDate: filter.toDate.toDateString(),
                    pageIndex: null,
                    itemsPerPage: null
                };

                $scope.$broadcast('exportSideDishSummaryReport');
            };
        }]);