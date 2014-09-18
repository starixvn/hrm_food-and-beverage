'use strict';
/*global foodMenuManagementModule*/

foodMenuManagementModule.controller('FoodMenuManagementController',
['$scope', '$modal',
function ($scope, $modal) {

	$scope.addFoodMenu = function () {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/FoodMenuManagement/Templates/create.html',
			controller: "CreateFoodMenuController",
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function () {
		});
	};

	$scope.edit = function () {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/FoodMenuManagement/Templates/edit.html',
			controller: "EditFoodMenuController",
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function () {
		});
	};
} ]);

foodMenuManagementModule.controller('CreateFoodMenuController',
['$scope', '$modalInstance',
function ($scope, $modalInstance) {
	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);

foodMenuManagementModule.controller('EditFoodMenuController',
['$scope', '$modalInstance',
function ($scope, $modalInstance) {
	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);