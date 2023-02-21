'use strict';
/*global sideDishManagementModule*/

sideDishManagementModule.controller('CreateSideDishController', createSideDishController);
createSideDishController.$inject = ['$scope', '$modalInstance', 'unitOptions', 'employeeTypesOption', 'foodMenuUtilities', 'sideDishUtilities',
    'sideDishManagementFactory', 'notificationFactory', 'ErrorFactory'];

function createSideDishController($scope, $modalInstance, unitOptions, employeeTypesOption, foodMenuUtilities, sideDishUtilities,
    sideDishManagementFactory, notificationFactory, ErrorFactory) {
    $scope.unitOptions = unitOptions;
    $scope.employeeTypesOption = employeeTypesOption;

    $scope.sideDish = {
        foodCode: '',
        foodVnmName: '',
        foodEngName: '',
        unitId: '',
        isActive: true,
        kitchenOnly: false,
        price: '',
        employeeTypeId: ''
    };

    // Add new side dish.
    $scope.save = function (sideDish) {
        $scope.disabledSubmitButton = true;

        if (sideDish) {
            sideDish.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
        }

        if (sideDishUtilities.validateSideDish(sideDish)) {
            sideDishManagementFactory.insertSideDish(sideDish).success(function (response) {
                if (response.IsSuccessful) {
                    notificationFactory.success('DataChangeSuccessfully');
                    $modalInstance.close(sideDish);
                }
                else {
                    notificationFactory.error('', response.Message);
                }
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            })['finally'](function () {
                $scope.disabledSubmitButton = false;
            });
        }
        else {
            $scope.disabledSubmitButton = false;
        }
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}