'use strict';
/*global mealPlanningManagementModule*/

mealPlanningManagementModule.controller('MealPlanningManagementController',
['$scope', '$modal',
function ($scope, $modal) {
	$scope.addMealPlanning = function () {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/MealPlanningManagement/Templates/create.html',
			controller: "CreateMealPlanningController",
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function () {
		});
	};

	$scope.edit = function () {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/MealPlanningManagement/Templates/edit.html',
			controller: "EditMealPlanningController",
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function () {
		});
	};
} ]);


mealPlanningManagementModule.controller('CreateMealPlanningController',
['$scope', '$modalInstance',
function ($scope, $modalInstance) {
	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);

mealPlanningManagementModule.controller('EditMealPlanningController',
['$scope', '$modalInstance',
function ($scope, $modalInstance) {
	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);