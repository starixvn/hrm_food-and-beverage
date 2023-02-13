'use strict';
/*global reportModule*/

reportModule.controller('MonthlyMainDishReportLocalController',
    ['$scope', 'reportFactory', 'ErrorFactory', 'userControlFactory', 'foodMenuFactory', 'groupControlFactory', 'foodMenuUtilities',
        'reportUtilities',
        function ($scope, reportFactory, ErrorFactory, userControlFactory, foodMenuFactory, groupControlFactory, foodMenuUtilities,
            reportUtilities) {

            /*
            ** Declare variable.
            */
            var currentDate = new Date();
            $scope.searchEmployee = '';

            $scope.department = {
                Nexcel: '1',
                SMS: '2',
                HM: '3',
                Luminet: '4'
            };

            $scope.filter = {
                reportDate: currentDate,
                departmentId: '',
                userId: null,
                mealId: '',
                employeeTypeId: '',
                teamId: '',
                groupBy: 0,
                startIndex: 0,
                itemsPerPage: 50,
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
            $scope.teamOptions = [];

            /*
            ** Get all employees.
            */
            $scope.getEmployee = function () {
                var departmentId = $scope.department.Nexcel + "," + $scope.department.SMS + "," + $scope.department.HM + "," + $scope.department.Luminet;

                return groupControlFactory.getEmployees($scope.searchEmployee, departmentId, null, true);
            };

            $scope.unSelectEmployee = function () {
                if ($scope.searchEmployee.trim() === '') {
                    if ($scope.filter && $scope.filter.employee) {
                        $scope.filter.employee = null;
                    }
                }
            };

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
            ** Get emp shift in month
            */


            /*
            ** Get all department.
            */
            reportFactory.getDepartmentByUserRole().success(function (departments) {
                $scope.departmentOptions = departments;
                getDepartmentFilter(departments);
                $scope.getMonthlyReportLocal($scope.filter);

                if (departments && departments.length === 1) {
                    $scope.getTeamByCompany($scope.filter.departmentId);
                }

                getEmployeeType($scope.filter.departmentId, null);
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            /*
            ** Get all team of department.
            */
            $scope.getTeamByCompany = function (departmentId) {
                if (departmentId) {
                    userControlFactory.getAllTeamByCompany(departmentId).success(function (teams) {
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
            ** Get all meal timesheet.
            */
            foodMenuFactory.getAllMealTimesheet().success(function (meals) {
                $scope.mealsOption = meals;
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            /*
            ** Get all employee type.
            */
            var getEmployeeType = function (departmentId, isLocal) {
                foodMenuFactory.getAllEmployeeType(departmentId, isLocal).success(function (employeeTypes) {
                    $scope.employeeTypesOption = employeeTypes;
                }).error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                });
            };

            /*
            ** 
            */
            $scope.getTeamAndEmployeeTypeOfDepartment = function (departmentId) {
                $scope.getTeamByCompany(departmentId);

                if (!departmentId) {
                    departmentId = $scope.originalDepartmentId;
                }

                getEmployeeType(departmentId, null);
            };

            /*
            ** Get monthly main dish report by local.
            */
            $scope.getMonthlyReportLocal = async function (filter) {
                reportFactory.getEmployeeShiftInMonth(filter).success(function (result) {
                    $scope.employeeShift = result;
                }).error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                });
                setTimeout(() => {
                    reportFactory.getMonthlyMainDishReportByLocal(filter)
                        .success(function (report) {
                            $scope.localReport = report;
                            $scope.$broadcast('renderMonthlyReport');

                        })
                        .error(function (response, status) {
                            ErrorFactory.errorHandle(status);
                        });
                }, 500);
            };

            function getShift() {
                return new Promise(resolve => {
                    setTimeout(() => {

                    }, 2000);
                });
            }

            /*
            ** Query data.
            */
            $scope.query = function (filter) {
                resetGridOption();
                resetPagingOption(filter);
                filter.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
                filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
                filter.teamId = reportUtilities.getTeamId($scope.teamOptions);
                filter.userId = filter.employee ? filter.employee.UserId : '';

                if (!filter.departmentId) {
                    filter.departmentId = $scope.originalDepartmentId;
                }

                $scope.getMonthlyReportLocal(filter);
            };

            /*
            ** Export excel.
            */
            $scope.exportExcel = function (filter) {
                filter.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
                filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
                filter.teamId = reportUtilities.getTeamId($scope.teamOptions);
                filter.userId = filter.employee ? filter.employee.UserId : '';

                $scope.criteria = {
                    reportDate: filter.reportDate.toDateString(),
                    departmentId: filter.departmentId,
                    userId: filter.userId,
                    employeeTypeId: filter.employeeTypeId,
                    teamId: filter.teamId,
                    mealId: filter.mealId,
                    groupBy: filter.groupBy,
                    isLocal: 1
                };

                $scope.$broadcast('exportMonthlyMainDishLocalReport');
            };
        }]);