'use strict';
/*global groupControlModule*/

groupControlModule.controller('CreateGroupController',
    ['$scope', '$modalInstance', 'groupControlFactory', 'notificationFactory', 'ErrorFactory',
        function ($scope, $modalInstance, groupControlFactory, notificationFactory, ErrorFactory) {
            $scope.group = {
                groupName: '',
                isActive: true
            };
            
            /*
            ** Set form scope to current scope.
            */
            $scope.setFormScope = function (formScope) {
                $scope.formScope = formScope;
            };

            $scope.addGroup = function (group) {
                groupControlFactory.insertGroup(group).success(function (response) {
                    if (response.IsSuccessful) {
                        $modalInstance.close();
                        notificationFactory.success("DataChangeSuccessfully");
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