'use strict';
/*global mealPlanningManagementModule*/

mealPlanningManagementModule.factory('mealPlanningFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
    return {
        getAllMealPlanning: function (fromDate, toDate, mealId, employeeTypeId) {
            return $http.get(baseUrl + 'api/MealPlanning/GetAllMealPlanning',
                { params: { fromDate: fromDate, toDate: toDate, mealId: mealId, employeeTypeId: employeeTypeId } });
        },
        insertMealPlanning: function (mealPlanning) {
            return $http.post(baseUrl + 'api/MealPlanning/InsertMealPlanning', mealPlanning);
        },
        updateMealPlanning: function (mealPlanning) {
            return $http.put(baseUrl + 'api/MealPlanning/UpdateMealPlanning', mealPlanning);
        },
        deleteMealPlanning: function (menuId, onDate) {
            return $http['delete'](baseUrl + 'api/MealPlanning/DeleteMealPlanning',
                { params: { menuId: menuId, date: onDate } });
        },
        activateMealPlanning: function (mealPlanning) {
            return $http.put(baseUrl + 'api/MealPlanning/ActivateMealPlanning', mealPlanning);
        },
        getUserPermissionOnMealPlanning: function () {
            return $http.get(baseUrl + 'api/MealPlanning/GetUserPermissionOnMealPlanning');
        },
        getAllMealTimesheet: function () {
            return $http.get(baseUrl + 'api/Common/GetAllMealTimesheet');
        },
        getFoodByOnDate: function (date, menuGroupId) {
            return $http.get(baseUrl + 'api/MealPlanning/GetFoodByOnDate',
                { params: { date: date, menuGroupId: menuGroupId } });
        },
        checkDisableMealPlanning: function (onDate, mealId, employeeTypeId, menuId) {
            return $http.get(baseUrl + 'api/MealPlanning/CheckDisableMealPlanning',
                { params: { date: onDate, mealId: mealId, employeeTypeId: employeeTypeId, menuId: menuId } });
        }
    };
}]);

mealPlanningManagementModule.factory('mealPlanningUtilities', [
    'notificationFactory',
    function (notificationFactory) {
        var utilities = {};

        utilities.validateMealPlanning = function (mealPlanning) {
            var valid = true;

            if (!mealPlanning) {
                return false;
            }
            else {
                if (!mealPlanning.onDate) {
                    notificationFactory.error("DateRequire");
                    valid = false;
                }
                if (!mealPlanning.menuId) {
                    notificationFactory.error("MenuRequire");
                    valid = false;
                }
            }

            return valid;
        };

        utilities.getFoodId = function (foodItems) {
            var foodId = '';

            angular.forEach(foodItems, function (food) {
                var id = food.FoodId;

                foodId = foodId !== '' ? foodId + ',' + id : id;
            });

            return foodId;
        };

        /*
        ** Upward or downward food item.
        */
        utilities.upFoodItems = function (selectedIndex, scope) {
            if (selectedIndex > 0) {
                var currentItem = angular.copy(scope.foodItems[selectedIndex]);

                scope.foodItems[selectedIndex] = angular.copy(scope.foodItems[selectedIndex - 1]);
                scope.foodItems[selectedIndex - 1] = currentItem;

                scope.selectedIndex = selectedIndex - 1;
            }
        };

        utilities.downFoodItems = function (selectedIndex, scope) {
            if (selectedIndex < scope.foodItems.length - 1) {
                var currentItem = angular.copy(scope.foodItems[selectedIndex]);

                scope.foodItems[selectedIndex] = angular.copy(scope.foodItems[selectedIndex + 1]);
                scope.foodItems[selectedIndex + 1] = currentItem;

                scope.selectedIndex = selectedIndex + 1;
            }
        };

        utilities.createMealId = function (meals) {
            var mealId = '';

            angular.forEach(meals, function (meal) {
                mealId = mealId !== '' ? mealId + ',' + meal.MealId : meal.MealId;
            });

            return mealId;
        };

        utilities.createEmployeeTypeId = function (employeeTypes) {
            var employeeTypeId = '';

            angular.forEach(employeeTypes, function (employeeType) {
                employeeTypeId = employeeTypeId !== '' ? employeeTypeId + ',' + employeeType.EmployeeTypeId : employeeType.EmployeeTypeId;
            });

            return employeeTypeId;
        };

        return utilities;
    }]);