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

mealPlanningManagementModule.factory('mealPlanningUtilities', [function () {
	var utilities = {};

	utilities.validateMealPlanning = function (mealPlanning) {
		if (!mealPlanning) {
			return false;
		}
		else {
			if (!mealPlanning.menuId) {
				return false;
			}
			if (!mealPlanning.onDate) {
				return false;
			}
		}

		return true;
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