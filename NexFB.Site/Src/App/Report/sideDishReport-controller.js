'use strict';
/*global reportModule*/

reportModule.controller('SideDishReportController',
['$scope', 'reportFactory', 'ErrorFactory', 'userControlFactory', 'foodMenuFactory', 'foodMenuUtilities',
'reportUtilities',
function ($scope, reportFactory, ErrorFactory, userControlFactory, foodMenuFactory, foodMenuUtilities,
reportUtilities) {
    var self = this;

    $scope.departments = [];
    $scope.teams = [];
    $scope.employeeTypes = [];
    $scope.sideDishItems = [];

    $scope.criteria = {
        departmentId: 0,
        teamIds: '',
        employeeTypeIds: '',
        fromDate: new Date(),
        toDate: new Date(),
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

        if (dateType == "from") {
            $scope.fromOpened = true;
        } else if (dateType == "to") {
            $scope.toOpened = true;
        }
    };

    reportFactory.getDepartmentByUserRole().success(function (departments) {
        $scope.departments = departments;

        //getDepartmentFilter(departments);

        //todo: get report data

        //self.getTeamByCompany($scope.criteria.departmentId);

        //self.getEmployeeType($scope.criteria.departmentId, true);

    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    self.getTeamByCompany = function (departmentId) {
        userControlFactory.getAllTeamByCompany(departmentId).success(function (teams) {
            $scope.teams = teams;
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    self.getEmployeeType = function (departmentId) {
        foodMenuFactory.getAllEmployeeType(departmentId).success(function (employeeTypes) {
            $scope.employeeTypes = employeeTypes;
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };



} ]);
