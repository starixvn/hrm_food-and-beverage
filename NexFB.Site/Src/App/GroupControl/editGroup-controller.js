'use strict';
/*global groupControlModule*/

groupControlModule.controller('EditGroupController',
    ['$scope', '$modalInstance', 'group', 'groupControlFactory', 'notificationFactory', 'ErrorFactory',
    function ($scope, $modalInstance, group, groupControlFactory, notificationFactory, ErrorFactory) {
        
    $scope.group = group;

    $scope.updateGroup = function (group) {

        var editedGroup = {
            groupId: group.GroupId,
            groupName: group.GroupName,
            isActive: group.IsActive
        };

        groupControlFactory.updateGroup(editedGroup).success(function (response) {
            if (response.IsSuccessful) {
                notificationFactory.success("DataChangeSuccessfully");
                $modalInstance.close(group);
            }
            else {
                notificationFactory.error("", response.Message);
            }
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    $scope.cancel = function () {
        $modalInstance.dismiss("cancel");
    };
} ]);