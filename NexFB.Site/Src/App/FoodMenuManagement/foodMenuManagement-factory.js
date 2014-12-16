'use strict';
/*global foodMenuManagementModule*/

foodMenuManagementModule.factory('foodMenuFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		getAllFoodMenu: function (mealId, employeeTypeId, isActive) {
			return $http.get(baseUrl + 'api/FoodMenuManagement/GetAllFoodMenu',
				{ params: { mealId: mealId, employeeTypeId: employeeTypeId, isActive: isActive} });
		},
		getAllFoodMenuToFilter: function () {
			return $http.get(baseUrl + 'api/FoodMenuManagement/GetAllFoodMenuToFilter');
		},
		insertFoodMenu: function (foodMenu) {
			return $http.post(baseUrl + 'api/FoodMenuManagement/InsertFoodMenu', foodMenu);
		},
		getAllMealTimesheet: function () {
			return $http.get(baseUrl + 'api/Common/GetAllMealTimesheet');
		},
		getAllEmployeeType: function (departmentId, isLocal) {
			return $http.get(baseUrl + 'api/Common/GetAllEmployeeType', 
				{ params: { departmentId: departmentId, isLocal: isLocal } });
		},
		deleteFoodMenu: function (menuId) {
			return $http['delete'](baseUrl + 'api/FoodMenuManagement/DeleteFoodMenu', { params: { menuId: menuId} });
		},
		updateFoodMenu: function (foodMenu) {
			return $http.put(baseUrl + 'api/FoodMenuManagement/UpdateFoodMenu', foodMenu);
		},
		activateFoodMenu: function (foodMenu) {
			return $http.put(baseUrl + 'api/FoodMenuManagement/ActivateFoodMenu', foodMenu);
		},
		getUserPermissionOnFoodMenu: function () {
			return $http.get(baseUrl + 'api/FoodMenuManagement/GetUserPermissionOnFoodMenu');
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
			if ((foodMenu.pricePerMeal === "" && foodMenu.pricePerDay === "") ||
					(foodMenu.pricePerMeal === undefined && foodMenu.pricePerDay === undefined)) {
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
			if (value.ticked) {
				var id = value.MealId.toString();
				mealId = mealId !== '' ? mealId + ',' + id : id;
			}
		});

		return mealId;
	};

	utility.getEmployeeTypeId = function (employeeTypesOption) {
		var employeeTypeId = '';

		angular.forEach(employeeTypesOption, function (value) {
			if (value.ticked) {
				var id = value.EmployeeTypeId.toString();
				employeeTypeId = employeeTypeId !== '' ? employeeTypeId + ',' + id : id;
			}
		});

		return employeeTypeId;
	};

	utility.createSetMenuId = function (rawSetMenuIds) {
		var setMenuId = '';

		angular.forEach(rawSetMenuIds, function (rawSetMenuId) {
			setMenuId = setMenuId !== '' ? setMenuId + ',' + rawSetMenuId : rawSetMenuId;
		});

		return setMenuId;
	};

	return utility;
} ]);