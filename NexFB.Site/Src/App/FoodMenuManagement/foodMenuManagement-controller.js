'use strict';
/*global foodMenuManagementModule*/

foodMenuManagementModule.controller('FoodMenuManagementController', foodMenuManagementController);
foodMenuManagementController.$inject = ['$scope', '$modal', 'foodMenuFactory', 'notificationFactory', 'commonFactory', 'setMenuFactory', 'baseUrl', 'ErrorFactory'];

function foodMenuManagementController($scope, $modal, foodMenuFactory, notificationFactory, commonFactory, setMenuFactory, baseUrl, ErrorFactory) {
    $scope.filter = {
        mealId: '',
        employeeTypeId: '',
        isActive: 'true'
    };

    $scope.originalPage = 0;

    $scope.$on('$routeChangeStart', function () {
        // if modal instance defined, dismiss dialog
        if ($scope.modalInstance) {
            $scope.modalInstance.dismiss('cancel');
        }
    });

    $scope.showEntries = function () {
        commonFactory.tablePagination($scope, $scope.foodMenus, $scope.entrySelected);
    };

    $scope.entrySelected = 10;
    commonFactory.numberEntryList($scope);

    foodMenuFactory.getAllMealTimesheet().success(function (meals) {
        $scope.mealsOption = meals;
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    foodMenuFactory.getAllEmployeeType(null, null).success(function (employeeTypes) {
        $scope.employeeTypesOption = employeeTypes;
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    setMenuFactory.getSetMenu(null, null).success(function (setMenus) {
        $scope.setMenuItems = setMenus;
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    foodMenuFactory.getUserPermissionOnFoodMenu().success(function (permission) {
        $scope.policy = {
            permission: permission
        };
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    var getAllFoodMenu = function (mealId, employeeTypeId, isActive) {
        foodMenuFactory.getAllFoodMenu(mealId, employeeTypeId, isActive).success(function (foodMenus) {
            $scope.foodMenus = foodMenus;
            commonFactory.tablePagination($scope, $scope.foodMenus, $scope.entrySelected);
            $scope.currentPage = $scope.originalPage;
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    getAllFoodMenu(null, null, true);

    $scope.query = function (filter) {
        if (filter) {
            getAllFoodMenu(filter.mealId, filter.employeeTypeId, filter.isActive);
        }
    };

    // Add food menu.
    $scope.addFoodMenu = function () {
        $scope.originalPage = $scope.currentPage;

        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/app/FoodMenuManagement/Templates/create.html',
            controller: 'CreateFoodMenuController',
            backdrop: 'static',
            resolve: {
                mealsOption: function () {
                    return $scope.mealsOption;
                },
                employeeTypesOption: function () {
                    return $scope.employeeTypesOption;
                },
                setMenuItems: function () {
                    return $scope.setMenuItems;
                }
            },
            windowClass: 'modal-confirm-center food-menu-modal'
        });

        $scope.modalInstance.result.then(function () {
        })['finally'](function () {
            getAllFoodMenu(null, null, null);
            $scope.modalInstance = '';
        });
    };

    // Edit food menu.
    var updateChangedFoodMenu = function (foodMenu, changedFoodMenu) {
        foodMenu.MenuVnmName = changedFoodMenu.MenuVnmName;
        foodMenu.MenuEngName = changedFoodMenu.MenuEngName;
        foodMenu.IsVegetarian = changedFoodMenu.IsVegetarian;
        foodMenu.IsActive = changedFoodMenu.IsActive;
        foodMenu.SetMenus = changedFoodMenu.SetMenus;
        foodMenu.EmployeeTypes = changedFoodMenu.EmployeeTypes;
        foodMenu.Meals = changedFoodMenu.Meals;
    };

    $scope.edit = function (foodMenu) {
        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/app/FoodMenuManagement/Templates/edit.html',
            controller: 'EditFoodMenuController',
            backdrop: 'static',
            resolve: {
                foodMenu: function () {
                    return angular.copy(foodMenu);
                },
                setMenuItems: function () {
                    return $scope.setMenuItems;
                },
                mealsOption: function () {
                    return $scope.mealsOption;
                },
                employeeTypesOption: function () {
                    return $scope.employeeTypesOption;
                }
            },
            windowClass: 'modal-confirm-center food-menu-modal'
        });

        $scope.modalInstance.result.then(function (changedFoodMenu) {
            updateChangedFoodMenu(foodMenu, changedFoodMenu);
        })['finally'](function () {
            $scope.modalInstance = '';
        });
    };

    $scope.viewHistory = function (menuId, menuNameVN, menuNameEN) {
        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/app/FoodMenuManagement/Templates/menuHistory.html',
            controller: 'ViewHistoryFoodMenuController',
            backdrop: 'static',
            resolve: {
                menuId: function () {
                    return menuId;
                },
                menuNameVN: function () {
                    return menuNameVN;
                },
                menuNameEN: function () {
                    return menuNameEN;
                },
            },
            windowClass: 'modal-confirm-center food-menu-modal'
        });

        $scope.modalInstance.result.then(function (changedFoodMenu) {
            updateChangedFoodMenu(foodMenu, changedFoodMenu);
        })['finally'](function () {
            $scope.modalInstance = '';
        });
    };

    // Activate or deactivate food menu.
    $scope.confirmActive = function (foodMenu) {
        var action = !foodMenu.IsActive ? 'activate' : 'deactivate',
            newFoodMenu = {
                menuId: foodMenu.MenuId,
                IsActive: !foodMenu.IsActive
            };

        foodMenu.IsActive = newFoodMenu.IsActive;

        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/components/extensions/common/templates/ModalConfirmContent.html',
            controller: 'ModalConfirmController',
            backdrop: 'static',
            resolve: {
                name: function () {
                    return foodMenu.MenuVnmName + ' (' + foodMenu.MenuEngName + ')';
                },
                action: function () {
                    return action;
                }
            },
            windowClass: 'modal-confirm-center food-item-modal'
        });

        var keyMessage = '';

        $scope.modalInstance.result.then(function () {
            foodMenuFactory.activateFoodMenu(newFoodMenu).success(function (response) {
                if (response.IsSuccessful) {
                    keyMessage = action === 'activate' ? 'ActivationSuccess' : 'DeactivationSuccess';
                    notificationFactory.success(keyMessage);
                }
                else {
                    keyMessage = action === 'activate' ? 'ActivationFail' : 'DeactivationFail';
                    foodMenu.IsActive = !newFoodMenu.IsActive;

                    if (response.Code === 101) {
                        notificationFactory.error('DuplicateFoodMenuMessage');
                    }
                    else {
                        notificationFactory.error(keyMessage);
                    }
                }
            }).error(function (response, status) {
                if (status === 405) {
                    ErrorFactory.errorHandle(status);
                }
                else {
                    keyMessage = action === 'activate' ? 'ActivationFail' : 'DeactivationFail';
                    foodMenu.IsActive = !newFoodMenu.IsActive;
                    notificationFactory.error(keyMessage);
                }
            });
        }, function () {
            foodMenu.IsActive = !newFoodMenu.IsActive;
        })['finally'](function () {
            $scope.modalInstance = '';
        });
    };

    // Delete food menu.
    var updatePaging = function (foodMenu, currentPage) {
        var index = $scope.foodMenus.indexOf(foodMenu);
        $scope.foodMenus.splice(index, 1);

        commonFactory.tablePagination($scope, $scope.foodMenus, $scope.entrySelected);
        var numberPage = $scope.pagedItems.length;

        if (currentPage <= numberPage - 1) {
            $scope.currentPage = currentPage;
        }
        else {
            $scope.currentPage = currentPage - 1;
        }
    };

    $scope.deleteFoodMenu = function (foodMenu) {
        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/components/extensions/common/templates/ModalConfirmContent.html',
            controller: 'ModalConfirmController',
            backdrop: 'static',
            resolve: {
                action: function () {
                    return 'delete';
                },
                name: function () {
                    return foodMenu.MenuVnmName + ' (' + foodMenu.MenuEngName + ')';
                }
            },
            windowClass: 'modal-confirm-center food-item-modal'
        });

        $scope.modalInstance.result.then(function () {
            var currentPage = $scope.currentPage;

            foodMenuFactory.deleteFoodMenu(foodMenu.MenuId).success(function (response) {
                if (response.IsSuccessful) {
                    updatePaging(foodMenu, currentPage);
                    notificationFactory.success('DataChangeSuccessfully');
                }
                else {
                    notificationFactory.error('', response.Message);
                }
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });
        })['finally'](function () {
            $scope.modalInstance = '';
        });
    };
}