'use strict';
/*global foodMenuManagementModule*/

foodMenuManagementModule.controller('FoodMenuManagementController',
['$scope', '$modal', 'foodMenuFactory', 'notificationFactory', 'commonFactory',
function ($scope, $modal, foodMenuFactory, notificationFactory, commonFactory) {
	/*
	** Declare variables.
	*/
	$scope.filter = {
		mealId: '',
		employeeTypeId: ''
	};

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
	** Get all food menu.
	*/
	var getAllFoodMenu = function (mealId, employeeTypeId) {
		foodMenuFactory.getAllFoodMenu(mealId, employeeTypeId).success(function (foodMenus) {
			$scope.foodMenus = foodMenus;
			commonFactory.tablePagination($scope, $scope.foodMenus, $scope.entrySelected);
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
			getAllFoodMenu(filter.mealId, filter.employeeTypeId);
		}
	};

	/*
	** Add food menu.
	*/
	$scope.addFoodMenu = function () {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/FoodMenuManagement/Templates/create.html',
			controller: "CreateFoodMenuController",
			resolve: {
				mealsOption: function () {
					return $scope.mealsOption;
				},
				employeeTypesOption: function () {
					return $scope.employeeTypesOption;
				}
			},
			windowClass: "modal-confirm-center"
		});

		$scope.modalInstance.result.then(function () {
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};

	/*
	** Edit food menu.
	*/
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
['$scope', '$modalInstance', 'mealsOption', 'employeeTypesOption',
function ($scope, $modalInstance, mealsOption, employeeTypesOption) {

	$scope.mealsOption = mealsOption;
	$scope.employeeTypesOption = employeeTypesOption;

	$scope.mealSelectSettings = {
		displayProp: 'MealVnmName',
		idProp: 'MealId'
	};

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