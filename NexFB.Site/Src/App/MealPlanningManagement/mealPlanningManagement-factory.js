'use strict';
/*global mealPlanningManagementModule*/

mealPlanningManagementModule.factory('mealPlanningFactory', ['$http', function ($http) {
	return {
		getAllMealPlanning: function (fromDate, toDate, mealId, employeeTypeId) {
			return $http.get('/api/MealPlanning/GetAllMealPlanning',
				{ params: { fromDate: fromDate, toDate: toDate, mealId: mealId, employeeTypeId: employeeTypeId} });
		},
//		getBasicFoodItem: function (foodId, menuGroupId, isActive) {
//			return $http.get('/api/FoodItemManagement/GetBasicFoodItem',
//				{ params: { foodId: foodId, menuGroupId: menuGroupId, isActive: isActive} });
//		},
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
		},
		getFoodByOnDate: function (date, menuGroupId) {
			return $http.get('/api/MealPlanning/GetFoodByOnDate', 
				{ params: { date: date, menuGroupId: menuGroupId } });
		}
	};
} ]);

mealPlanningManagementModule.factory('mealPlanningUtilities', [
'notificationFactory',
function (notificationFactory) {
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

//	utilities.getBasicFoodItems = function (foodId, menuId, isActive, scope) {
//		mealPlanningFactory.getBasicFoodItem(foodId, menuId, isActive).success(function (foodItems) {
//			scope.groupOptions = foodItems;
//		}).error(function (response, status) {
//			if (status === 405) {
//				ErrorFactory.errorHandle(status);
//			}
//			else {
//				notificationFactory.error("ErrorMessage");
//			}
//		});
//	};

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

	utilities.checkRightMeal = function (currentTimesheet, onDate) {
		if (onDate) {
			var toDay = new Date(),
				currentOnDate = new Date(onDate);

			if (toDay.getMonth() === currentOnDate.getMonth() && toDay.getDate() === currentOnDate.getDate() && 
					toDay.getFullYear() === currentOnDate.getFullYear()) {
				var currentHour = toDay.getHours(),
					currentMinute = toDay.getMinutes(),
					startHour = new Date(currentTimesheet.StartTime + 'Z').getUTCHours(),
					startMinute = new Date(currentTimesheet.StartTime + 'Z').getUTCMinutes();

				if (currentHour > startHour) {
					return false;
				}
				else if (currentHour === startHour && currentMinute > startMinute) {
					return false;
				}
			}
		}

		return true;
	};

	var getCurrentTimesheet = function (mealTimesheet, mealId) {
		var currentTimesheet = '';

		angular.forEach(mealTimesheet, function (timesheetItem) {
			if (timesheetItem.MealId === mealId) {
				currentTimesheet = timesheetItem;
			}
		});

		return currentTimesheet;
	};

	utilities.checkMealTimesheet = function (mealTimesheets, meal, onDate) {
		var currentTimesheet = getCurrentTimesheet(mealTimesheets, meal.MealId),
			rightMeal = utilities.checkRightMeal(currentTimesheet, onDate);

		return !rightMeal;
	};

	return utilities;
} ]);