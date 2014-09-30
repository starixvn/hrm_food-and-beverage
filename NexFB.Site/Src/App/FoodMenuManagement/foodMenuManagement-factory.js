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
		},
		deleteFoodMenu: function (menuId) {
			return $http['delete']('/api/FoodMenuManagement/DeleteFoodMenu', { params: { menuId: menuId} });
		},
		updateFoodMenu: function (foodMenu) {
			return $http.put('/api/FoodMenuManagement/UpdateFoodMenu', foodMenu);
		},
		activateFoodMenu: function (foodMenu) {
			return $http.put('/api/FoodMenuManagement/ActivateFoodMenu', foodMenu);
		}
	};
} ]);

foodMenuManagementModule.factory('foodMenuUtilities', ['notificationFactory',
function (notificationFactory) {
	var utility = {};

	utility.validateFoodMenu = function (foodMenu) {
		var valid = true;

		if (!foodMenu) {
			return false;
		}
		else {
			if (!foodMenu.pricePerMeal && !foodMenu.pricePerDay) {
				notificationFactory.error("FoodMenuPriceRequire");
				valid = false;
			}
			if (!foodMenu.employeeTypeId) {
				notificationFactory.error("FoodMenuEmployeeTypeRequire");
				valid = false;
			}
			if (!foodMenu.mealId) {
				notificationFactory.error("FoodMenuMealRequire");
				valid = false;
			}
			if (!foodMenu.menuEngName) {
				notificationFactory.error("FoodMenuEngNameRequire");
				valid = false;
			}
			if (!foodMenu.menuVnmName) {
				notificationFactory.error("FoodMenuVnmNameRequire");
				valid = false;
			}
		}

		return valid;
	};

	utility.getMealId = function (mealsOption) {
		var mealId = '';

		angular.forEach(mealsOption, function (value) {
			if (value.ticked === true) {
				var id = value.MealId.toString();
				mealId = mealId !== '' ? mealId + ',' + id : id;
			}
		});

		return mealId;
	};

	utility.getEmployeeTypeId = function (employeeTypesOption) {
		var employeeTypeId = '';

		angular.forEach(employeeTypesOption, function (value) {
			if (value.ticked === true) {
				var id = value.EmployeeTypeId.toString();
				employeeTypeId = employeeTypeId !== '' ? employeeTypeId + ',' + id : id;
			}
		});

		return employeeTypeId;
	};

	utility.createFoodId = function (rawFoodIds) {
		var foodId = '';

		angular.forEach(rawFoodIds, function (rawFoodId) {
			foodId = foodId !== '' ? foodId + ',' + rawFoodId : rawFoodId;
		});

		return foodId;
	};

	return utility;
} ]);