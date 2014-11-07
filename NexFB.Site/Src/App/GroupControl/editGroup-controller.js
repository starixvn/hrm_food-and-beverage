'use strict';
/*global groupControlModule*/

groupControlModule.controller('EditGroupController',
    ['$scope', '$modalInstance', 'group', 'groupControlFactory', 'notificationFactory', 'ErrorFactory',
    function ($scope, $modalInstance, group, groupControlFactory, notificationFactory, ErrorFactory) {
        
    $scope.group = group;

    $scope.updateGroup = function (grp) {
        var editedGroup = angular.copy(grp);

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