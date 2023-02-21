'use strict';
/*global sideDishManagementModule*/

sideDishManagementModule.controller('EditSideDishController', editSideDishController);
editSideDishController.$inject = ['$scope', '$modalInstance', 'sideDish', 'unitOptions', 'employeeTypesOption', 'foodMenuUtilities',
    'sideDishUtilities', 'commonFactory', 'sideDishManagementFactory', 'notificationFactory', 'ErrorFactory'];

function editSideDishController($scope, $modalInstance, sideDish, unitOptions, employeeTypesOption, foodMenuUtilities,
    sideDishUtilities, commonFactory, sideDishManagementFactory, notificationFactory, ErrorFactory) {
    $scope.sideDish = sideDish;
    $scope.unitOptions = unitOptions;
    $scope.employeeTypesOption = employeeTypesOption;

    // Check employee type that belong to side dish.
    var checkEmployeeTypeBelongToSideDish = function (employeeTypeId, employeeTypes) {
        var exist = false;

        angular.forEach(employeeTypes, function (employeeType) {
            if (employeeType.EmployeeTypeId === employeeTypeId) {
                exist = true;
            }
        });

        return exist;
    };

    var checkEmployeeTypeOptions = function () {
        angular.forEach($scope.employeeTypesOption, function (employeeType) {
            employeeType.ticked = checkEmployeeTypeBelongToSideDish(employeeType.EmployeeTypeId, $scope.sideDish.EmployeeTypes);
        });
    };

    checkEmployeeTypeOptions();

    // Get update unit.
    var getUpdateUnit = function (unitId, unitOptions) {
        for (var i = 0; i < unitOptions.length; i++) {
            var unit = unitOptions[i];

            if (unit.UnitId === unitId) {
                return unit;
            }
        }

        return '';
    };

    // Edit side dish.
    $scope.save = function (sideDish) {
        $scope.disabledSubmitButton = true;

        var updatedSideDish = {
            foodId: sideDish.FoodId,
            foodCode: sideDish.FoodCode,
            foodVnmName: sideDish.FoodVnmName,
            foodEngName: sideDish.FoodEngName,
            isActive: sideDish.IsActive,
            kitchenOnly: sideDish.KitchenOnly,
            unitId: sideDish.Unit.UnitId,
            price: sideDish.Price,
            employeeTypeId: foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption)
        };

        if (sideDishUtilities.validateSideDish(updatedSideDish)) {
            sideDish.Unit = getUpdateUnit(updatedSideDish.unitId, $scope.unitOptions);
            sideDish.EmployeeTypes = commonFactory.updateEmployeeTypes(updatedSideDish.employeeTypeId, $scope.employeeTypesOption);

            var allInfoSideDish = {
                updatedSideDish: updatedSideDish,
                sideDish: sideDish
            };

            sideDishManagementFactory.updateSideDish(allInfoSideDish.updatedSideDish).success(function (response) {
                if (response.IsSuccessful) {
                    notificationFactory.success('DataChangeSuccessfully');
                    $modalInstance.close(allInfoSideDish);
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