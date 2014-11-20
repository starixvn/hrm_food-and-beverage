'use strict';
/*global foodItemManagementModule*/

foodItemManagementModule.factory('foodItemFactory', ['$http', function ($http) {
	return {
		getAllFoodItem: function (setMenuId, isActive) {
			return $http.get('/api/FoodItemManagement/GetAllFoodItem',
				{ params: { setMenuId: setMenuId, isActive: isActive} });
		},
		insertFoodItem: function (food) {
			return $http.post('/api/FoodItemManagement/InsertFoodItem', food);
		},
		updateFoodItem: function (food) {
			return $http.put('/api/FoodItemManagement/UpdateFoodItem', food);
		},
		updateStatusFoodItem: function (food) {
			return $http.put('/api/FoodItemManagement/UpdateStatusFoodItem', food);
		},
		deleteFoodItem: function (foodId) {
			return $http['delete']('/api/FoodItemManagement/DeleteFoodItem', { params: { foodId: foodId} });
		},
		getPermissonFoodItem: function () {
			return $http.get('/api/FoodItemManagement/GetUserPermissionOnFoodItem');
		}
	};
} ]);

foodItemManagementModule.factory('foodItemUtilities',
['notificationFactory', function (notificationFactory) {
	var utility = {};

	utility.validateFoodItem = function (food) {
		var isValid = true;

		if (!food) {
			isValid = false;
		}
		else {
			if (!food.foodCode) {
				isValid = false;
				notificationFactory.error("FoodCodeRequire");
			}
			if (!food.foodEngName) {
				isValid = false;
				notificationFactory.error("FoodEngNameRequire");
			}
			if (!food.foodVnmName) {
				isValid = false;
				notificationFactory.error("FoodVnmNameRequire");
			}
		}

		return isValid;
	};

	utility.createSetMenuId = function (setMenuIds) {
		var setMenuId = '';

		angular.forEach(setMenuIds, function (tempSetMenuId) {
			setMenuId = setMenuId !== '' ? setMenuId + ',' + tempSetMenuId : tempSetMenuId;
		});

		return setMenuId;
	};

	return utility;
} ]);