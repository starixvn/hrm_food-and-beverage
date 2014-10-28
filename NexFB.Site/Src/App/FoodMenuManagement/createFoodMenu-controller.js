'use strict';
/*global foodMenuManagementModule*/

foodMenuManagementModule.controller('CreateFoodMenuController',
['$scope', '$modalInstance', 'mealsOption', 'employeeTypesOption', 'setMenuItems', 'foodMenuUtilities',
'foodMenuFactory', 'notificationFactory', 'ErrorFactory',
function ($scope, $modalInstance, mealsOption, employeeTypesOption, setMenuItems, foodMenuUtilities,
foodMenuFactory, notificationFactory, ErrorFactory) {
	/*
	** Declare variables.
	*/
	$scope.mealsOption = angular.copy(mealsOption);
	$scope.employeeTypesOption = angular.copy(employeeTypesOption);

	var getSetMenus = function (setMenuItems) {
		var setMenus = [];

		angular.forEach(setMenuItems, function (setMenu) {
			if (setMenu.IsActive) {
				setMenus.push(setMenu);
			}
		});

		return setMenus;
	};

	$scope.setMenuItems = getSetMenus(setMenuItems);

	$scope.foodMenu = {
		menuVnmName: '',
		menuEngName: '',
		isVegetarian: false,
		isActive: true,
		mealId: '',
		employeeTypeId: '',
		setMenuId: '',
		pricePerMeal: 0,
		pricePerDay: 0,
		rawSetMenuIds: []
	};

	$scope.mealSelectSettings = {
		displayProp: 'MealVnmName',
		idProp: 'MealId'
	};

	/*
	** Set form scope to current scope.
	*/
	$scope.setFormScope = function (formScope) {
		$scope.formScope = formScope;
	};

	/*
	** Add new food menu.
	*/
	$scope.addFoodMenu = function (foodMenu, isContinue) {
		$scope.disabledSubmitButton = true;

		if (foodMenu) {
			foodMenu.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
			foodMenu.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);

			if (!foodMenu.pricePerMeal) {
				foodMenu.pricePerMeal = 0;
			}
			if (!foodMenu.pricePerDay) {
				foodMenu.pricePerDay = 0;
			}
		}

		if (foodMenuUtilities.validateFoodMenu(foodMenu)) {
			foodMenu.setMenuId = foodMenuUtilities.createSetMenuId(foodMenu.rawSetMenuIds);

			foodMenuFactory.insertFoodMenu(foodMenu).success(function (response) {
				$scope.submitted = false;
				$scope.disabledSubmitButton = false;

				if (response.IsSuccessful) {
					if (isContinue) {
						$scope.$broadcast("resetFoodMenu");
					}
					else {
						$modalInstance.close();
					}
					notificationFactory.success("DataChangeSuccessfully");

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