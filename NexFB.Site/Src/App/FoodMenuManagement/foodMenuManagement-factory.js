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

foodMenuManagementModule.factory('foodMenuUtilities', [function () {
	var utility = {};

	utility.validateFoodMenu = function (foodMenu) {
		if (!foodMenu) {
			return false;
		}
		else {
			if (!foodMenu.menuVnmName) {
				return false;
			}
			if (!foodMenu.menuEngName) {
				return false;
			}
			if (!foodMenu.mealId) {
				return false;
			}
			if (!foodMenu.employeeTypeId) {
				return false;
			}
			if (!foodMenu.pricePerMeal && !foodMenu.pricePerDay) {
				return false;
			}
		}

		return true;
	};

	utility.getMealId = function (mealsOption) {
		var mealId = '';

		angular.forEach(mealsOption, function (value) {
			if (value.ticked === true) {
				var id = value.MealId;
				mealId = mealId !== '' ? mealId + ',' + id : id;
			}
		});

		return mealId;
	};

	utility.getEmployeeTypeId = function (employeeTypesOption) {
		var employeeTypeId = '';

		angular.forEach(employeeTypesOption, function (value) {
			if (value.ticked === true) {
				var id = value.EmployeeTypeId;
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