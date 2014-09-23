'use strict';
/*global foodItemManagementModule*/

foodItemManagementModule.controller('FoodItemManagementController',
['$scope', '$modal', 'foodItemFactory', 'notificationFactory', '$route',
function ($scope, $modal, foodItemFactory, notificationFactory, $route) {

	/*
	** Get all food item.
	*/
	var getAllFoodItem = function (menuGroupId, isActive) {
		foodItemFactory.getAllFoodItem(menuGroupId, isActive).success(function (foodItems) {
			$scope.foodItems = foodItems;
		}).error(function () {
			notificationFactory.error("ErrorMessage");
		});
	};

	getAllFoodItem(null, null);

	$scope.addFoodItem = function () {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/FoodItemManagement/Templates/create.html',
			controller: "CreateFoodItemController",
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function () {
		});
	};

	$scope.edit = function () {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/FoodItemManagement/Templates/edit.html',
			controller: "EditFoodItemController",
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function () {
		});
	};

	$scope.reload = function () {
		$route.reload();
	};
} ]);


foodItemManagementModule.controller('CreateFoodItemController',
['$scope', '$modalInstance',
function ($scope, $modalInstance) {
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
