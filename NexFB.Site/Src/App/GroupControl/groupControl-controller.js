'use strict';
/*global groupControlModule*/

groupControlModule.controller("ManageGroupController",
['$scope', '$modal', '$route', 'commonFactory', 'groupControlFactory', 'notificationFactory', 'ErrorFactory',
function ($scope, $modal, $route, commonFactory, groupControlFactory, notificationFactory, ErrorFactory) {
    $scope.filter = {
        groupId: '',
        isActive: ''
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
    ** Get all groups for dropdown.
    */
    groupControlFactory.getGroups($scope.filter).success(function (groups) {
        if (groups) {
            $scope.groupOptions = groups;
        }
    }).error(function () {
        notificationFactory.error("ErrorMessage");
    });

    var getGroups = function (filter) {
        groupControlFactory.getGroups(filter).success(function (groups) {
            if (groups) {
                $scope.groups = groups;
                commonFactory.tablePagination($scope, $scope.groups, $scope.entrySelected);
                $scope.currentPage = $scope.originalPage;
            }
        }).error(function () {
            notificationFactory.error("ErrorMessage");
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
    ** Get user permission on food menu.
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
            templateUrl: '/src/app/GroupControl/Templates/create.html',
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
            templateUrl: '/src/app/GroupControl/Templates/edit.html',
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
            templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
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
} ]);
