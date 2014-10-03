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
				{ params: { foodId: foodId, menuGroupId: menuGroupId, isActive: isActive } });
		}
	};
} ]);