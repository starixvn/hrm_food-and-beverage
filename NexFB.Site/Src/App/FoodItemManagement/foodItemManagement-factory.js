'use strict';
/*global foodItemManagementModule*/

foodItemManagementModule.factory('foodItemFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		getAllFoodItem: function (setMenuId, isActive) {
			return $http.get(baseUrl + 'api/FoodItemManagement/GetAllFoodItem',
				{ params: { setMenuId: setMenuId, isActive: isActive} });
		},
		insertFoodItem: function (food) {
			return $http.post(baseUrl + 'api/FoodItemManagement/InsertFoodItem', food);
		},
		updateFoodItem: function (food) {
			return $http.put(baseUrl + 'api/FoodItemManagement/UpdateFoodItem', food);
		},
		updateStatusFoodItem: function (food) {
			return $http.put(baseUrl + 'api/FoodItemManagement/UpdateStatusFoodItem', food);
		},
		deleteFoodItem: function (foodId) {
			return $http['delete'](baseUrl + 'api/FoodItemManagement/DeleteFoodItem', { params: { foodId: foodId} });
		},
		getPermissonFoodItem: function () {
			return $http.get(baseUrl + 'api/FoodItemManagement/GetUserPermissionOnFoodItem');
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