'use strict';
/*global mealTimeManagementModule*/

mealTimeManagementModule.factory('mealTimeManagementFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		getAllMealTimesheet: function () {
			return $http.get(baseUrl + 'api/Common/GetAllMealTimesheet');
		},
		getUserPermissionOnMealTimeManagement: function () {
			return $http.get(baseUrl + 'api/MealTimeManagement/GetUserPermissionOnMealTimeManagement');
		},
		updateMealTimesheet: function (mealTimesheet) {
			return $http.put(baseUrl + 'api/MealTimeManagement/UpdateMealTime', mealTimesheet);
		}
	};
} ]);