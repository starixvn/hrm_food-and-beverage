'use strict';
/*global foodMenuManagementModule*/

foodMenuManagementModule.factory('foodMenuFactory', ['$http', function ($http) {
	return {
		getAllFoodMenu: function (mealId, employeeTypeId) {
			return $http.get('/api/FoodMenuManagement/GetAllFoodMenu',
				{ params: { mealId: mealId, employeeTypeId: employeeTypeId} });
		},
		getAllFoodMenuToFilter: function () {
			return $http.get('/api/FoodMenuManagement/GetAllFoodMenuToFilter');
		},
		insertFoodMenu: function (foodMenu) {
			return $http.post('/api/FoodMenuManagement/InsertFoodMenu', foodMenu);
		},
		getAllMealTimesheet: function () {
			return $http.get('/api/Common/GetAllMealTimesheet');
		},
		getAllEmployeeType: function () {
			return $http.get('/api/Common/GetAllEmployeeType');
		}
	};
} ]);