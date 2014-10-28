'use strict';
/*global foodMenuManagementModule*/

foodMenuManagementModule.controller('EditFoodMenuController',
['$scope', '$modalInstance', 'foodMenu', 'setMenuItems', 'mealsOption', 'employeeTypesOption',
'foodMenuUtilities', 'foodMenuFactory', 'notificationFactory', 'ErrorFactory', 'commonFactory',
function ($scope, $modalInstance, foodMenu, setMenuItems, mealsOption, employeeTypesOption,
foodMenuUtilities, foodMenuFactory, notificationFactory, ErrorFactory, commonFactory) {
	/*
	** Declare variables.
	*/
	$scope.foodMenu = foodMenu;
	$scope.mealsOption = angular.copy(mealsOption);
	$scope.employeeTypesOption = angular.copy(employeeTypesOption);

	var getSetMenus = function (setMenuItems) {
		var setMenus = [];

		angular.forEach(setMenuItems, function (setMenu) {
			if (setMenu.IsActive) {
				setMenus.push(setMenu);
			}
			else {
				angular.forEach(foodMenu.SetMenus, function (foodSetMenu) {
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
	** Create list food item Id.
	*/
	var createSetMenuIds = function (setMenus) {
		var setMenuIds = [];

		angular.forEach(setMenus, function (setMenu) {
			setMenuIds.push(setMenu.SetMenuId);
		});

		return setMenuIds;
	};

	$scope.foodMenu.rawSetMenuIds = createSetMenuIds(foodMenu.SetMenus);

	/*
	** Check meal that belong to food menu.
	*/
	var checkMealBelongToFoodMenu = function (mealId, meals) {
		var exist = false;

		angular.forEach(meals, function (meal) {
			if (meal.MealId === mealId) {
				exist = true;
			}
		});

		return exist;
	};

	var checkMealOptions = function () {
		angular.forEach($scope.mealsOption, function (meal) {
			meal.ticked = checkMealBelongToFoodMenu(meal.MealId, $scope.foodMenu.Meals);
		});
	};

	checkMealOptions();
	/*
	** Check employee type that belong to food menu.
	*/
	var checkEmployeeTypeBelongToFoodMenu = function (employeeTypeId, employeeTypes) {
		var exist = false;

		angular.forEach(employeeTypes, function (employeeType) {
			if (employeeType.EmployeeTypeId === employeeTypeId) {
				exist = true;
			}
		});

		return exist;
	};

	var checkEmployeeTypeOptions = function () {
		angular.forEach($scope.employeeTypesOption, function (employeeType) {
			employeeType.ticked = checkEmployeeTypeBelongToFoodMenu(employeeType.EmployeeTypeId, $scope.foodMenu.EmployeeTypes);
		});
	};

	checkEmployeeTypeOptions();

	/*
	** Update meals.
	*/
	var getMealById = function (mealId, mealsOption) {
		var meal = '',
			mealsOptionLength = mealsOption.length;

		for (var i = 0; i < mealsOptionLength; i++) {
			var currentMeal = mealsOption[i];

			if (currentMeal.MealId === mealId) {
				return currentMeal;
			}
		}

		return meal;
	};

	var updateMeals = function (mealIds) {
		var updateMeals = [],
			mealIdList = mealIds.split(',');

		angular.forEach(mealIdList, function (mealId) {
			var addedMeal = getMealById(parseInt(mealId), $scope.mealsOption);
			updateMeals.push(addedMeal);
		});

		return updateMeals;
	};

	/*
	** Update Set menu.
	*/
	var getSetMenuById = function (setMenuId, setMenuItems) {
		var setMenuItem = '',
			numberSetMenu = setMenuItems.length;

		for (var i = 0; i < numberSetMenu; i++) {
			var currentSetMenu = setMenuItems[i];

			if (currentSetMenu.SetMenuId === setMenuId) {
				return currentSetMenu;
			}
		}

		return setMenuItem;
	};

	var updateSetMenuItems = function (rawSetMenuIds) {
		var setMenuItems = [];

		angular.forEach(rawSetMenuIds, function (setMenuId) {
			var addedSetMenuItem = getSetMenuById(setMenuId, $scope.setMenuItems);
			setMenuItems.push(addedSetMenuItem);
		});

		return setMenuItems;
	};

	/*
	** Update food menu.
	*/
	$scope.updateFoodMenu = function (foodMenu) {
		if (foodMenu) {
			foodMenu.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
			foodMenu.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);

			if (!foodMenu.PricePerMeal) {
				foodMenu.PricePerMeal = 0;
			}
			if (!foodMenu.PricePerDay) {
				foodMenu.PricePerDay = 0;
			}
		}

		var editedFoodMenu = {
			menuId: foodMenu.MenuId,
			menuVnmName: foodMenu.MenuVnmName,
			menuEngName: foodMenu.MenuEngName,
			isVegetarian: foodMenu.IsVegetarian,
			pricePerMeal: foodMenu.PricePerMeal,
			pricePerDay: foodMenu.PricePerDay,
			isActive: foodMenu.IsActive,
			mealId: foodMenu.mealId,
			employeeTypeId: foodMenu.employeeTypeId,
			setMenuId: foodMenuUtilities.createSetMenuId(foodMenu.rawSetMenuIds)
		};

		$scope.disabledSubmitButton = true;

		if (foodMenuUtilities.validateFoodMenu(editedFoodMenu)) {

			foodMenuFactory.updateFoodMenu(editedFoodMenu).success(function (response) {
				if (response.IsSuccessful) {
					foodMenu.Meals = updateMeals(editedFoodMenu.mealId);
					foodMenu.EmployeeTypes = commonFactory.updateEmployeeTypes(editedFoodMenu.employeeTypeId, $scope.employeeTypesOption);
					foodMenu.SetMenus = updateSetMenuItems(foodMenu.rawSetMenuIds);

					notificationFactory.success("DataChangeSuccessfully");
					$modalInstance.close(foodMenu);
				}
				else if (response.Code === 101) {
					notificationFactory.error("DuplicateFoodMenuMessage");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
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