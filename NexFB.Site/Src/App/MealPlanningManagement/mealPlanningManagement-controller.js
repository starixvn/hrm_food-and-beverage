'use strict';
/*global mealPlanningManagementModule*/

mealPlanningManagementModule.controller('MealPlanningManagementController',
['$scope', '$modal', 'mealPlanningFactory', 'ErrorFactory', 'notificationFactory', 'commonFactory',
'foodMenuFactory', 'foodMenuUtilities',
function ($scope, $modal, mealPlanningFactory, ErrorFactory, notificationFactory, commonFactory,
foodMenuFactory, foodMenuUtilities) {
	/*
	** Declare variables.
	*/
	$scope.originalPage = 0;
	var toDay = new Date();
	var newDate = new Date(toDay.setYear(toDay.getFullYear() + 1));

	var startDate = new Date(),
		endDate = new Date();

	$scope.filter = {
		startDate: new Date(startDate.setYear(startDate.getFullYear() + 1)),
		endDate: new Date(endDate.setYear(endDate.getFullYear() + 1))
	};

	$scope.calendar = {
		startDateOpened: false,
		endDateOpened: false
	};

	/*
	** Paging.
	*/
	$scope.showEntries = function () {
		commonFactory.tablePagination($scope, $scope.mealPlannings, $scope.entrySelected);
	};

	$scope.entrySelected = 10;
	commonFactory.numberEntryList($scope);

	/*
	** Open calendar.
	*/
	$scope.openPopupStartDate = function ($event) {
		commonFactory.openPopupStartDate($event, $scope);
	};

	$scope.openPopupEndDate = function ($event) {
		commonFactory.openPopupEndDate($event, $scope);
	};

	/*
	** Get all food menu to filter.
	*/
	foodMenuFactory.getAllFoodMenuToFilter().success(function (foodMenus) {
		$scope.menuOptions = foodMenus;
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

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

	getAllMealPlanning(newDate.toDateString(), newDate.toDateString(), null, null);

	/*
	** Query data.
	*/
	$scope.query = function (filter) {
		if (filter) {
			var mealId = foodMenuUtilities.getMealId($scope.mealsOption),
				employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption),
				fromDate = filter.startDate.toDateString(),
				toDate = filter.endDate.toDateString();

			getAllMealPlanning(fromDate, toDate, mealId, employeeTypeId);
		}
	};

	/*
	** Add meal planning.
	*/
	$scope.addMealPlanning = function (mealPlanning) {
		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/MealPlanningManagement/Templates/create.html',
			controller: "CreateMealPlanningController",
			resolve: {
				menuId: function () {
					return mealPlanning.FoodMenu.MenuId;
				},
				onDate: function () {
					return mealPlanning.Date;
				},
				isActive: function () {
					return mealPlanning.IsActive;
				},
				menuOptions: function () {
					return $scope.menuOptions;
				}
			},
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
['$scope', '$modalInstance', 'menuId', 'onDate', 'mealPlanningFactory', 'ErrorFactory', 'notificationFactory',
'menuOptions', 'commonFactory', 'isActive',
function ($scope, $modalInstance, menuId, onDate, mealPlanningFactory, ErrorFactory, notificationFactory,
menuOptions, commonFactory, isActive) {

	$scope.menuOptions = menuOptions;
	$scope.onDate = onDate;
	$scope.menuId = menuId;
	$scope.isActive = isActive;

	$scope.calendar = {
		startDateOpened: false
	};

	/*
	** Open calendar.
	*/
	$scope.openPopupStartDate = function ($event) {
		commonFactory.openPopupStartDate($event, $scope);
	};

	/*
	** Get all food items of current menu.
	*/
	var getBasicFoodItems = function (foodId, menuId, isActive) {
		mealPlanningFactory.getBasicFoodItem(foodId, menuId, isActive).success(function (foodItems) {
			$scope.groupOptions = foodItems;
		}).error(function (response, status) {
			if (status === 405) {
				ErrorFactory.errorHandle(status);
			}
			else {
				notificationFactory.error("ErrorMessage");
			}
		});
	};

	getBasicFoodItems(null, menuId, true);

	$scope.getFoodItems = function (menuId) {
		getBasicFoodItems(null, menuId, true);
	};

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