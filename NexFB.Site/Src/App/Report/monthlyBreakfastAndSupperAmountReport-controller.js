'use strict';

reportModule.controller('MonthlyBreakfastAndSuperController',
    ['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'userControlFactory', 'commonFactory', 'baseUrl',
        function ($scope, reportFactory, ErrorFactory, reportUtilities, userControlFactory, commonFactory, baseUrl) {
            $scope.filter = {
                reportDate: new Date(),
                departmentId: '',
                startIndex: 0,
                isExportReport: false,
                isPagingCallback: false
            };

            $scope.originalDepartmentId = '';

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

            /*
            ** Open pop-up calendar.
            */
            $scope.open = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();

                $scope.opened = true;
            };

            var getDepartmentFilter = function (departments) {
                var departmentId = '';

                if (departments) {
                    if (departments.length === 1) {
                        departmentId = departments[0].DepartmentId;
                    }
                    else {
                        angular.forEach(departments, function (department) {
                            departmentId = departmentId ? departmentId + ',' + department.DepartmentId : department.DepartmentId;
                        });
                    }
                }

                $scope.filter.departmentId = departmentId;
                $scope.originalDepartmentId = departmentId;
            };

            /*
            ** Get all department.
            */
            reportFactory.getCompanyByUserRole().success(function (departments) {
                $scope.departmentOptions = departments;
                getDepartmentFilter(departments);
                $scope.getMonthlyBreakfastAndSupperReport($scope.filter);
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            /*
            ** Get Day Type.
            */
            reportFactory.getDayTypeEnum().success(function (dayTypes) {
                $scope.dayTypeOptions = dayTypes;
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });
            /*
            ** Get monthly breakfast and supper report.
            */
            $scope.getMonthlyBreakfastAndSupperReport = function (filter) {
                reportFactory.getMonthlyBreakfastAndSupperReport(filter).success(function (report) {
                    $scope.report = report;
                    commonFactory.tablePagination($scope, report, $scope.entrySelected);
                }).error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                });
            };

            /*
            ** Query data.
            */
            $scope.query = function (filter) {
                if (!filter.departmentId) {
                    filter.departmentId = $scope.originalDepartmentId;
                }

                $scope.getMonthlyBreakfastAndSupperReport(filter);
            };
            /*
            ** Export Excel
            */
            $scope.exportExcel = function (filter) {
                var criteria = {
                    departmentId: filter.departmentId,
                    reportDate: filter.reportDate.toDateString()
                }
                var data = $.param(criteria),
                    url = baseUrl + 'ExportReport/ExportMonthlyBreakfastAndSupperAmountReport',
                    downloadUrl = url + "?" + data,
                    iframe = $('body iframe');

                if (iframe.length === 0) {
                    iframe = $('<iframe width="0" height="0" class="display-none"></iframe>');
                    iframe.appendTo('body');
                }

                iframe.attr('src', downloadUrl);
            };
        }]);