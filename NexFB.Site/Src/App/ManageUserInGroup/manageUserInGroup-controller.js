'use strict';
/*global manageUserInGroupModule*/

manageUserInGroupModule.controller("ManageUserInGroupController",
['$scope', '$modal', '$route', '$log', '$routeParams', 'commonFactory',
    'manageUserInGroupFactory', 'manageUserInGroupUtilities', 'notificationFactory', 'ErrorFactory',
function ($scope, $modal, $route, $log, $routeParams, commonFactory,
    manageUserInGroupFactory, manageUserInGroupUtilities, notificationFactory, ErrorFactory) {
    var groupId = parseInt($routeParams.groupId, 10);
    console.log(groupId);

    var originalFilter = {
        DepartmentId: '',
        GroupId: '',
        EmployeeTypeId: '',
        JoinedDateFrom: '',
        JoinedDateTo: '',
        Query: ''
    };

    var originalGroupOptions = [];

    var globalMember = {
        SysId: '',
        EmpId: '',
        Fullname: '',
        Nickname: 'All selected employee',
        DepartmentId: '',
        DepartmentName: '',
        AssignedGroups: [],
        GroupOptions: [],
        BackupGroupOptions: [],
        IsEditing: false,
        JoinedDate: ''
    };

    $scope.HasEditing = false;
    $scope.globalMember = angular.copy(globalMember);
    $scope.filter = angular.copy(originalFilter);
    $scope.filter.GroupId = isNaN(groupId) ? '' : groupId;

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
        commonFactory.tablePagination($scope, $scope.members, $scope.entrySelected);
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

    var customizeMember = function (members) {
        angular.forEach(members, function (member) {
            var groupOptions = angular.copy(originalGroupOptions);
            var groupIds = manageUserInGroupUtilities.getGroupIds(member.AssignedGroups);

            angular.forEach(groupOptions, function (group) {
                var index = groupIds.indexOf(group.GroupId);
                group.ticked = index >= 0;
            });

            member.GroupOptions = groupOptions;
            member.IsEditing = false;
        });

        return members;
    };

    var getMembers = function (filter) {
        manageUserInGroupFactory.getMembers(filter).success(function (members) {
            if (members) {
                $scope.globalMember = angular.copy(globalMember);
                $scope.members = customizeMember(members);
                $scope.HasEditing = false;

                commonFactory.tablePagination($scope, $scope.members, $scope.entrySelected);
                $scope.currentPage = $scope.originalPage;
                $scope.previousPage = $scope.currentPage;
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
            originalGroupOptions = angular.copy(groups);
            angular.forEach(groups, function (group) {
                group.ticked = isNaN(groupId) ? false : group.GroupId === groupId;
            });

            globalMember.GroupOptions = angular.copy(originalGroupOptions);
            $scope.groupOptions = angular.copy(groups);

            getMembers($scope.filter);
        }
    }).error(function () {
        notificationFactory.error("ErrorMessage");
    });

    $scope.query = function (filter) {
        if (filter) {
            var customizeFilter = angular.copy(filter);
            var selectedGroups = manageUserInGroupUtilities.getSelectedGroups($scope.groupOptions);

            customizeFilter.GroupId = manageUserInGroupUtilities.getGroupIds(selectedGroups).join(',');
            customizeFilter.EmployeeTypeId = manageUserInGroupUtilities.getEmployeeTypeId($scope.employeeTypeOptions);

            if (customizeFilter.JoinedDateFrom !== "" && customizeFilter.JoinedDateTo !== "") {
                customizeFilter.JoinedDateFrom = customizeFilter.JoinedDateFrom.toDateString();
                customizeFilter.JoinedDateTo = customizeFilter.JoinedDateTo.toDateString();
            }

            getMembers(customizeFilter);
        }
    };

    /*####### Start: Assign Single Member ####*/
    $scope.editMember = function (member) {
        member.IsEditing = true;
        member.BackupGroupOptions = angular.copy(member.GroupOptions);
        $scope.HasEditing = true;
    };

    $scope.cancelAssignMember = function (member) {
        member.IsEditing = false;
        member.GroupOptions = angular.copy(member.BackupGroupOptions);
        hasEditedMember();
    };

    $scope.assignMember = function (member) {
        var filter = {
            SysIds: '',
            GroupIds: '',
            MemberId: ''
        };

        var selectedGroups = manageUserInGroupUtilities.getSelectedGroups(member.GroupOptions);
        filter.SysIds = member.SysId;
        filter.GroupIds = manageUserInGroupUtilities.getGroupIds(selectedGroups).join(',');

        manageUserInGroupFactory.assignMemberToGroup(filter)
            .success(function () {
                member.IsEditing = false;
                member.AssignedGroups = selectedGroups;
                hasEditedMember();
            }).error(function () {
                notificationFactory.error("ErrorMessage");
            });
    };

    var hasEditedMember = function () {
        var members = $scope.pagedItems[$scope.currentPage];
        var hasEditing = false;

        angular.forEach(members, function (member) {
            if (member.IsEditing) {
                hasEditing = true;
                return false;
            }
        });

        if (!hasEditing) {
            $scope.globalMember.IsEditing = false;
        }

        $scope.HasEditing = hasEditing;
    };

    /*####### End: Assign Single Member ####*/


    /*####### Start: Assign Multiple Member ####*/

    $scope.editMultipleMembers = function () {
        $scope.HasEditing = true;
        $scope.globalMember.IsEditing = true;
        $scope.globalMember.BackupGroupOptions = angular.copy($scope.globalMember.GroupOptions);

        var members = $scope.pagedItems[$scope.currentPage];
        angular.forEach(members, function (member) {
            member.IsEditing = true;
            member.BackupGroupOptions = angular.copy(member.GroupOptions);
        });
    };

    $scope.cancelAssignMultipleMembers = function () {
        $scope.HasEditing = false;
        $scope.globalMember.IsEditing = false;
        $scope.globalMember.GroupOptions = angular.copy($scope.globalMember.BackupGroupOptions);

        var members = $scope.pagedItems[$scope.currentPage];
        angular.forEach(members, function (member) {
            member.IsEditing = false;
            member.GroupOptions = angular.copy(member.BackupGroupOptions);
        });
    };

    $scope.changeSelectedGroup = function (groupOptions) {
        var members = $scope.pagedItems[$scope.currentPage];
        angular.forEach(members, function (member) {
            member.GroupOptions = angular.copy(groupOptions);
        });
    };

    $scope.assignMultipleMembers = function () {
        var filter = {
            SysIds: '',
            GroupIds: '',
            MemberId: ''
        };

        var members = $scope.pagedItems[$scope.currentPage];
        var sysIds = [], groupIds = [], editingMembers = [], editingGroupOptions = [];

        angular.forEach(members, function (member) {
            if (member.IsEditing) {
                var selectedGroups = manageUserInGroupUtilities.getSelectedGroups(member.GroupOptions);
                editingMembers.push(member);
                editingGroupOptions.push(selectedGroups);

                sysIds.push(member.SysId);
                groupIds.push(manageUserInGroupUtilities.getGroupIds(selectedGroups).join(','));
            }
        });

        filter.SysIds = sysIds.join(';');
        filter.GroupIds = groupIds.join(';');

        manageUserInGroupFactory.assignMemberToGroup(filter)
            .success(function () {
                $scope.globalMember.IsEditing = false;
                $scope.globalMember.HasEditing = false;
                $scope.globalMember.AssignedGroups = [];
                $scope.globalMember.GroupOptions = angular.copy(originalGroupOptions);

                var length = editingMembers.length;
                for (var i = 0; i < length; i++) {
                    editingMembers[i].IsEditing = false;
                    editingMembers[i].AssignedGroups = editingGroupOptions[i];
                }
            }).error(function () {
                notificationFactory.error("ErrorMessage");
            });
    };

    /*####### End: Assign Multiple Member ####*/

    $scope.selectMember = function (SysId) {
        $scope.selectedMember = SysId;
    };

    $scope.resetMember = function () {
        var members = $scope.pagedItems[$scope.previousPage];
        angular.forEach(members, function (member) {
            if (member.IsEditing) {
                member.IsEditing = false;
                member.GroupOptions = angular.copy(member.BackupGroupOptions);
            }
        });

        $scope.HasEditing = false;
        $scope.globalMember.IsEditing = false;
        $scope.globalMember.GroupOptions = angular.copy($scope.globalMember.BackupGroupOptions);
        $scope.previousPage = $scope.currentPage;
    };

    $scope.$watch('currentPage', function () {
        var current = parseInt($scope.currentPage);
        if (!isNaN(current)) {
            $scope.resetMember();
        }
    });
} ]);
