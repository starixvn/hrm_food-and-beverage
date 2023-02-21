'use strict';
/*global employeeModule*/

employeeModule.controller('CreateSpecialEmployeeController', createSpecialEmployeeController);
createSpecialEmployeeController.$inject = ['$scope', '$modalInstance', 'companyItems', 'employeeFactory', 'notificationFactory', 'ErrorFactory'];

function createSpecialEmployeeController($scope, $modalInstance, companyItems, employeeFactory, notificationFactory, ErrorFactory) {
    $scope.companyItems = companyItems;
    $scope.employeeTypeItems = [];

    $scope.defaultData = function () {
        $scope.user = {
            Code: '',
            NickName: '',
            EmployeeCard: '',
            IsActive: true,
            CompanyId: '',
            EmployeeType: ''
        };
    };

    $scope.defaultData();

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

    $scope.saveSpecialUser = function (user, isContinue) {
        $scope.disabledSubmitButton = true;
        if ($scope.validateSpecialUser(user)) {
            employeeFactory.insertSpecialUser(user).success(function (response) {
                $scope.submitted = false;
                $scope.disabledSubmitButton = false;

                if (response.IsSuccessful) {
                    notificationFactory.success('DataChangeSuccessfully');
                    if (!isContinue) {
                        $modalInstance.close();
                    } else {
                        $scope.defaultData();
                    }
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