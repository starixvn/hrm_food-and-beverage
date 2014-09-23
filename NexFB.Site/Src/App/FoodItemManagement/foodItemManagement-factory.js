'use strict';
/*global foodItemManagementModule*/

foodItemManagementModule.factory('foodItemFactory', ['$http', function ($http) {
	return {
		getAllFoodItem: function (menuGroupId, isActive) {
			return $http.get('/api/FoodItemManagement/GetAllFoodItem',
				{ params: { menuGroupId: menuGroupId, isActive: isActive } });
		}
	};
} ]);