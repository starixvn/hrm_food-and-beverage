'use strict';
/*global foodMenuManagementModule*/

foodMenuManagementModule.factory('foodMenuFactory', ['$http', function ($http) {
	return {
		getAllFoodMenuToFilter: function () {
			return $http.get('/api/FoodMenuManagement/GetAllFoodMenuToFilter');
		}
	};
} ]);