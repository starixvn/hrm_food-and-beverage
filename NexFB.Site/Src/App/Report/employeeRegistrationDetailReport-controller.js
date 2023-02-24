'use strict';
/*global reportModule*/

reportModule.controller('EmployeeRegistrationDetailReport',
    ['$scope', '$window', '$routeParams', 'reportFactory', 'ErrorFactory', 'reportUtilities',
        function ($scope, $window, $routeParams, reportFactory, ErrorFactory, reportUtilities) {
            $scope.filter = {
                date: $routeParams.date,
                companyId: $routeParams.companyId,
                employeeTypeId: $routeParams.employeeTypeId,
                mealId: $routeParams.mealId,
                menuId: $routeParams.menuId,
                dietType: $routeParams.dietType
            };

            $scope.numberFormat = reportUtilities.getNumberFormat();
            $scope.dateFormat = reportUtilities.getDateFormatEmployeeOrderHistoryReport();
            $scope.originalCompanyId = '';

            $scope.gridOption = {
                itemsPerPage: 50,
                currentPage: 0,
                startIndex: 0
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
                    date: filter.date,
                    companyId: filter.companyId,
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
        }]);