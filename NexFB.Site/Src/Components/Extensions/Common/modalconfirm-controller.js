/*global appModule*/
'use strict';

appModule.controller("ModalConfirmController", ['$scope', '$modalInstance', 'name', 'action',
 function ($scope, $modalInstance, name, action) {
    $scope.name = name;
    $scope.action = action;

    $scope.ok = function () {
        $modalInstance.close();
    };

    $scope.cancel = function () {
        $modalInstance.dismiss("cancel");
    };
}]);