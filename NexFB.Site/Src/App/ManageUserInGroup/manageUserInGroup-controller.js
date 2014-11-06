'use strict';
/*global manageUserInGroupModule*/

manageUserInGroupModule.controller("ManageUserInGroupController",
['$scope', '$modal', '$route', '$log', 'commonFactory', 'manageUserInGroupFactory', 'manageUserInGroupUtilities', 'notificationFactory', 'ErrorFactory',
function ($scope, $modal, $route, $log, commonFactory, manageUserInGroupFactory, manageUserInGroupUtilities, notificationFactory, ErrorFactory) {
    var originalFilter = {
        DepartmentId: '',
        GroupId: '',
        EmployeeTypeId: '',
        JoinedDateFrom: '',
        JoinedDateTo: '',
        Query: ''
    };

    var globalMember = {
        SysId: '',
        EmpId: '',
        Fullname: '',
        Nickname: 'All selected employee',
        DepartmentId: '',
        DepartmentName: '',
        GroupIds: [],
        GroupNames: [],
        JoinedDate: ''
    };

    $scope.globalMember = globalMember;
    
    $scope.filter = originalFilter;

    $scope.calendar = {
        startDateOpened: false
    };

    /*
    ** Open calendar.
    */
    $scope.openPopupStartDate = function ($event) {
        commonFactory.openPopupStartDate($event, $scope);
    };

    $scope.originalPage = 0;

    /*
    ** Paging.
    */
    $scope.showEntries = function () {
        commonFactory.tablePagination($scope, $scope.groups, $scope.entrySelected);
    };

    $scope.entrySelected = 10;

    commonFactory.numberEntryList($scope);

    /*
    ** If route change, close dialog.
    */
    $scope.$on('$routeChangeStart', function () {
        // if modal instance defined, dismiss dialog
        if ($scope.modalInstance) {
            $scope.modalInstance.dismiss('cancel');
        }
    });

    /*
    ** Get user permission on food menu.
    */
    manageUserInGroupFactory.getUserPermissionOnManageUserInGroup().success(function (permission) {
        $scope.policy = {
            permission: permission
        };
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });
    
    

    /*
    ** Get all company for drop down.
    */
    manageUserInGroupFactory.getAllDepartment().success(function (companies) {
        if (companies) {
            $scope.companyOptions = companies;
        }
    }).error(function () {
        notificationFactory.error("ErrorMessage");
    });

    /*
    ** Get all employee type for drop down.
    */
    manageUserInGroupFactory.getAllEmployeeType().success(function (types) {
        if (types) {
            $scope.employeeTypeOptions = types;
        }
    }).error(function () {
        notificationFactory.error("ErrorMessage");
    });
    

    var getMembers = function (filter) {
        manageUserInGroupFactory.getMembers(filter).success(function (members) {
            if (members) {
                $scope.members = members;
                commonFactory.tablePagination($scope, $scope.members, $scope.entrySelected);
                $scope.currentPage = $scope.originalPage;
            }
        }).error(function () {
            notificationFactory.error("ErrorMessage");
        });
    };

    /*
    ** Get all groups for drop down.
    */
    manageUserInGroupFactory.getAllGroup().success(function (groups) {
        if (groups) {
            angular.forEach(groups, function (group) {
                group.ticked = true;
            });

            $scope.groupOptions = groups;
            getMembers(originalFilter);
        }
    }).error(function () {
        notificationFactory.error("ErrorMessage");
    });

    $scope.query = function (filter) {
        if (filter) {
            var customizeFilter = {};
            angular.copy(filter, customizeFilter);

            customizeFilter.GroupId = manageUserInGroupUtilities.getGroupId($scope.groupOptions);
            customizeFilter.EmployeeTypeId = manageUserInGroupUtilities.getEmployeeTypeId($scope.employeeTypeOptions);

            if (customizeFilter.JoinedDateFrom !== "" && customizeFilter.JoinedDateTo !== "") {
                customizeFilter.JoinedDateFrom = customizeFilter.JoinedDateFrom.toDateString();
                customizeFilter.JoinedDateTo = customizeFilter.JoinedDateTo.toDateString();
            }

            getMembers(customizeFilter);
        }
    };
} ]);
