'use strict';
/*global reportModule*/

reportModule.controller('SideDishReportController', sideDishReportController);
sideDishReportController.$inject = ['$scope', '$location', '$routeParams', 'reportFactory', 'notificationFactory', 'ErrorFactory', 'userControlFactory',
    'foodMenuFactory', 'foodMenuUtilities', 'reportUtilities', 'orderingFactory', 'commonFactory', 'baseUrl'];

function sideDishReportController($scope, $location, $routeParams, reportFactory, notificationFactory, ErrorFactory, userControlFactory,
    foodMenuFactory, foodMenuUtilities, reportUtilities, orderingFactory, commonFactory, baseUrl) {
    var self = this;
    var date = new Date(),
        year = date.getFullYear(),
        month = date.getMonth(),
        day = date.getDate();

    // $scope region.
    $scope.companies = [];
    $scope.teams = [];
    $scope.employeeTypes = [];
    $scope.sideDishItems = [];
    $scope.searchEmployee = '';

    $scope.criteria = {
        companyId: null,
        companyIds: '',
        teamIds: null,
        employeeTypeIds: '',
        employee: {},
        userId: null,
        fromDate: new Date(year, month, 1),
        toDate: new Date(year, month, day),
        sideDishItemIds: '',
        pageIndex: 0,
        itemsPerPage: 50
    };

    if ($routeParams.companyId) {
        var companyId = -1;
        if ($routeParams.companyId.length === 0 || $routeParams.companyId.split(',').length > 1) {
            companyId = -1;
        } else {
            companyId = parseInt($routeParams.companyId);
        }

        $scope.history = {
            companyId: companyId,
            teamId: $routeParams.teamId,
            employeeTypeId: $routeParams.employeeTypeId,
            userId: $routeParams.userId,
            fromDate: Date.parse($routeParams.fromDate),
            toDate: Date.parse($routeParams.toDate)
        };
    }

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
            $scope.toOpened = false;
        } else if (dateType === "to") {
            $scope.toOpened = true;
            $scope.fromOpened = false;
        }
    };

    $scope.getTeamAndEmployeeType = function () {
        self.getTeams();
        self.getEmployeeTypes();
    };

    $scope.search = function () {
        var isValid = self.validateFilterOptions();

        if (isValid) {
            $scope.criteria.pageIndex = 0;

            $scope.getReportData();
        }
    };

    $scope.exportExcel = function () {
        self.getCriteria();

        var criteria = {
            companyId: $scope.criteria.companyIds,
            employeeTypeId: $scope.criteria.employeeTypeIds,
            teamId: $scope.criteria.teamIds,
            userId: $scope.criteria.employee ? $scope.criteria.employee.UserId : '',
            fromDate: $scope.criteria.fromDate.toString("MM/dd/yyyy"),
            toDate: $scope.criteria.toDate.toString("MM/dd/yyyy"),
            sideDishItemIds: $scope.criteria.sideDishItemIds,
            pageIndex: null,
            itemsPerPage: null
        };
        console.log(criteria)

        var data = $.param(criteria),
            url = baseUrl + 'ExportReport/ExportSideDishReport',
            downloadUrl = url + "?" + data,
            iframe = $('body iframe');

        if (iframe.length === 0) {
            iframe = $('<iframe width="0" height="0" class="display-none"></iframe>');
            iframe.appendTo('body');
        }

        iframe.attr('src', downloadUrl);
    };

    $scope.getReportData = function () {
        if ($routeParams.companyId && $scope.criteria.companyId === $scope.history.companyId) {
            $scope.sideDishSummary();
        } else {
            self.getCriteria();
            commonFactory.blockUI();
            reportFactory.getSideDishReport($scope.criteria).success(function (reportData) {
                $scope.reportData = reportData;
                $scope.$broadcast('renderSideDishReport');

                commonFactory.unblockUI();
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);

                commonFactory.unblockUI();
            });
        }
    };

    $scope.sideDishSummary = function () {
        $scope.criteria.companyId = $scope.history.companyId;

        self.getCompanyIds();

        $scope.criteria.teamIds = $scope.history.teamId;
        $scope.criteria.employeeTypeIds = $scope.history.employeeTypeId;

        if ($scope.history.userId.length > 0) {
            $scope.criteria.userId = parseInt($scope.history.userId);
        }
        else {
            $scope.criteria.employee = '';
        }

        $scope.criteria.fromDate = $scope.history.fromDate;
        $scope.criteria.toDate = $scope.history.toDate;

        commonFactory.blockUI();
        reportFactory.getSideDishReport($scope.criteria).success(function (reportData) {
            $scope.reportData = reportData;
            $scope.$broadcast('renderSideDishReport');

            commonFactory.unblockUI();
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);

            commonFactory.unblockUI();
        });

        if ($scope.criteria.companyId > 0) {
            userControlFactory.getAllTeamByCompany($scope.criteria.companyId).success(function (teams) {
                $scope.teams = teams;
                if ($scope.history.teamId.length > 0) {
                    var ids = $scope.history.teamId.split(',');
                    angular.forEach($scope.teams, function (team) {
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
        } else {
            $scope.teams = [];
        }

        foodMenuFactory.getAllEmployeeType($scope.criteria.companyIds).success(function (employeeTypes) {
            $scope.employeeTypes = employeeTypes;
            if ($scope.history.employeeTypeId.length > 0) {
                var ids = $scope.history.employeeTypeId.split(',');
                angular.forEach($scope.employeeTypes, function (employeeType) {
                    angular.forEach(ids, function (id) {
                        if (employeeType.EmployeeTypeId === parseInt(id)) {
                            employeeType.ticked = true;
                        }
                    });
                });
            } else {
                $scope.criteria.employeeTypeIds = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypes);
            }
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });

        if (angular.isNumber($scope.criteria.userId)) {
            reportFactory.getSpecialUserById($scope.criteria.userId).success(function (employee) {
                $scope.criteria.employee = employee;
                $scope.searchEmployee = employee.NickName + ' (' + employee.FullName + ')';
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });
        }
        else {
            $scope.criteria.employee = '';
        }
    };

    $scope.getAllSideDish = function () {
        $scope.criteria.employeeTypeIds = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypes);

        if ($scope.criteria.employeeTypeIds.length === 0) {
            var ids = [];

            angular.forEach($scope.employeeTypes, function (employeeType) {
                ids.push(employeeType.EmployeeTypeId);
            });

            $scope.criteria.employeeTypeIds = ids.join(',');
        }

        reportFactory.getAllSideDish(null, null, $scope.criteria.employeeTypeIds).success(function (sideDishes) {
            $scope.sideDishItems = sideDishes;
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    $scope.getEmployee = function () {
        self.getTeamIdsAndCompanyIds();

        var result = orderingFactory.getEmployees($scope.searchEmployee, null, $scope.criteria.companyIds, $scope.criteria.teamIds, true);

        return result;
    };

    $scope.unSelectEmployee = function () {
        if ($scope.searchEmployee.trim() === '') {
            $scope.criteria.employee = null;
            $scope.criteria.userId = '';
        }
    };

    $scope.goback = function () {
        $location.url('/SideDishSummaryReport/Index');
    };

    $scope.onSelectTeam = function (teamIds) {
        $scope.criteria.teamIds = teamIds;
    };

    self.getTeams = function () {
        if ($scope.criteria.companyId > 0) {
            userControlFactory.getOrgChart($scope.criteria.companyId).success(function (teams) {
                $scope.teams = teams;
                if ($routeParams.companyId && $scope.criteria.companyId !== $scope.history.companyId) {
                    $scope.history.companyId = $scope.criteria.companyId;
                }
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });
        } else {
            $scope.teams = [];
        }
    };

    self.getCompanyIds = function () {
        if ($scope.criteria.companyId === -1) {
            var ids = [];

            angular.forEach($scope.companies, function (company) {
                if (company.CompanyId !== -1) {
                    ids.push(company.CompanyId);
                }
            });

            $scope.criteria.companyIds = ids.join(',');
        } else {
            $scope.criteria.companyIds = $scope.criteria.companyId;
        }
    };

    self.getEmployeeTypes = function () {
        self.getCompanyIds();

        foodMenuFactory.getAllEmployeeType($scope.criteria.companyIds).success(function (employeeTypes) {
            $scope.employeeTypes = employeeTypes;
            if ($routeParams.companyId && $scope.criteria.companyId !== $scope.history.companyId) {
                $scope.history.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypes);
            }
            $scope.getAllSideDish();
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    self.getCompany = function (callback) {
        reportFactory.getCompanyByUserRole().success(function (companies) {
            if (companies.length > 1) {
                companies.splice(0, 0, { CompanyId: -1, Name: '-- All --' });
            }

            $scope.companies = companies;

            if (companies.length > 0) {
                $scope.criteria.companyId = companies[0].CompanyId;
            }

            if ($routeParams.companyId) {
                $scope.criteria.companyId = $scope.history.companyId;
            }

            if (angular.isFunction(callback)) {
                callback();
            }
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    self.loadData = function () {
        self.getCompany(function () {
            $scope.getTeamAndEmployeeType();

            $scope.getReportData();
        });
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

    self.getCriteria = function () {
        $scope.criteria.employeeTypeIds = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypes);
        $scope.criteria.sideDishItemIds = reportUtilities.getSideDishIds($scope.sideDishItems);
        self.getTeamIdsAndCompanyIds();
    };

    self.getTeamIdsAndCompanyIds = function () {
        self.getCompanyIds();
    };

    self.loadData();
}