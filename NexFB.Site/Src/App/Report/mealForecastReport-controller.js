'use strict';
/*global reportModule*/

reportModule.controller('MealForecastReportController',
    ['$scope', 'baseUrl', 'reportFactory', 'foodMenuFactory', 'foodMenuUtilities', 'ErrorFactory', 'reportUtilities', 'commonFactory', 'notificationFactory',
        function ($scope, baseUrl, reportFactory, foodMenuFactory, foodMenuUtilities, ErrorFactory, reportUtilities, commonFactory, notificationFactory) {
            $scope.toDate = new Date();
            $scope.history = reportFactory.getHistory();

            $scope.filter = {
                fromDate: $scope.history.fromDate,
                toDate: $scope.history.toDate,
                companyId: $scope.history.companyId,
                employeeTypeId: $scope.history.employeeTypeId,
                vegetarianType: $scope.history.vegetarianType,
                isPagingCallback: $scope.history.isPagingCallback,
                startIndex: $scope.history.startIndex,
                itemsPerPage: $scope.history.itemsPerPage
            };

            $scope.baseUrl = baseUrl;
            $scope.numberFormat = reportUtilities.getNumberFormat();
            $scope.dateFormat = reportUtilities.getDateFormatEmployeeOrderHistoryReport();
            $scope.originalCompanyId = '';

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
                itemsPerPage: $scope.history.itemsPerPage,
                currentPage: $scope.history.currentPage,
                startIndex: $scope.history.startIndex
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

            var getCompanyFilter = function (companies) {
                var companyId = '';

                if (companies.length === 1) {
                    companyId = companies[0].CompanyId;
                }
                else {
                    angular.forEach(companies, function (company) {
                        companyId = companyId ? companyId + ',' + company.CompanyId : company.CompanyId;
                    });
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

                if ($scope.history.isCallBack) {
                    $scope.filter.companyId = angular.isNumber($scope.history.companyId) ? parseInt($scope.history.companyId) : $scope.history.companyId;
                }

                $scope.getMealForecastReport($scope.filter);
                getEmployeeType($scope.filter.companyId, null);
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            /*
            ** Get all employee type.
            */
            var getEmployeeType = function (companyId, isLocal) {
                foodMenuFactory.getAllEmployeeType(companyId, isLocal).success(function (employeeTypes) {
                    $scope.employeeTypesOption = employeeTypes;
                    if ($scope.history.isCallBack && $scope.history.employeeTypeId.length > 0) {
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
            ** Get employee type when company change.
            */
            $scope.getEmployeeTypeOfCompany = function (companyId) {
                $scope.history.isCallBack = false;
                $scope.history.employeeTypeId = '';

                if (!companyId) {
                    $scope.filter.companyId = $scope.originalCompanyId;
                    companyId = $scope.originalCompanyId;
                }

                getEmployeeType(companyId, null);
            };

            /*
            ** Get report
            */
            $scope.getMealForecastReport = function (filter) {
                reportFactory.getMealForecastReport(filter).success(function (report) {
                    $scope.report = report;
                    $scope.$broadcast("renderMealForecastReport");
                    $scope.setHistory();
                }).error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                });
            };

            $scope.setHistory = function () {
                var companyId = $scope.filter.companyId;

                if (!companyId) {
                    $scope.filter.companyId = $scope.originalCompanyId;
                }
                reportFactory.setHistory($scope, true);
            };

            /*
            ** Filter report.
            */
            $scope.query = function (filter) {
                resetGridOption();
                resetPagingOption(filter);

                filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);

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
                    companyId: filter.companyId,
                    isVegetarian: filter.vegetarianType.length === 0 ? '' : filter.vegetarianType === '0' ? false : true
                };
                $scope.$broadcast('exportMealForecastReport');
            };
        }]);