'use strict';
/*global foodItemManagementModule*/

foodItemManagementModule.controller('EditFoodItemController',
['$scope', '$modalInstance', 'setMenuItems', 'foodItem', 'foodItemUtilities',
function ($scope, $modalInstance, setMenuItems, foodItem, foodItemUtilities) {
	/*
	** Declare variables.
	*/

	$scope.food = foodItem;

	var getSetMenus = function (setMenuItems) {
		var setMenus = [];

		angular.forEach(setMenuItems, function (setMenu) {
			if (setMenu.IsActive) {
				setMenus.push(setMenu);
			}
			else {
				angular.forEach($scope.food.SetMenus, function (foodSetMenu) {
					if (setMenu.SetMenuId === foodSetMenu.SetMenuId) {
						setMenus.push(setMenu);
					}
				});
			}
		});

		return setMenus;
	};

	$scope.setMenuItems = getSetMenus(setMenuItems);

	/*
	** Set scope of form to current scope.
	*/
	$scope.setFormScope = function (formScope) {
		$scope.formScope = formScope;
	};

	/*
	** Create list set menu id.
	*/
	var createSetMenuIds = function (setMenus) {
		var setMenuIds = [];

		angular.forEach(setMenus, function (setMenu) {
			setMenuIds.push(setMenu.SetMenuId);
		});

		return setMenuIds;
	};

	$scope.food.setMenuIds = createSetMenuIds(foodItem.SetMenus);

	/*
	** Update food menus.
	*/
	var findSetMenuById = function (setMenuId, setMenuItems) {
		var setMenu = '';

		for (var i = 0; i < setMenuItems.length; i++) {
			var currentSetMenu = setMenuItems[i];

			if (currentSetMenu.SetMenuId === setMenuId) {
				setMenu = currentSetMenu;
			}
		}

		return setMenu;
	};

	var updateFoodSetMenus = function (food) {
		food.SetMenus = [];

		angular.forEach(food.setMenuIds, function (setMenuId) {
			var addedSetMenu = findSetMenuById(setMenuId, $scope.setMenuItems);
			food.SetMenus.push(addedSetMenu);
		});

		return food;
	};

	/*
	** Update food item.
	*/
	foodItemUtilities.showMessageValidateFoodItem($scope);

	$scope.updateFoodItem = function (food) {
		var editedFood = {
			foodId: food.FoodId,
			foodVnmName: food.FoodVnmName,
			foodEngName: food.FoodEngName,
			isActive: food.IsActive,
			setMenuId: foodItemUtilities.createSetMenuId(food.setMenuIds)
		};

		food = updateFoodSetMenus(food);

		var objFood = {
			editedFood: editedFood,
			food: food
		};

		$scope.submitted = true;
		$scope.disabledSubmitButton = true;

		if (foodItemUtilities.validateFoodItem(editedFood)) {
			$modalInstance.close(objFood);
		}
		else {
			$scope.disabledSubmitButton = false;
		}
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);
