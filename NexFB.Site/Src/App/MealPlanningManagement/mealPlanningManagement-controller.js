'use strict';
/*global mealPlanningManagementModule*/

mealPlanningManagementModule.controller('MealPlanningManagementController',
['$scope', '$modal', 'mealPlanningFactory', 'ErrorFactory', 'notificationFactory', 'commonFactory',
function ($scope, $modal, mealPlanningFactory, ErrorFactory, notificationFactory, commonFactory) {
	/*
	** Declare variables.
	*/
	$scope.originalPage = 0;
	var toDay = new Date();

	/*
	** Paging.
	*/
	$scope.showEntries = function () {
		commonFactory.tablePagination($scope, $scope.foodItems, $scope.entrySelected);
	};

	$scope.entrySelected = 10;
	commonFactory.numberEntryList($scope);

	/*
	** Get all meal planning.
	*/
	var getAllMealPlanning = function (fromDate, toDate, mealId, employeeTypeId) {
		mealPlanningFactory.getAllMealPlanning(fromDate, toDate, mealId, employeeTypeId)
		.success(function (mealPlannings) {
			$scope.mealPlannings = mealPlannings;
			commonFactory.tablePagination($scope, $scope.mealPlannings, $scope.entrySelected);
			$scope.currentPage = $scope.originalPage;
		}).error(function (response, status) {
			if (status === 405) {
				ErrorFactory.errorHandle(status);
			}
			else {
				notificationFactory.error("ErrorMessage");
			}
		});
	};

	getAllMealPlanning(toDay.toDateString(), toDay.toDateString(), null, null);

	/*
	** Add meal planning.
	*/
	$scope.addMealPlanning = function () {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/MealPlanningManagement/Templates/create.html',
			controller: "CreateMealPlanningController",
			windowClass: "modal-confirm-center"
		});

		modalAssignInstance.result.then(function () {
		});
	};

	/*
	** Edit meal planning.
	*/
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