'use strict';
/*global reportModule*/

reportModule.controller('MealForecastDetailReportController',
    ['$scope', '$location', '$routeParams', 'baseUrl', 'reportFactory', 'foodMenuFactory', 'foodMenuUtilities', 'ErrorFactory', 'reportUtilities',
        function ($scope, $location, $routeParams, baseUrl, reportFactory, foodMenuFactory, foodMenuUtilities, ErrorFactory, reportUtilities) {
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
            ** Get all company.
            */
            reportFactory.getCompanyByUserRole().success(function (companies) {
                $scope.companyOptions = companies;
                $scope.filter.companyId = $routeParams.companyId.split(',').length > 1 ? $routeParams.companyId : parseInt($routeParams.companyId);
                $scope.originalcompanyId = $scope.filter.companyId;
                $scope.getMealForecastDetailReport($scope.filter);
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

            /*
            ** Get employee type when company change.
            */
            $scope.getEmployeeTypeOfcompany = function (companyId) {
                $scope.filter.employeeTypeId = '';

                if (!companyId) {
                    $scope.filter.companyId = $scope.originalcompanyId;
                    companyId = $scope.originalcompanyId;
                }

                getEmployeeType(companyId, null);
            };

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

                filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);

                $scope.getMealForecastDetailReport(filter);
            };

            /*
            **  Export excel
            */
            $scope.exportExcel = function (filter) {
                $scope.criteria = {
                    onDate: filter.date,
                    companyId: filter.companyId,
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
        }]);