'use strict';
/*global foodMenuManagementModule*/

foodMenuManagementModule.controller('FoodMenuManagementController',
['$scope', '$modal', 'foodMenuFactory', 'notificationFactory', 'commonFactory', 'setMenuFactory',
'ErrorFactory',
function ($scope, $modal, foodMenuFactory, notificationFactory, commonFactory, setMenuFactory,
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
		commonFactory.tablePagination($scope, $scope.foodMenus, $scope.entrySelected);
	};

	$scope.entrySelected = 10;
	commonFactory.numberEntryList($scope);

	/*
	** Get all meal timesheet.
	*/
	foodMenuFactory.getAllMealTimesheet().success(function (meals) {
		$scope.mealsOption = meals;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all employee type.
	*/
	foodMenuFactory.getAllEmployeeType(null, null).success(function (employeeTypes) {
		$scope.employeeTypesOption = employeeTypes;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all set menu item.
	*/
	setMenuFactory.getSetMenu(null, null).success(function (setMenus) {
		$scope.setMenuItems = setMenus;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get user permission on food menu.
	*/
	foodMenuFactory.getUserPermissionOnFoodMenu().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all food menu.
	*/
	var getAllFoodMenu = function (mealId, employeeTypeId, isActive) {
		foodMenuFactory.getAllFoodMenu(mealId, employeeTypeId, isActive).success(function (foodMenus) {
			$scope.foodMenus = foodMenus;
			commonFactory.tablePagination($scope, $scope.foodMenus, $scope.entrySelected);
			$scope.currentPage = $scope.originalPage;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
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
			backdrop: 'static',
			resolve: {
				mealsOption: function () {
					return $scope.mealsOption;
				},
				employeeTypesOption: function () {
					return $scope.employeeTypesOption;
				},
				setMenuItems: function () {
					return $scope.setMenuItems;
				}
			},
			windowClass: "modal-confirm-center food-menu-modal"
		});

		$scope.modalInstance.result.then(function () {
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
		foodMenu.PricePerMeal = isNaN(changedFoodMenu.PricePerMeal) ?
									parseInt(changedFoodMenu.PricePerMeal.replace(/\,/g, ''))
									: changedFoodMenu.PricePerMeal;
		foodMenu.PricePerDay = isNaN(changedFoodMenu.PricePerDay) ?
									parseInt(changedFoodMenu.PricePerDay.replace(/\,/g, ''))
									: changedFoodMenu.PricePerDay;
		foodMenu.SetMenus = changedFoodMenu.SetMenus;
		foodMenu.EmployeeTypes = changedFoodMenu.EmployeeTypes;
		foodMenu.Meals = changedFoodMenu.Meals;
	};

	$scope.edit = function (foodMenu) {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/FoodMenuManagement/Templates/edit.html',
			controller: "EditFoodMenuController",
			backdrop: 'static',
			resolve: {
				foodMenu: function () {
					return angular.copy(foodMenu);
				},
				setMenuItems: function () {
					return $scope.setMenuItems;
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

		$scope.modalInstance.result.then(function (changedFoodMenu) {
			updateChangedFoodMenu(foodMenu, changedFoodMenu);
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
			backdrop: 'static',
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
				if (response.IsSuccessful) {
					keyMessage = action === "activate" ? "ActivationSuccess" : "DeactivationSuccess";
					notificationFactory.success(keyMessage);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					foodMenu.IsActive = !newFoodMenu.IsActive;

					if (response.Code === 101) {
						notificationFactory.error("DuplicateFoodMenuMessage");
					}
					else {
						notificationFactory.error(keyMessage);
					}
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					foodMenu.IsActive = !newFoodMenu.IsActive;
					notificationFactory.error(keyMessage);
				}
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
			backdrop: 'static',
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
				if (response.IsSuccessful) {
					updatePaging(foodMenu, currentPage);
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};
} ]);