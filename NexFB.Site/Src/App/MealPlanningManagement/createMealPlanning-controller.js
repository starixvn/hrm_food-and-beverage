'use strict';
/*global mealPlanningManagementModule*/

mealPlanningManagementModule.controller('CreateMealPlanningController',
['$scope', '$modalInstance', 'mealPlanning', 'mealPlanningFactory', 'ErrorFactory', 'notificationFactory',
'menuOptions', 'foodOptions', 'commonFactory', 'setMenuFactory', 'mealPlanningUtilities',
'currentMealPlanning',
function ($scope, $modalInstance, mealPlanning, mealPlanningFactory, ErrorFactory, notificationFactory,
menuOptions, foodOptions, commonFactory, setMenuFactory, mealPlanningUtilities,
currentMealPlanning) {

	$scope.menuOptions = menuOptions;
	$scope.foodItems = [];
	$scope.currentMealPlanning = currentMealPlanning;
	$scope.foodOptions = foodOptions;
	$scope.rawFoodIds = [];


	$scope.calendar = {
		startDateOpened: false
	};

	$scope.mealPlanning = {
		menuId: mealPlanning !== '' ? mealPlanning.Menu.MenuId : '',
		setMenuId: mealPlanning !== '' ? mealPlanning.SetMenu.SetMenuId : '',
		onDate: '',
		isActive: true, //mealPlanning !== '' ? mealPlanning.IsActive : '',
		date: mealPlanning !== '' ? mealPlanning.Date : new Date()
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
		$scope.rawFoodIds = [];
		angular.forEach($scope.setMenuItems, function (setMenu) {
			if (setMenu.SetMenuId === setMenuId) {
				angular.forEach(setMenu.FoodItems, function (food) {
					$scope.foodItems.push(food);
					$scope.rawFoodIds.push(food.FoodId);
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
	** Create meal planning.
	*/
	$scope.save = function () {
		$scope.disabledSubmitButton = true;
		$scope.mealPlanning.foodId = mealPlanningUtilities.getFoodId($scope.foodItems);
		$scope.mealPlanning.onDate = $scope.mealPlanning.date;

		if ($scope.mealPlanning.onDate instanceof Date) {
			$scope.mealPlanning.onDate = $scope.mealPlanning.onDate.toDateString();
		}

		if (mealPlanningUtilities.validateMealPlanning($scope.mealPlanning)) {
			mealPlanningFactory.insertMealPlanning($scope.mealPlanning)
			.success(function (response) {
				$scope.disabledSubmitButton = false;

				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
					$modalInstance.close();
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
	};

	/*
	** Create and continue create meal planning.
	*/
	$scope.saveAndContinue = function () {
		$scope.disabledSubmitButton = true;
		$scope.mealPlanning.foodId = mealPlanningUtilities.getFoodId($scope.foodItems);
		$scope.mealPlanning.onDate = $scope.mealPlanning.date;

		if ($scope.mealPlanning.onDate instanceof Date) {
			$scope.mealPlanning.onDate = $scope.mealPlanning.onDate.toDateString();
		}

		if (mealPlanningUtilities.validateMealPlanning($scope.mealPlanning)) {
			mealPlanningFactory.insertMealPlanning($scope.mealPlanning)
			.success(function (response) {
				if (response.IsSuccessful) {
					$scope.foodItems = [];
					$scope.$broadcast('resetMealPlanning');
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			})['finally'](function () {
				$scope.disabledSubmitButton = false;
			});
		}
		else {
			$scope.disabledSubmitButton = false;
		}
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);