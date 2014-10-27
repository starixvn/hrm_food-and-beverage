'use strict';
/*global foodItemManagementModule*/

foodItemManagementModule.controller('FoodItemManagementController',
['$scope', '$modal', 'foodItemFactory', 'notificationFactory', 'setMenuFactory', 'commonFactory',
'ErrorFactory',
function ($scope, $modal, foodItemFactory, notificationFactory, setMenuFactory, commonFactory,
ErrorFactory) {
	/*
	** Declare variable.
	*/
	$scope.filter = {
		setMenuId: '',
		isActive: ''
	};

	$scope.originalPage = 0;

	/*
	** Paging.
	*/
	$scope.showEntries = function () {
		commonFactory.tablePagination($scope, $scope.foodItems, $scope.entrySelected);
	};

	$scope.entrySelected = 10;
	commonFactory.numberEntryList($scope);

	/*
	** Get all set menu to filter.
	*/
	setMenuFactory.getSetMenu(null, null).success(function (setMenus) {
		$scope.setMenuItems = setMenus;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all food item.
	*/
	var getAllFoodItem = function (setMenuId, isActive) {
		foodItemFactory.getAllFoodItem(setMenuId, isActive).success(function (foodItems) {
			$scope.foodItems = foodItems;
			commonFactory.tablePagination($scope, foodItems, $scope.entrySelected);
			$scope.currentPage = $scope.originalPage;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	getAllFoodItem(null, null);

	/*
	** Query data when filter.
	*/
	function getSelectedSetMenu(filter) {
		filter.setMenuId = '';

		angular.forEach($scope.setMenuItems, function (value) {
			if (value.ticked) {
				filter.setMenuId = filter.setMenuId !== '' ? filter.setMenuId + ',' + value.SetMenuId : value.SetMenuId;
			}
		});
	}

	$scope.query = function (filter) {
		getSelectedSetMenu(filter);
		getAllFoodItem(filter.setMenuId, filter.isActive);
	};

	/*
	** Get user permission on Food Item Management.
	*/
	foodItemFactory.getPermissonFoodItem().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
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
				setMenuItems: function () {
					return $scope.setMenuItems;
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		$scope.modalInstance.result.then(function (food) {

			foodItemFactory.insertFoodItem(food).success(function (response) {
				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
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
		food.SetMenus = changedFood.SetMenus;
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
				setMenuItems: function () {
					return $scope.setMenuItems;
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		$scope.modalInstance.result.then(function (objFood) {
			foodItemFactory.updateFoodItem(objFood.editedFood).success(function (response) {
				if (response.IsSuccessful) {
					updateChangedFoodItem(food, objFood.food);
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
			foodItemFactory.updateStatusFoodItem(newFood).success(function (response) {
				if (response.IsSuccessful) {
					keyMessage = action === "activate" ? "ActivationSuccess" : "DeactivationSuccess";
					notificationFactory.success(keyMessage);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					food.IsActive = !newFood.IsActive;
					notificationFactory.error(keyMessage);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					food.IsActive = !newFood.IsActive;
					notificationFactory.error(keyMessage);
				}
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
				if (response.IsSuccessful) {
					updatePaging(food, currentPage);
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