'use strict';
/*global foodItemManagementModule*/

foodItemManagementModule.controller('FoodItemManagementController',
['$scope', '$modal',
function ($scope, $modal) {
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
