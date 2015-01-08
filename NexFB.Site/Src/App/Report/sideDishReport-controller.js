'use strict';
/*global reportModule*/

reportModule.controller('SideDishReportController',
['$scope', 'reportFactory', 'notificationFactory', 'ErrorFactory', 'userControlFactory', 'foodMenuFactory', 'foodMenuUtilities',
'reportUtilities', 'sideDishManagementFactory',
function ($scope, reportFactory, notificationFactory, ErrorFactory, userControlFactory, foodMenuFactory, foodMenuUtilities,
reportUtilities, sideDishManagementFactory) {
    var self = this;
    var date = new Date(),
        year = date.getFullYear(),
        month = date.getMonth(),
        day = date.getDate();

    $scope.departments = [];
    $scope.teams = [];
    $scope.employeeTypes = [];
    $scope.sideDishItems = [];

    $scope.criteria = {
        departmentId: 0,
        teamIds: '',
        employeeTypeIds: '',
        fromDate: new Date(year, month, 1),
        toDate: new Date(year, month, day),
        sideDishIds: ''
    };

    $scope.fromDateOptions = {
        minDate: null,
        maxDate: null
    };

    $scope.toDateOptions = {
        minDate: null,
        maxDate: null
    };

    $scope.fromOpened = false;
    $scope.toOpened = false;

    $scope.open = function ($event, dateType) {
        $event.preventDefault();
        $event.stopPropagation();

        if (dateType === "from") {
            $scope.fromOpened = true;
        } else if (dateType === "to") {
            $scope.toOpened = true;
        }
    };

    $scope.getTeamAndEmployeeType = function (departmentId) {
        self.getTeams(departmentId);
        self.getEmployeeTypes(departmentId);
    };

    $scope.search = function () {
        var isValid = self.validateFilterOptions();

        if (isValid) {

        }
    };

    self.getTeams = function (departmentId) {
        if (departmentId === -1) {
            departmentId = null;
        }

        userControlFactory.getAllTeamByCompany(departmentId).success(function (teams) {
            $scope.teams = teams;
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    self.getEmployeeTypes = function (departmentId) {
        if (departmentId === -1) {
            departmentId = null;
        }

        foodMenuFactory.getAllEmployeeType(departmentId).success(function (employeeTypes) {
            $scope.employeeTypes = employeeTypes;
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    self.getAllSideDish = function () {
        sideDishManagementFactory.getAllSideDish().success(function (sideDishes) {
            $scope.sideDishItems = sideDishes;
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    self.getDepartment = function (callback) {
        reportFactory.getDepartmentByUserRole().success(function (departments) {
            if (departments.length > 1) {
                departments.splice(0, 0, { DepartmentId: -1, Name: '-- All --' });
            }

            $scope.departments = departments;

            if (departments.length > 0) {
                $scope.criteria.departmentId = departments[0].DepartmentId;
            }

            if (angular.isFunction(callback)) {
                callback();
            }

        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    self.loadData = function () {
        self.getDepartment(function () {
            $scope.getTeamAndEmployeeType($scope.criteria.departmentId);
        });

        self.getAllSideDish();

        //todo: get report data
    };

    self.validateFilterOptions = function () {        
        var isValid = true;
        var fromDate = $scope.criteria.fromDate;
        var toDate = $scope.criteria.toDate;
        var maxDate = new Date(fromDate.getFullYear() + 1, fromDate.getMonth(), fromDate.getDate());

        if (fromDate > toDate) {
            isValid = false;
            notificationFactory.error("ValidateFromToDate");
        }
        else if (toDate > maxDate) {
            isValid = false;
            notificationFactory.error("ValidateMaximumOneYear");
        }

        return isValid;
    };

    self.loadData();
} ]);
