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
		if (!food) {
			return false;
		}
		else {
			if (!food.foodVnmName) {
				return false;
			}

			if (!food.foodEngName) {
				return false;
			}
		}

		return true;
	};

	utility.showMessageValidateFoodItem = function (scope) {
		scope.$watchCollection('[submitted, formScope.form.$invalid, \
			formScope.form.vnmName.$error.required, formScope.form.engName.$error.required]', function (newVals) {
				if (newVals[0] && newVals[1]) {
					if (newVals[3]) {
						notificationFactory.error("FoodEngNameRequire");
					}
					if (newVals[2]) {
						notificationFactory.error("FoodVnmNameRequire");
					}

					scope.submitted = false;
				}
			});
	};

	utility.createMenuGroupId = function (setMenuIds) {
		var setMenuId = '';

		angular.forEach(setMenuIds, function (tempSetMenuId) {
			setMenuId = setMenuId !== '' ? setMenuId + ',' + tempSetMenuId : tempSetMenuId;
		});

		return setMenuId;
	};

	return utility;
} ]);