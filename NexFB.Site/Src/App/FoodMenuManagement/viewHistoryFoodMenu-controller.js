'use strict';
/*global foodMenuManagementModule*/

foodMenuManagementModule.controller('ViewHistoryFoodMenuController',
    ['$scope', '$modalInstance', 'menuId', 'foodMenuUtilities', 'foodMenuFactory', 'notificationFactory', 'ErrorFactory', 'commonFactory',
        function ($scope, $modalInstance, menuId, foodMenuUtilities, foodMenuFactory, notificationFactory, ErrorFactory, commonFactory) {
            var getFoodMenuHistories = function (menuId) {
                foodMenuFactory.getFoodMenuHistory(menuId).success(function (histories) {
                    $scope.histories = histories;

                    var currentFoodMenu = histories.length > 0 ? histories[0] : null;
                    if (currentFoodMenu) {
                        $scope.foodMenu.currentBeginDate = currentFoodMenu.EffectedDateBegin;
                        $scope.foodMenu.currentEndDate = currentFoodMenu.EffectedDateEnd;
                        $scope.foodMenu.currentPricePerMeal = currentFoodMenu.PricePerMeal;
                        $scope.foodMenu.currentPricePerDay = currentFoodMenu.PricePerDay;
                    }
                }).error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                });
            };

            getFoodMenuHistories(menuId);

            $scope.foodMenu = {
                menuId: menuId,
                currentBeginDate: new Date(),
                currentEndDate: new Date(),
                currentPricePerMeal: 0,
                currentPricePerDay: 0,
                newBeginDate: null,
                newEndDate: null,
                newPricePerMeal: 0,
                newPricePerDay: 0
            };

            $scope.calendar = {
                currentEffectedEndDate: false
            };

            $scope.openCurrentEffectedEndDateCalendar = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();

                $scope.calendar.currentEffectedEndDate = true;
            };

            $scope.onChangeCurrentEndDate = function () {
                var endDate = new Date($scope.foodMenu.currentEndDate);
                endDate.setDate(endDate.getDate() + 1);
                $scope.foodMenu.newBeginDate = endDate;
            };

            $scope.onUpdateFoodMenu = function (foodMenu) {
                if (foodMenuUtilities.validateFoodMenuHistory(foodMenu)) {
                    foodMenuFactory.insertFoodMenuHistory(foodMenu).success(function (result) {
                        if (result.IsSuccessful) {
                            notificationFactory.success('', "Update food menu successfully!");
                            getFoodMenuHistories(foodMenu.menuId);
                        } else {
                            notificationFactory.error("", result.Message);
                        }
                    }).error(function (response, status) {
                        ErrorFactory.errorHandle(status);
                    });
                }
            };

            $scope.cancel = function () {
                $modalInstance.dismiss("cancel");
            };
        }
    ]);