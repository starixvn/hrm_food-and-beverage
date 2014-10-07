'use strict';
/*global mealPlanningManagementModule*/

mealPlanningManagementModule.factory('mealPlanningFactory', ['$http', function ($http) {
	return {
		getAllMealPlanning: function (fromDate, toDate, mealId, employeeTypeId) {
			return $http.get('/api/MealPlanning/GetAllMealPlanning',
				{ params: { fromDate: fromDate, toDate: toDate, mealId: mealId, employeeTypeId: employeeTypeId} });
		},
		getBasicFoodItem: function (foodId, menuGroupId, isActive) {
			return $http.get('/api/FoodItemManagement/GetBasicFoodItem',
				{ params: { foodId: foodId, menuGroupId: menuGroupId, isActive: isActive} });
		},
		insertMealPlanning: function (mealPlanning) {
			return $http.post('/api/MealPlanning/InsertMealPlanning', mealPlanning);
		},
		updateMealPlanning: function (mealPlanning) {
			return $http.put('/api/MealPlanning/UpdateMealPlanning', mealPlanning);
		},
		deleteMealPlanning: function (menuId, onDate) {
			return $http['delete']('/api/MealPlanning/DeleteMealPlanning',
				{ params: { menuId: menuId, date: onDate} });
		},
		activateMealPlanning: function (mealPlanning) {
			return $http.put('/api/MealPlanning/ActivateMealPlanning', mealPlanning);
		},
		getUserPermissionOnMealPlanning: function () {
			return $http.get('/api/MealPlanning/GetUserPermissionOnMealPlanning');
		},
		getAllMealTimesheet: function () {
			return $http.get('/api/Common/GetAllMealTimesheet');
		}
	};
} ]);

mealPlanningManagementModule.factory('mealPlanningUtilities', [
'notificationFactory', 'mealPlanningFactory', 'ErrorFactory', 
function (notificationFactory, mealPlanningFactory, ErrorFactory) {
	var utilities = {};

	utilities.validateMealPlanning = function (mealPlanning) {
		var valid = true;

		if (!mealPlanning) {
			return false;
		}
		else {
			if (!mealPlanning.onDate) {
				notificationFactory.error("DateRequire");
				valid = false;
			}
			if (!mealPlanning.menuId) {
				notificationFactory.error("MenuRequire");
				valid = false;
			}
		}

		return valid;
	};

	utilities.getFoodId = function (foodItems) {
		var foodId = '';

		angular.forEach(foodItems, function (food) {
			var id = food.FoodId;

			foodId = foodId !== '' ? foodId + ',' + id : id;
		});

		return foodId;
	};

	utilities.getBasicFoodItems = function (foodId, menuId, isActive, scope) {
		mealPlanningFactory.getBasicFoodItem(foodId, menuId, isActive).success(function (foodItems) {
			scope.groupOptions = foodItems;
		}).error(function (response, status) {
			if (status === 405) {
				ErrorFactory.errorHandle(status);
			}
			else {
				notificationFactory.error("ErrorMessage");
			}
		});
	};

	/*
	** Upward or downward food item.
	*/
	utilities.upFoodItems = function (selectedIndex, scope) {
		if (selectedIndex > 0) {
			var currentItem = angular.copy(scope.foodItems[selectedIndex]);

			scope.foodItems[selectedIndex] = angular.copy(scope.foodItems[selectedIndex - 1]);
			scope.foodItems[selectedIndex - 1] = currentItem;

			scope.selectedIndex = selectedIndex - 1;
		}
	};

	utilities.downFoodItems = function (selectedIndex, scope) {
		if (selectedIndex < scope.foodItems.length - 1) {
			var currentItem = angular.copy(scope.foodItems[selectedIndex]);

			scope.foodItems[selectedIndex] = angular.copy(scope.foodItems[selectedIndex + 1]);
			scope.foodItems[selectedIndex + 1] = currentItem;

			scope.selectedIndex = selectedIndex + 1;
		}
	};

//	utilities.checkRightMeal = function (currentTimesheet, workDate) {
//		if (workDate) {
//			var toDay = new Date(),
//				currentWorkDate = new Date(workDate);

//			if (toDay.getMonth() === currentWorkDate.getMonth() && toDay.getDate() === currentWorkDate.getDate()) {
//				var currentHour = toDay.getHours(),
//					currentMinute = toDay.getMinutes(),
//					registerHour = new Date(currentTimesheet.RegisterTime + 'Z').getUTCHours(),
//					registerMinute = new Date(currentTimesheet.RegisterTime + 'Z').getUTCMinutes();

//				if (currentHour > registerHour) {
//					return false;
//				}
//				else if (currentHour === registerHour && currentMinute > registerMinute) {
//					return false;
//				}
//			}
//		}

//		return true;
//	};

//	utilities.checkMealTimesheet = function (mealTimesheets, meal, onDate) {
//		var currentTimesheet = getCurrentTimesheet(mealTimesheets, meal.MealId),
//			rightMeal = checkRightMeal(currentTimesheet, onDate);

//		return rightMeal;
//	};

	return utilities;
} ]);