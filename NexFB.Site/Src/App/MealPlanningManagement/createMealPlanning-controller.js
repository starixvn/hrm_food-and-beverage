'use strict';
/*global mealPlanningManagementModule*/

mealPlanningManagementModule.controller('CreateMealPlanningController',
    ['$scope', '$modalInstance', 'mealPlanning', 'mealPlanningFactory', 'ErrorFactory', 'notificationFactory',
        'menuOptions', 'foodOptions', 'commonFactory', 'setMenuFactory', 'mealPlanningUtilities', 'idsMultiSelect',
        'currentMealPlanning',
        function ($scope, $modalInstance, mealPlanning, mealPlanningFactory, ErrorFactory, notificationFactory,
            menuOptions, foodOptions, commonFactory, setMenuFactory, mealPlanningUtilities, idsMultiSelect,
            currentMealPlanning) {

            $scope.idsMultiSelect = idsMultiSelect;
            $scope.menuOptions = menuOptions;
            $scope.foodItems = [];
            $scope.currentMealPlanning = currentMealPlanning;
            $scope.foodOptions = foodOptions;
            $scope.foodIds = {
                rawFoodIds: []
            };

            $scope.calendar = {
                startDateOpened: false
            };

            $scope.mealPlanning = {
                menuId: mealPlanning !== '' ? mealPlanning.Menu.MenuId : '',
                setMenuId: mealPlanning !== '' ? mealPlanning.SetMenu.SetMenuId : '',
                onDate: '',
                isActive: true,
                date: mealPlanning !== '' ? mealPlanning.Date : new Date()
            };

            /*
            ** set default check when open dialog
            */
            var setDefaultCheck = function (items, id, idsMultiSelect) {
                angular.forEach(items, function (item) {
                    if (item.MenuId === id || (idsMultiSelect.includes(item.MenuId.toString()) && !$scope.currentMealPlanning)) {
                        item.ticked = true;
                    }
                });
            };
            var resetDefaultCheck = function (items) {
                angular.forEach(items, function (item) {
                    item.ticked = false;
                });
            };
            resetDefaultCheck($scope.menuOptions);
            setDefaultCheck($scope.menuOptions, $scope.mealPlanning.menuId, $scope.idsMultiSelect);

            /*
            ** get id from multi select menu
            */

            var getIdFromOptions = function (Options) {
                var arrId = '';

                angular.forEach(Options, function (value) {
                    if (value.ticked) {
                        var id = value.MenuId.toString();
                        arrId = arrId !== '' ? arrId + ',' + id : id;
                    }
                });

                return arrId;
            };


            /*
            ** Open calendar.
            */
            $scope.openPopupStartDate = function ($event) {
                commonFactory.openPopupStartDate($event, $scope);
            };

            /*
            ** Get food items by set menu.
            */
            $scope.getFoodItemsBySetMenu = function (setMenuId) {
                $scope.foodItems = [];
                $scope.foodIds.rawFoodIds = [];

                if (setMenuId) {
                    angular.forEach($scope.setMenuItems, function (setMenu) {
                        if (setMenu.SetMenuId === setMenuId) {
                            angular.forEach(setMenu.FoodItems, function (food) {
                                $scope.foodItems.push(food);
                                $scope.foodIds.rawFoodIds.push(food.FoodId);
                            });
                        }
                    });
                }
            };

            /*
            ** Get Set Menu bu Menu Id.
            */
            $scope.getSetMenu = function (menuId) {
                if (menuId) {
                    setMenuFactory.getSetMenu(menuId, true).success(function (setMenus) {
                        $scope.setMenuItems = setMenus;
                    }).error(function (response, status) {
                        ErrorFactory.errorHandle(status);
                    });
                }
                else {
                    $scope.setMenuItems = [];
                    $scope.mealPlanning.setMenuId = '';
                }
            };

            if ($scope.mealPlanning.menuId) {
                $scope.getSetMenu($scope.mealPlanning.menuId);
            }

            /*
            ** Set index to sort.
            */
            $scope.setIndex = function (index) {
                $scope.selectedIndex = index;
            };

            /*
            ** Upward or downward food item.
            */
            $scope.upFoodItems = function (selectedIndex) {
                mealPlanningUtilities.upFoodItems(selectedIndex, $scope);
            };

            $scope.downFoodItems = function (selectedIndex) {
                mealPlanningUtilities.downFoodItems(selectedIndex, $scope);
            };

            /*
            ** Create meal planning.
            */
            $scope.save = function () {
                $scope.disabledSubmitButton = true;
                $scope.mealPlanning.foodId = mealPlanningUtilities.getFoodId($scope.foodItems);
                $scope.mealPlanning.onDate = $scope.mealPlanning.date;
                var arrId = getIdFromOptions($scope.menuOptions); //menuId
                $scope.mealPlanning.menuId = arrId;

                if ($scope.mealPlanning.onDate instanceof Date) {
                    $scope.mealPlanning.onDate = $scope.mealPlanning.onDate.toDateString();
                }

                if (mealPlanningUtilities.validateMealPlanning($scope.mealPlanning)) {
                    mealPlanningFactory.insertMealPlanning($scope.mealPlanning)
                        .success(function (response) {
                            $scope.disabledSubmitButton = false;

                            if (response.IsSuccessful) {
                                notificationFactory.success("DataChangeSuccessfully");
                                $modalInstance.close();
                            }
                            else {
                                notificationFactory.error("", response.Message);
                            }
                        }).error(function (response, status) {
                            $scope.disabledSubmitButton = false;
                            ErrorFactory.errorHandle(status);
                        });
                }
                else {
                    $scope.disabledSubmitButton = false;
                }
            };

            /*
            ** Create and continue create meal planning.
            */
            $scope.saveAndContinue = function () {
                $scope.disabledSubmitButton = true;
                $scope.mealPlanning.foodId = mealPlanningUtilities.getFoodId($scope.foodItems);
                $scope.mealPlanning.onDate = $scope.mealPlanning.date;
                var arrId = getIdFromOptions($scope.menuOptions); //menuId
                $scope.mealPlanning.menuId = arrId;

                if ($scope.mealPlanning.onDate instanceof Date) {
                    $scope.mealPlanning.onDate = $scope.mealPlanning.onDate.toDateString();
                }

                if (mealPlanningUtilities.validateMealPlanning($scope.mealPlanning)) {
                    mealPlanningFactory.insertMealPlanning($scope.mealPlanning)
                        .success(function (response) {
                            if (response.IsSuccessful) {
                                $scope.foodItems = [];
                                $scope.$broadcast('resetMealPlanning');
                                notificationFactory.success("DataChangeSuccessfully");
                            }
                            else {
                                notificationFactory.error("", response.Message);
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
                $modalInstance.dismiss("cancel");
            };
        }]);