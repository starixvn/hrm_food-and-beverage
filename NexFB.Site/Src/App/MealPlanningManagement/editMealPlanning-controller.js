'use strict';
/*global mealPlanningManagementModule*/

mealPlanningManagementModule.controller('EditMealPlanningController',
['$scope', '$modalInstance', 'mealPlanning', 'menuOptions', 'foodOptions', 'commonFactory', 
'mealPlanningFactory', 'setMenuFactory',
'mealPlanningUtilities', 'ErrorFactory', 'mealTimesheets', 'notificationFactory',
function ($scope, $modalInstance, mealPlanning, menuOptions, foodOptions, commonFactory, mealPlanningFactory,
setMenuFactory, mealPlanningUtilities, ErrorFactory, mealTimesheets, notificationFactory) {
	/*
	** Declare variables.
	*/
	$scope.mealPlanning = mealPlanning;
	$scope.menuOptions = menuOptions;
	$scope.foodItems = mealPlanning.SetMenu.FoodItems;
	$scope.mealTimesheets = mealTimesheets;
	$scope.foodOptions = foodOptions;

	$scope.calendar = {
		startDateOpened: false
	};

	/*
	** Open calendar.
	*/
	$scope.openPopupStartDate = function ($event) {
		commonFactory.openPopupStartDate($event, $scope);
	};

	/*
	** Get food items by set menu.
	*/
	$scope.getFoodItemsBySetMenu = function (setMenuId) {
		$scope.foodItems = [];
		$scope.mealPlanning.rawFoodIds = [];
		angular.forEach($scope.setMenuItems, function (setMenu) {
			if (setMenu.SetMenuId === setMenuId) {
				angular.forEach(setMenu.FoodItems, function (food) {
					$scope.foodItems.push(food);
					$scope.mealPlanning.rawFoodIds.push(food.FoodId);
				});
			}
		});
	};

	/*
	** Get all set menu to filter.
	*/
	setMenuFactory.getSetMenu(null, true).success(function (setMenus) {
		$scope.setMenuItems = setMenus;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Create list food item Id.
	*/
	var createFoodIds = function (foodItems) {
		var foodIds = [];

		angular.forEach(foodItems, function (foodItem) {
			foodIds.push(foodItem.FoodId);
		});

		return foodIds;
	};

	$scope.mealPlanning.rawFoodIds = createFoodIds($scope.mealPlanning.SetMenu.FoodItems);

	/*
	** Get all food items of current menu.
	*/
	mealPlanningFactory.getFoodByOnDate(mealPlanning.Date, mealPlanning.Menu.MenuId)
	.success(function (foodItems) {
		$scope.groupOptions = foodItems;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Set index to sort.
	*/
	$scope.setIndex = function (index) {
		$scope.selectedIndex = index;
	};

	/*
	** Upward or downward food item.
	*/
	$scope.upFoodItems = function (selectedIndex) {
		mealPlanningUtilities.upFoodItems(selectedIndex, $scope);
	};

	$scope.downFoodItems = function (selectedIndex) {
		mealPlanningUtilities.downFoodItems(selectedIndex, $scope);
	};

	/*
	** Check overdue meal planning.
	*/
	var checkOverdueMeal = function (mealPlanning) {
		var disable = false,
			numberMeals = mealPlanning.Menu.Meals.length,
			lastMeal = numberMeals > 0 ? mealPlanning.Menu.Meals[numberMeals - 1] : '';

		if (lastMeal) {
			disable = mealPlanningUtilities.checkMealTimesheet($scope.mealTimesheets, lastMeal, mealPlanning.Date);
		}

		return disable;
	};

	/*
	** Edit meal planning.
	*/
	$scope.editMealPlanning = function (mealPlanning) {
		$scope.disabledSubmitButton = true;

		var updateMealPlanning = {
			menuId: mealPlanning.Menu.MenuId,
			setMenuId: mealPlanning.SetMenu.SetMenuId,
			onDate: mealPlanning.Date,
			isActive: mealPlanning.IsActive,
			foodId: mealPlanningUtilities.getFoodId($scope.foodItems)
		};

		var disable = checkOverdueMeal(mealPlanning);

		if (disable) {
			var action = !mealPlanning.IsActive ? "activate" : "deactivate";
			mealPlanning.IsActive = !updateMealPlanning.IsActive;
			mealPlanning.IsDisable = disable;

			if (action === 'activate') {
				notificationFactory.error("MealPlanningActivateWarning");
			}
			else {
				notificationFactory.error("MealPlanningDeactivateWarning");
			}
		}
		else {
			var informationMealPlanning = {
				updateMealPlanning: updateMealPlanning,
				foodItems: $scope.foodItems
			};

			if (mealPlanningUtilities.validateMealPlanning(updateMealPlanning)) {
				mealPlanningFactory.updateMealPlanning(informationMealPlanning.updateMealPlanning)
				.success(function (response) {
					$scope.disabledSubmitButton = false;

					if (response.IsSuccessful) {
						notificationFactory.success("DataChangeSuccessfully");
						$modalInstance.close(informationMealPlanning);
					}
					else {
						notificationFactory.error("", response.Message);
					}
				}).error(function (response, status) {
					$scope.disabledSubmitButton = false;

					ErrorFactory.errorHandle(status);
				});
			}
			else {
				$scope.disabledSubmitButton = false;
			}
		}
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);