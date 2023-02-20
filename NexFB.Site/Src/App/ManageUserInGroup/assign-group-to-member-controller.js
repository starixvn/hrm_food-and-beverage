'use strict';
/*global manageUserInGroupModule*/

manageUserInGroupModule.controller('AsignGroupToMemberController', assignGroupToMemberController);

assignGroupToMemberController.$inject = ['$scope', 'sysIds', '$modalInstance', 'groupControlFactory', 'notificationFactory', 'manageUserInGroupFactory',
    'ErrorFactory', 'manageUserInGroupUtilities', 'assignedGroups', 'members', 'commonFactory'];

function assignGroupToMemberController($scope, sysIds, $modalInstance, groupControlFactory, notificationFactory, manageUserInGroupFactory,
    ErrorFactory, manageUserInGroupUtilities, assignedGroups, members, commonFactory) {
    $scope.members = members;
    $scope.sysIds = sysIds;
    $scope.assignedGroups = assignedGroups;
    $scope.searchData = {};

    var checkAssignedGroup = function () {
        angular.forEach($scope.groupOptions, function (groupOption) {
            angular.forEach($scope.assignedGroups, function (assignedGroup) {
                if (groupOption.GroupId === assignedGroup.GroupId) {
                    groupOption.ticked = true;
                }
            });
        });
    };

    $scope.searchMemberGroup = function () {
        commonFactory.searchMemberGroup($scope, $scope.members, $scope.searchData.queryData);
    };

    $scope.searchMemberGroup();

    /*
    ** Get all groups.
    */
    groupControlFactory.getGroups($scope.filter).success(function (groups) {
        $scope.groupOptions = groups;

        if ($scope.assignedGroups) {
            checkAssignedGroup();
        }
    }).error(function () {
        notificationFactory.error("ErrorMessage");
    });

    /*
    ** Assign group to member.
    */
    $scope.save = function () {
        var groupIds = manageUserInGroupUtilities.getSelectedGroupIds($scope.groupOptions),
            filter = {
                sysIds: $scope.sysIds,
                groupIds: groupIds
            };

        manageUserInGroupFactory.assignGroupToMember(filter).success(function (response) {
            if (response.IsSuccessful) {
                notificationFactory.success("DataChangeSuccessfully");
                $modalInstance.close();
            }
            else {
                notificationFactory.error("", response.Message);
            }
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}