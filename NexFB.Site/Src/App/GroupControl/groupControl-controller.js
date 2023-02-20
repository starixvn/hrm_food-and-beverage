'use strict';
/*global groupControlModule*/

groupControlModule.controller("ManageGroupController", manageGroupController);
manageGroupController.$inject = ['$scope', '$modal', 'commonFactory', 'groupControlFactory', 'notificationFactory', 'ErrorFactory', 'baseUrl', '$translate'];

function manageGroupController($scope, $modal, commonFactory, groupControlFactory, notificationFactory, ErrorFactory, baseUrl, $translate) {
    $scope.filter = {
        isActive: ''
    };

    $scope.originalPage = 0;
    $scope.entrySelected = 10;
    commonFactory.numberEntryList($scope);

    $translate('RemoveMemberGroupConfirm').then(function (message) {
        $scope.removeMemberGroupConfirmMsg = message;
    });

    groupControlFactory.getAllCompany().success(function (companies) {
        $scope.companyOptions = companies;
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    /*
    ** Paging.
    */
    $scope.showEntries = function () {
        commonFactory.tablePagination($scope, $scope.groups, $scope.entrySelected);
    };

    /*
    ** Get all groups for dropdown.
    */
    groupControlFactory.getGroups($scope.filter).success(function (groups) {
        if (groups) {
            $scope.groupOptions = groups;
        }
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    var getGroups = function (filter) {
        groupControlFactory.getGroups(filter).success(function (groups) {
            if (groups) {
                $scope.groups = groups;
                commonFactory.tablePagination($scope, $scope.groups, $scope.entrySelected);
                $scope.currentPage = $scope.originalPage;
            }
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

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
    ** Get user permission on manage group.
    */
    groupControlFactory.getUserPermissionOnManageGroup().success(function (permission) {
        $scope.policy = {
            permission: permission
        };
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    getGroups($scope.filter);

    $scope.query = function (filter) {
        getGroups(filter);
    };

    /*
    ** Add group.
    */
    $scope.addGroup = function () {
        $scope.originalPage = $scope.currentPage;

        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/app/GroupControl/Templates/create.html',
            controller: "CreateGroupController",
            windowClass: "modal-confirm-center manage-group-modal"
        });

        $scope.modalInstance.result.then(function () {
        })['finally'](function () {
            getGroups($scope.filter);
            $scope.modalInstance = '';
        });
    };

    /*
    ** Edit group.
    */
    var updateChangedGroup = function (group, changedGroup) {
        group.GroupId = changedGroup.GroupId;
        group.GroupName = changedGroup.GroupName;
        group.Members = changedGroup.Members;
        group.IsActive = changedGroup.IsActive;
    };

    $scope.editGroup = function (group) {
        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/app/GroupControl/Templates/edit.html',
            controller: "EditGroupController",
            resolve: {
                group: function () {
                    return angular.copy(group);
                }
            },
            windowClass: "modal-confirm-center manage-group-modal"
        });

        $scope.modalInstance.result.then(function (changedGroup) {
            updateChangedGroup(group, changedGroup);
        })['finally'](function () {
            $scope.modalInstance = '';
        });
    };

    /*
    ** Delete group.
    */
    var updatePaging = function (group, currentPage) {
        var index = $scope.groups.indexOf(group);
        $scope.groups.splice(index, 1);

        commonFactory.tablePagination($scope, $scope.groups, $scope.entrySelected);
        var numberPage = $scope.pagedItems.length;

        if (currentPage <= numberPage - 1) {
            $scope.currentPage = currentPage;
        }
        else {
            $scope.currentPage = currentPage - 1;
        }
    };

    $scope.deleteGroup = function (group) {
        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/components/extensions/common/templates/ModalConfirmContent.html',
            controller: "ModalConfirmController",
            resolve: {
                action: function () {
                    return 'delete';
                },
                name: function () {
                    return group.GroupName;
                }
            },
            windowClass: "modal-confirm-center food-item-modal"
        });

        $scope.modalInstance.result.then(function () {
            var currentPage = $scope.currentPage;

            groupControlFactory.deleteGroup(group.GroupId).success(function (response) {
                if (response.IsSuccessful) {
                    updatePaging(group, currentPage);
                    notificationFactory.success("DataChangeSuccessfully");
                }
                else {
                    notificationFactory.error("", response.Message);
                }
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });
        })['finally'](function () {
            $scope.modalInstance = '';
        });
    };

    /*
    ** Manage member of group.
    */
    $scope.manageGroupMember = function (group) {
        $scope.originalPage = $scope.currentPage;

        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/app/groupControl/templates/manageGroupMember.html',
            controller: "ManageGroupMemberController",
            resolve: {
                companies: function () {
                    return $scope.companyOptions;
                },
                group: function () {
                    return group;
                },
                removeMessage: function () {
                    return $scope.removeMemberGroupConfirmMsg;
                },
                permission: function () {
                    return $scope.policy.permission;
                }
            },
            backdrop: 'static',
            windowClass: "modal-confirm-center manage-group-member"
        });

        $scope.modalInstance.result
            .then()['finally'](function () {
                getGroups($scope.filter);
                $scope.modalInstance = '';
            });
    };

    /*
    ** Activate or deactivate group.
    */
    $scope.confirmActive = function (group) {
        var action = !group.IsActive ? "activate" : "deactivate";
        group.IsActive = !group.IsActive;

        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/components/extensions/common/templates/ModalConfirmContent.html',
            controller: "ModalConfirmController",
            backdrop: 'static',
            resolve: {
                name: function () {
                    return group.GroupName;
                },
                action: function () {
                    return action;
                }
            },
            windowClass: "modal-confirm-center food-item-modal"
        });

        var keyMessage = '';

        $scope.modalInstance.result.then(function () {
            groupControlFactory.activateGroup(group).success(function (response) {
                if (response.IsSuccessful) {
                    keyMessage = action === "activate" ? "ActivationSuccess" : "DeactivationSuccess";
                    notificationFactory.success(keyMessage);
                }
                else {
                    keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
                    group.IsActive = !group.IsActive;
                    notificationFactory.error(keyMessage);
                }
            }).error(function (response, status) {
                if (status === 405) {
                    ErrorFactory.errorHandle(status);
                }
                else {
                    keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
                    group.IsActive = !group.IsActive;
                    notificationFactory.error(keyMessage);
                }
            });
        }, function () {
            group.IsActive = !group.IsActive;
        })['finally'](function () {
            $scope.modalInstance = '';
        });
    };
}