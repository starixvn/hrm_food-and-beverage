'use strict';
/*global foodMenuManagementModule*/

foodMenuManagementModule.controller('FoodMenuManagementController',
['$scope', '$modal', 'foodMenuFactory', 'notificationFactory', 'commonFactory', 'foodItemFactory',
'ErrorFactory',
function ($scope, $modal, foodMenuFactory, notificationFactory, commonFactory, foodItemFactory,
ErrorFactory) {
	/*
	** Declare variables.
	*/
	$scope.filter = {
		mealId: '',
		employeeTypeId: ''
	};

	$scope.originalPage = 0;

	/*
	** If route change, close dialog.
	*/
	$scope.$on('$routeChangeStart', function () {
		// if modal instance defined, dismiss dialog
		if ($scope.modalInstance) {
			$scope.modalInstance.dismiss('cancel');
		}
	});

	/*
	** Paging.
	*/
	$scope.showEntries = function () {
		commonFactory.tablePagination($scope, $scope.foodItems, $scope.entrySelected);
	};

	$scope.entrySelected = 10;
	commonFactory.numberEntryList($scope);

	/*
	** Get all meal timesheet.
	*/
	foodMenuFactory.getAllMealTimesheet().success(function (meals) {
		$scope.mealsOption = meals;
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

	/*
	** Get all employee type.
	*/
	foodMenuFactory.getAllEmployeeType().success(function (employeeTypes) {
		$scope.employeeTypesOption = employeeTypes;
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

	/*
	** Get all food item.
	*/
	foodItemFactory.getAllFoodItem(null, true).success(function (foodItems) {
		$scope.foodItems = foodItems;
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

	/*
	** Get user permission on food menu.
	*/
	foodMenuFactory.getUserPermissionOnFoodMenu().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

	/*
	** Get all food menu.
	*/
	var getAllFoodMenu = function (mealId, employeeTypeId, isActive) {
		foodMenuFactory.getAllFoodMenu(mealId, employeeTypeId, isActive).success(function (foodMenus) {
			$scope.foodMenus = foodMenus;
			commonFactory.tablePagination($scope, $scope.foodMenus, $scope.entrySelected);
			$scope.currentPage = $scope.originalPage;
		}).error(function () {
			notificationFactory.error("ErrorMessage");
		});
	};

	getAllFoodMenu(null, null, null);

	/*
	** Query to filter data.
	*/
	$scope.query = function (filter) {
		if (filter) {
			getAllFoodMenu(filter.mealId, filter.employeeTypeId, filter.isActive);
		}
	};

	/*
	** Add food menu.
	*/
	$scope.addFoodMenu = function () {
		$scope.originalPage = $scope.currentPage;

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/FoodMenuManagement/Templates/create.html',
			controller: "CreateFoodMenuController",
			resolve: {
				mealsOption: function () {
					return $scope.mealsOption;
				},
				employeeTypesOption: function () {
					return $scope.employeeTypesOption;
				},
				foodItems: function () {
					return $scope.foodItems;
				}
			},
			windowClass: "modal-confirm-center food-menu-modal"
		});

		$scope.modalInstance.result.then(function (foodMenu) {
			foodMenuFactory.insertFoodMenu(foodMenu).success(function (response) {
				$scope.submitted = false;
				$scope.disabledSubmitButton = false;

				if (response.IsSuccessful === true) {
					notificationFactory.success("DataChangeSuccessfully");
				}
				else if (response.Code === 101) {
					notificationFactory.error("DuplicateFoodMenuMessage");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("ErrorMessage");
				}
			});
		})['finally'](function () {
			getAllFoodMenu(null, null, null);
			$scope.modalInstance = '';
		});
	};

	/*
	** Edit food menu.
	*/
	var updateChangedFoodMenu = function (foodMenu, changedFoodMenu) {
		foodMenu.MenuVnmName = changedFoodMenu.MenuVnmName;
		foodMenu.MenuEngName = changedFoodMenu.MenuEngName;
		foodMenu.IsVegetarian = changedFoodMenu.IsVegetarian;
		foodMenu.IsActive = changedFoodMenu.IsActive;
		foodMenu.PricePerMeal = changedFoodMenu.PricePerMeal;
		foodMenu.PricePerDay = changedFoodMenu.PricePerDay;
		foodMenu.FoodItems = changedFoodMenu.FoodItems;
		foodMenu.EmployeeTypes = changedFoodMenu.EmployeeTypes;
		foodMenu.Meals = changedFoodMenu.Meals;
		foodMenu.NumberFoodItems = changedFoodMenu.NumberFoodItems;
	};

	$scope.edit = function (foodMenu) {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/FoodMenuManagement/Templates/edit.html',
			controller: "EditFoodMenuController",
			resolve: {
				foodMenu: function () {
					return angular.copy(foodMenu);
				},
				foodItems: function () {
					return $scope.foodItems;
				},
				mealsOption: function () {
					return $scope.mealsOption;
				},
				employeeTypesOption: function () {
					return $scope.employeeTypesOption;
				}
			},
			windowClass: "modal-confirm-center food-menu-modal"
		});

		$scope.modalInstance.result.then(function (objFood) {
			foodMenuFactory.updateFoodMenu(objFood.editedFoodMenu).success(function (response) {
				if (response.IsSuccessful === true) {
					updateChangedFoodMenu(foodMenu, objFood.foodMenu);
					notificationFactory.success("DataChangeSuccessfully");
				}
				else if (response.Code === 101) {
					notificationFactory.error("DuplicateFoodMenuUpdateMessage");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("DataChangeFail");
				}
			});
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};

	/*
	** Activate or deactivate food menu.
	*/
	$scope.confirmActive = function (foodMenu) {
		var action = !foodMenu.IsActive ? "activate" : "deactivate",
			newFoodMenu = {
				menuId: foodMenu.MenuId,
				IsActive: !foodMenu.IsActive
			};

		foodMenu.IsActive = newFoodMenu.IsActive;

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			resolve: {
				name: function () {
					return foodMenu.MenuVnmName + ' (' + foodMenu.MenuEngName + ')';
				},
				action: function () {
					return action;
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		var keyMessage = '';

		$scope.modalInstance.result.then(function () {
			foodMenuFactory.activateFoodMenu(newFoodMenu).success(function (response) {
				if (response.IsSuccessful === true) {
					keyMessage = action === "activate" ? "ActivationSuccess" : "DeactivationSuccess";
					notificationFactory.success(keyMessage);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					foodMenu.IsActive = !newFoodMenu.IsActive;

					if (response.Code === 101) {
						notificationFactory.error("DuplicateFoodMenuUpdateMessage");
					}

					notificationFactory.error(keyMessage);
				}
			}).error(function () {
				keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
				foodMenu.IsActive = !newFoodMenu.IsActive;
				notificationFactory.error(keyMessage);
			});
		}, function () {
			foodMenu.IsActive = !newFoodMenu.IsActive;
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};

	/*
	** Delete food menu.
	*/
	var updatePaging = function (foodMenu, currentPage) {
		var index = $scope.foodMenus.indexOf(foodMenu);
		$scope.foodMenus.splice(index, 1);

		commonFactory.tablePagination($scope, $scope.foodMenus, $scope.entrySelected);
		var numberPage = $scope.pagedItems.length;

		if (currentPage <= numberPage - 1) {
			$scope.currentPage = currentPage;
		}
		else {
			$scope.currentPage = currentPage - 1;
		}
	};

	$scope.deleteFoodMenu = function (foodMenu) {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			resolve: {
				action: function () {
					return 'delete';
				},
				name: function () {
					return foodMenu.MenuVnmName + ' (' + foodMenu.MenuEngName + ')';
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		$scope.modalInstance.result.then(function () {
			var currentPage = $scope.currentPage;

			foodMenuFactory.deleteFoodMenu(foodMenu.MenuId).success(function (response) {
				if (response.IsSuccessful === true) {
					updatePaging(foodMenu, currentPage);
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("DataChangeFail");
				}
			});
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};
} ]);

foodMenuManagementModule.controller('CreateFoodMenuController',
['$scope', '$modalInstance', 'mealsOption', 'employeeTypesOption', 'foodItems', 'foodMenuUtilities',
'foodMenuFactory', 'notificationFactory', 'ErrorFactory',
function ($scope, $modalInstance, mealsOption, employeeTypesOption, foodItems, foodMenuUtilities,
foodMenuFactory, notificationFactory, ErrorFactory) {
	/*
	** Declare variables.
	*/
	$scope.mealsOption = angular.copy(mealsOption);
	$scope.employeeTypesOption = angular.copy(employeeTypesOption);
	$scope.groupOptions = foodItems;

	$scope.foodMenu = {
		menuVnmName: '',
		menuEngName: '',
		isVegetarian: false,
		isActive: false,
		mealId: '',
		employeeTypeId: '',
		foodId: '',
		pricePerMeal: 0,
		pricePerDay: 0,
		rawFoodIds: ''
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
	$scope.addFoodMenu = function (foodMenu) {
		$scope.disabledSubmitButton = true;

		if (foodMenu) {
			foodMenu.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
			foodMenu.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
		}

		if (foodMenuUtilities.validateFoodMenu(foodMenu) === true) {
			foodMenu.foodId = foodMenuUtilities.createFoodId(foodMenu.rawFoodIds);
			$modalInstance.close(foodMenu);
		}
		else {
			$scope.disabledSubmitButton = false;
		}
	};

	/*
	** Add new food menu and continue.
	*/
	$scope.addAndContinue = function (foodMenu) {
		$scope.disabledSubmitButton = true;

		if (foodMenu) {
			foodMenu.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
			foodMenu.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
		}

		if (foodMenuUtilities.validateFoodMenu(foodMenu) === true) {
			foodMenu.foodId = foodMenuUtilities.createFoodId(foodMenu.rawFoodIds);

			foodMenuFactory.insertFoodMenu(foodMenu).success(function (response) {
				if (response.IsSuccessful === true) {
					$scope.$broadcast("resetFoodMenu");
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("ErrorMessage");
				}
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

foodMenuManagementModule.controller('EditFoodMenuController',
['$scope', '$modalInstance', 'foodMenu', 'foodItems', 'mealsOption', 'employeeTypesOption',
'foodMenuUtilities',
function ($scope, $modalInstance, foodMenu, foodItems, mealsOption, employeeTypesOption,
foodMenuUtilities) {
	/*
	** Declare variables.
	*/
	$scope.foodMenu = foodMenu;
	$scope.groupOptions = foodItems;
	$scope.mealsOption = angular.copy(mealsOption);
	$scope.employeeTypesOption = angular.copy(employeeTypesOption);

	/*
	** Disable or enable food item multiselect.
	*/
	$scope.$watch('foodMenu.IsActive', function (newValue) {
		if (newValue === false) {
			$scope.$broadcast("disableChosenSelect");
		}
		else {
			$scope.$broadcast("enableChosenSelect");
		}
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

	$scope.foodMenu.rawFoodIds = createFoodIds(foodMenu.FoodItems);

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
	** Update employee type.
	*/
	var getEmployeeTypeById = function (employeeTypeId, employeeTypesOption) {
		var employeeType = '',
			length = employeeTypesOption.length;

		for (var i = 0; i < length; i++) {
			var currentEmployeeType = employeeTypesOption[i];

			if (currentEmployeeType.EmployeeTypeId === employeeTypeId) {
				return currentEmployeeType;
			}
		}

		return employeeType;
	};

	var updateEmployeeTypes = function (employeeTypeIds) {
		var updateEmployeeTypeList = [],
			employeeTypeIdList = employeeTypeIds.split(',');

		angular.forEach(employeeTypeIdList, function (employeeTypeId) {
			var addedEmployeeType = getEmployeeTypeById(parseInt(employeeTypeId), $scope.employeeTypesOption);
			updateEmployeeTypeList.push(addedEmployeeType);
		});

		return updateEmployeeTypeList;
	};

	/*
	** Update Food Items.
	*/
	var getFoodItemById = function (foodId, allFoodItems) {
		var foodItem = '',
			numberFoodItem = allFoodItems.length;

		for (var i = 0; i < numberFoodItem; i++) {
			var currentFoodItem = allFoodItems[i];

			if (currentFoodItem.FoodId === foodId) {
				return currentFoodItem;
			}
		}

		return foodItem;
	};

	var updateFoodItems = function (rawFoodIds) {
		var foodItems = [];

		angular.forEach(rawFoodIds, function (foodId) {
			var addedFoodItem = getFoodItemById(foodId, $scope.groupOptions);
			foodItems.push(addedFoodItem);
		});

		return foodItems;
	};

	/*
	** Update food menu.
	*/
	$scope.updateFoodMenu = function (foodMenu) {
		if (foodMenu) {
			foodMenu.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
			foodMenu.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
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
			employeeTypeId: foodMenu.employeeTypeId
		};

		$scope.disabledSubmitButton = true;

		if (foodMenuUtilities.validateFoodMenu(editedFoodMenu) === true) {
			foodMenu.Meals = updateMeals(editedFoodMenu.mealId);
			foodMenu.EmployeeTypes = updateEmployeeTypes(editedFoodMenu.employeeTypeId);
			foodMenu.FoodItems = updateFoodItems(foodMenu.rawFoodIds);
			foodMenu.NumberFoodItems = foodMenu.rawFoodIds.length;
			editedFoodMenu.foodId = foodMenuUtilities.createFoodId(foodMenu.rawFoodIds);

			var objFood = {
				editedFoodMenu: editedFoodMenu,
				foodMenu: foodMenu
			};

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