'use strict';
/*global mealPlanningManagementModule*/

mealPlanningManagementModule.factory('mealPlanningFactory', ['$http', function ($http) {
	return {
		getAllMealPlanning: function (fromDate, toDate, mealId, employeeTypeId) {
			return $http.get('/api/MealPlanning/GetAllMealPlanning',
				{ params: { fromDate: fromDate, toDate: toDate, mealId: mealId, employeeTypeId: employeeTypeId} });
		},
		getBasicFoodItem: function (foodId, menuGroupId, isActive) {
			return $http.get('/api/FoodItemManagement/GetBasicFoodItem',
				{ params: { foodId: foodId, menuGroupId: menuGroupId, isActive: isActive} });
		},
		insertMealPlanning: function (mealPlanning) {
			return $http.post('/api/MealPlanning/InsertMealPlanning', mealPlanning);
		}
	};
} ]);

mealPlanningManagementModule.factory('mealPlanningUtilities', [
'notificationFactory', function (notificationFactory) {
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

	return utilities;
} ]);