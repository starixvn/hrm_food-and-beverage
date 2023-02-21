'use strict';
/*global employeeModule*/

employeeModule.controller('EditSpecialEmployeeController', editSpecialEmployeeController);
editSpecialEmployeeController.$inject = ['$scope', '$modalInstance', 'companyItems', 'employeeFactory', 'notificationFactory', 'ErrorFactory', 'user'];

function editSpecialEmployeeController($scope, $modalInstance, companyItems, employeeFactory, notificationFactory, ErrorFactory, user) {
    $scope.companyItems = companyItems;
    $scope.user = user;

    var getEmployeeType = function (companyId) {
        employeeFactory.getEmployeeType(companyId).success(function (employeeTypes) {
            $scope.employeeTypeItems = employeeTypes;
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    $scope.getEmployeeTypeByCompany = function (companyId) {
        getEmployeeType(companyId);
    };

    getEmployeeType(user.CompanyId);

    $scope.validateSpecialUser = function (user) {
        var isValid = true;

        if (!user.Code) {
            isValid = false;
            notificationFactory.error('RequireEmployeeCode');
        }

        if (!user.NickName) {
            isValid = false;
            notificationFactory.error('RequireEmployeeName');
        }

        if (!user.CompanyId) {
            isValid = false;
            notificationFactory.error('CompanyRequired');
        }

        if (!user.EmployeeType) {
            isValid = false;
            notificationFactory.error('RequireEmployeeType');
        }

        return isValid;
    };

    $scope.saveSpecialUser = function (user) {
        $scope.disabledSubmitButton = true;
        if ($scope.validateSpecialUser(user)) {
            employeeFactory.updateSpecialUser(user).success(function (response) {
                $scope.submitted = false;
                $scope.disabledSubmitButton = false;

                if (response.IsSuccessful) {
                    notificationFactory.success('DataChangeSuccessfully');
                    $modalInstance.close();
                } else {
                    notificationFactory.error('', response.Message);
                }
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
                $scope.disabledSubmitButton = false;
            });
        }

        $scope.disabledSubmitButton = false;
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}