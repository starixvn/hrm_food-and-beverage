'use strict';
/*global foodItemManagementModule*/

foodItemManagementModule.controller('FoodItemManagementController',
['$scope', '$modal', 'foodItemFactory', 'notificationFactory', 'foodMenuFactory', 'commonFactory',
'ErrorFactory', 'foodItemUtilities',
function ($scope, $modal, foodItemFactory, notificationFactory, foodMenuFactory, commonFactory,
ErrorFactory, foodItemUtilities) {
	/*
	** Declare variable.
	*/
	$scope.filter = {
		menuGroupId: '',
		isActive: ''
	};

	$scope.originalPage = 0;
	/*
	** Paging.
	*/
	$scope.showEntries = function () {
		commonFactory.tablePagination($scope, $scope.foodItems, $scope.entrySelected);
	};

	$scope.entrySelected = 15;
	commonFactory.numberEntryList($scope);

	/*
	** Get all food menu to filter.
	*/
	foodMenuFactory.getAllFoodMenuToFilter().success(function (foodMenus) {
		$scope.menuOptions = foodMenus;
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

	/*
	** Get all food item.
	*/
	var getAllFoodItem = function (menuGroupId, isActive) {
		foodItemFactory.getAllFoodItem(menuGroupId, isActive).success(function (foodItems) {
			$scope.foodItems = foodItems;
			commonFactory.tablePagination($scope, foodItems, $scope.entrySelected);
			$scope.currentPage = $scope.originalPage;
		}).error(function () {
			notificationFactory.error("ErrorMessage");
		});
	};

	getAllFoodItem(null, null);

	/*
	** Query data when filter.
	*/
	function getSelectedMenuGroup(filter) {
		filter.menuGroupId = '';

		angular.forEach($scope.menuOptions, function (value) {
			if (value.ticked === true) {
				var menuGroupId = value.MenuId;
				filter.menuGroupId = filter.menuGroupId !== '' ? filter.menuGroupId + ',' + menuGroupId : menuGroupId;
			}
		});
	}

	$scope.query = function (filter) {
		getSelectedMenuGroup(filter);
		getAllFoodItem(filter.menuGroupId, filter.isActive);
	};

	/*
	** Get user permission on Food Item Management.
	*/
	foodItemFactory.getPermissonFoodItem().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

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
	** Add food item.
	*/
	$scope.addFoodItem = function () {
		$scope.originalPage = $scope.currentPage;

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/FoodItemManagement/Templates/create.html',
			controller: "CreateFoodItemController",
			resolve: {
				menuOptions: function () {
					return $scope.menuOptions;
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		$scope.modalInstance.result.then(function (food) {

			foodItemFactory.insertFoodItem(food).success(function (response) {
				if (response.IsSuccessful === true) {
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
			});
		})['finally'](function () {
			getAllFoodItem(null, null);
			$scope.modalInstance = '';
		});
	};

	/*
	** Edit food item.
	*/
	var updateChangedFoodItem = function (food, changedFood) {
		food.FoodVnmName = changedFood.FoodVnmName;
		food.FoodEngName = changedFood.FoodEngName;
		food.FoodMenus = changedFood.FoodMenus;
		food.IsActive = changedFood.IsActive;
	};

	$scope.edit = function (food) {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/FoodItemManagement/Templates/edit.html',
			controller: "EditFoodItemController",
			resolve: {
				foodItem: function () {
					return angular.copy(food);
				},
				menuOptions: function () {
					return $scope.menuOptions;
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		$scope.modalInstance.result.then(function (objFood) {
			foodItemFactory.updateFoodItem(objFood.editedFood).success(function (response) {
				if (response.IsSuccessful === true) {
					updateChangedFoodItem(food, objFood.food);
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

	/*
	** Activate or deactivate food item.
	*/
	$scope.confirmActive = function (food) {
		var action = !food.IsActive ? "activate" : "deactivate",
			newFood = angular.copy(food);

		newFood.IsActive = !food.IsActive;
		food.IsActive = newFood.IsActive;

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			resolve: {
				name: function () {
					return food.FoodVnmName + ' (' + food.FoodEngName + ')';
				},
				action: function () {
					return action;
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		var keyMessage = '';

		$scope.modalInstance.result.then(function () {
			newFood.menuGroupId = foodItemUtilities.createMenuGroupIdFromFoodMenu(newFood.FoodMenus);

			foodItemFactory.updateFoodItem(newFood).success(function (response) {
				if (response.IsSuccessful === true) {
					keyMessage = action === "activate" ? "ActivationSuccess" : "DeactivationSuccess";
					notificationFactory.success(keyMessage);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					food.IsActive = !newFood.IsActive;
					notificationFactory.error(keyMessage);
				}
			}).error(function () {
				keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
				food.IsActive = !newFood.IsActive;
				notificationFactory.error(keyMessage);
			});
		}, function () {
			food.IsActive = !newFood.IsActive;
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};

	/*
	** Delete food item.
	*/
	var updatePaging = function (food, currentPage) {
		var index = $scope.foodItems.indexOf(food);
		$scope.foodItems.splice(index, 1);

		commonFactory.tablePagination($scope, $scope.foodItems, $scope.entrySelected);
		var numberPage = $scope.pagedItems.length;

		if (currentPage <= numberPage - 1) {
			$scope.currentPage = currentPage;
		}
		else {
			$scope.currentPage = currentPage - 1;
		}
	};

	$scope.deleteFoodItem = function (food) {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			resolve: {
				action: function () {
					return 'delete';
				},
				name: function () {
					return food.FoodVnmName + ' (' + food.FoodEngName + ')';
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		$scope.modalInstance.result.then(function () {
			var currentPage = $scope.currentPage;

			foodItemFactory.deleteFoodItem(food.FoodId).success(function (response) {
				if (response.IsSuccessful === true) {
					updatePaging(food, currentPage);
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


foodItemManagementModule.controller('CreateFoodItemController',
['$scope', '$modalInstance', 'menuOptions', 'notificationFactory', 'ErrorFactory', 'foodItemFactory',
'foodItemUtilities',
function ($scope, $modalInstance, menuOptions, notificationFactory, ErrorFactory, foodItemFactory,
foodItemUtilities) {
	/*
	** Declare variables.
	*/
	$scope.groupOptions = menuOptions;

	$scope.food = {
		foodEngName: '',
		foodVnmName: '',
		isActive: false,
		groupIds: '',
		menuGroupId: ''
	};

	/*
	** Set scope of form to current scope.
	*/
	$scope.setFormScope = function (formScope) {
		$scope.formScope = formScope;
	};

	/*
	** Create food item.
	*/
	foodItemUtilities.showMessageValidateFoodItem($scope);

	$scope.create = function (food) {
		$scope.submitted = true;
		$scope.disabledSubmitButton = true;

		if (foodItemUtilities.validateFoodItem(food) === true) {
			food.menuGroupId = foodItemUtilities.createMenuGroupId(food.groupIds);
			$modalInstance.close(food);
		}
		else {
			$scope.disabledSubmitButton = false;
		}
	};

	/*
	** Create and continue.
	*/
	$scope.createAndContinue = function (food) {
		$scope.submitted = true;
		$scope.disabledSubmitButton = true;

		if (foodItemUtilities.validateFoodItem(food) === true) {
			food.menuGroupId = foodItemUtilities.createMenuGroupId(food.groupIds);

			foodItemFactory.insertFoodItem(food).success(function (response) {
				$scope.submitted = false;
				$scope.disabledSubmitButton = false;

				if (response.IsSuccessful === true) {
					$scope.$broadcast("resetFoodItem");
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

foodItemManagementModule.controller('EditFoodItemController',
['$scope', '$modalInstance', 'menuOptions', 'foodItem', 'foodItemUtilities',
function ($scope, $modalInstance, menuOptions, foodItem, foodItemUtilities) {
	/*
	** Declare variables.
	*/
	$scope.groupOptions = menuOptions;
	$scope.food = foodItem;

	/*
	** Set scope of form to current scope.
	*/
	$scope.setFormScope = function (formScope) {
		$scope.formScope = formScope;
	};

	/*
	** Disable or enable food menu multiselect.
	*/
	$scope.$watch('food.IsActive', function (newValue) {
		if (newValue === false) {
			$scope.$broadcast("disableChosenSelect");
		}
		else {
			$scope.$broadcast("enableChosenSelect");
		}
	});

	/*
	** Create list group Id.
	*/
	var createGroupIds = function (foodMenus) {
		var groupIds = [];

		angular.forEach(foodMenus, function (menu) {
			groupIds.push(menu.MenuId);
		});

		return groupIds;
	};

	$scope.food.groupIds = createGroupIds(foodItem.FoodMenus);

	/*
	** Update food menus.
	*/
	var findFoodByMenu = function (menuId, groupMenus) {
		var food = '';

		for(var i=0 ; i < groupMenus.length; i++) {
			var currentMenu = groupMenus[i];
			
			if (currentMenu.MenuId === menuId) {
				food = currentMenu;
			}
		}

		return food;
	};

	var updateFoodMenus = function (food) {
		food.FoodMenus = [];

		angular.forEach(food.groupIds, function (menuId) {
			var addedFood = findFoodByMenu(menuId, $scope.groupOptions);
			food.FoodMenus.push(addedFood);
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
			menuGroupId: foodItemUtilities.createMenuGroupId(food.groupIds)
		};

		food = updateFoodMenus(food);

		var objFood = {
			editedFood: editedFood,
			food: food
		};

		$scope.submitted = true;
		$scope.disabledSubmitButton = true;

		if (foodItemUtilities.validateFoodItem(editedFood) === true) {
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
