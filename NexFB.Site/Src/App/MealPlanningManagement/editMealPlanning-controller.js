'use strict';
/*global mealPlanningManagementModule*/

mealPlanningManagementModule.controller('EditMealPlanningController',
    ['$scope', '$modalInstance', 'mealPlanning', 'menuOptions', 'foodOptions', 'commonFactory',
        'mealPlanningFactory', 'setMenuFactory', 'mealPlanningUtilities', 'ErrorFactory', 'mealTimesheets',
        'notificationFactory', 'isOriginalActive',
        function ($scope, $modalInstance, mealPlanning, menuOptions, foodOptions, commonFactory, mealPlanningFactory,
            setMenuFactory, mealPlanningUtilities, ErrorFactory, mealTimesheets, notificationFactory, isOriginalActive) {
            /*
            ** Declare variables.
            */
            $scope.mealPlanning = mealPlanning;
            $scope.menuOptions = menuOptions;
            $scope.foodItems = mealPlanning.SetMenu.FoodItems;
            $scope.mealTimesheets = mealTimesheets;
            $scope.foodOptions = foodOptions;
            $scope.isOriginalActive = isOriginalActive;

            $scope.calendar = {
                startDateOpened: false
            };

            /*
            ** set default check when open dialog
             */
            var setDefaultCheck = function (items, id) {
                angular.forEach(items, function (item) {
                    if (item.MenuId === id) {
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
            setDefaultCheck($scope.menuOptions, $scope.mealPlanning.Menu.MenuId);

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
                $scope.mealPlanning.rawFoodIds = [];
                angular.forEach($scope.setMenuItems, function (setMenu) {
                    if (setMenu.SetMenuId === setMenuId) {
                        angular.forEach(setMenu.FoodItems, function (food) {
                            $scope.foodItems.push(food);
                            $scope.mealPlanning.rawFoodIds.push(food.FoodId);
                        });
                    }
                });
            };

            /*
            ** Get all set menu to filter.
            */
            setMenuFactory.getSetMenu($scope.mealPlanning.Menu.MenuId, true).success(function (setMenus) {
                $scope.setMenuItems = setMenus;
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            /*
            ** Create list food item Id.
            */
            var createFoodIds = function (foodItems) {
                var foodIds = [];

                angular.forEach(foodItems, function (foodItem) {
                    foodIds.push(foodItem.FoodId);
                });

                return foodIds;
            };

            $scope.mealPlanning.rawFoodIds = createFoodIds($scope.mealPlanning.SetMenu.FoodItems);

            /*
            ** Get all food items of current menu.
            */
            mealPlanningFactory.getFoodByOnDate(mealPlanning.Date, mealPlanning.Menu.MenuId)
                .success(function (foodItems) {
                    $scope.groupOptions = foodItems;
                }).error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                });

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
            ** Edit meal planning.
            */
            $scope.editMealPlanning = function (mealPlanning) {
                var arrId = getIdFromOptions($scope.menuOptions); //menuId
                $scope.disabledSubmitButton = true;

                var updateMealPlanning = {
                    menuId: arrId,// mealPlanning.Menu.MenuId,
                    setMenuId: mealPlanning.SetMenu.SetMenuId,
                    onDate: mealPlanning.Date,
                    isActive: mealPlanning.IsActive,
                    foodId: mealPlanningUtilities.getFoodId($scope.foodItems)
                };

                var getSelectedSetMenu = function (setMenuId, setMenuItems) {
                    var selectedSetMenu = '';

                    angular.forEach(setMenuItems, function (setMenu) {
                        if (setMenu.SetMenuId === setMenuId) {
                            selectedSetMenu = setMenu;
                        }
                    });

                    return selectedSetMenu;
                };

                var updateMealPlanningDetail = function (mealPlanning, updateMealPlanning) {
                    var selectedSetMenu = getSelectedSetMenu(mealPlanning.SetMenu.SetMenuId, $scope.setMenuItems);

                    var informationMealPlanning = {
                        updateMealPlanning: updateMealPlanning,
                        setMenu: selectedSetMenu,
                        foodItems: $scope.foodItems
                    };

                    if (mealPlanningUtilities.validateMealPlanning(updateMealPlanning)) {
                        mealPlanningFactory.updateMealPlanning(informationMealPlanning.updateMealPlanning)
                            .success(function (response) {
                                if (response.IsSuccessful) {
                                    notificationFactory.success("DataChangeSuccessfully");
                                    $modalInstance.close(informationMealPlanning);
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

                var mealId = mealPlanningUtilities.createMealId(mealPlanning.Menu.Meals),
                    employeeTypeId = mealPlanningUtilities.createEmployeeTypeId(mealPlanning.Menu.EmployeeTypes);

                if ($scope.isOriginalActive !== mealPlanning.IsActive) {
                    mealPlanningFactory.checkDisableMealPlanning(mealPlanning.Date.toString(), mealId, employeeTypeId, mealPlanning.Menu.MenuId)
                        .success(function (isDisable) {
                            if (isDisable === 'true') {
                                var action = !mealPlanning.IsActive ? "activate" : "deactivate";
                                mealPlanning.IsActive = !updateMealPlanning.IsActive;
                                mealPlanning.IsDisable = true;

                                if (action === 'activate') {
                                    notificationFactory.error("MealPlanningActivateWarning");
                                }
                                else {
                                    notificationFactory.error("MealPlanningDeactivateWarning");
                                }
                            }
                            else {
                                updateMealPlanningDetail(mealPlanning, updateMealPlanning);
                            }
                        }).error(function (response, status) {
                            ErrorFactory.errorHandle(status);
                        })['finally'](function () {
                            $scope.disabledSubmitButton = false;
                        });
                }
                else {
                    updateMealPlanningDetail(mealPlanning, updateMealPlanning);
                }
            };

            $scope.cancel = function () {
                $modalInstance.dismiss("cancel");
            };
        }]);