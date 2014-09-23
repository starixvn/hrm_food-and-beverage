'use strict';
/*global foodItemManagementModule*/

foodItemManagementModule.controller('FoodItemManagementController',
['$scope', '$modal', 'foodItemFactory', 'notificationFactory', '$route', 'foodMenuFactory', 'commonFactory',
'ErrorFactory',
function ($scope, $modal, foodItemFactory, notificationFactory, $route, foodMenuFactory, commonFactory,
ErrorFactory) {
	/*
	** Declare variable.
	*/
	$scope.filter = {
		menuGroupId: '',
		isActive: ''
	};

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
	** Add food item.
	*/
	$scope.addFoodItem = function () {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/FoodItemManagement/Templates/create.html',
			controller: "CreateFoodItemController",
			resolve: {
				menuOptions: function () {
					return $scope.menuOptions;
				}
			},
			windowClass: "modal-confirm-center add-food-item"
		});

		modalAssignInstance.result.then(function (food) {
			foodItemFactory.insertFoodItem(food).success(function () {

			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("ErrorMessage");
				}
			});
		});
	};

	/*
	** Edit food item.
	*/
	$scope.edit = function () {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/FoodItemManagement/Templates/edit.html',
			controller: "EditFoodItemController",
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function () {
		});
	};

	/*
	** Activate or deactivate food item.
	*/
	$scope.confirmActive = function (food) {
	};

	$scope.reload = function () {
		$route.reload();
	};
} ]);


foodItemManagementModule.controller('CreateFoodItemController',
['$scope', '$modalInstance', 'menuOptions',
function ($scope, $modalInstance, menuOptions) {
	$scope.groupOptions = menuOptions;

	$scope.food = {
		foodEngName: '',
		foodVnmName: '',
		isActive: '',
		menuGroupId: ''
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);

foodItemManagementModule.controller('EditFoodItemController',
['$scope', '$modalInstance',
function ($scope, $modalInstance) {
	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);
