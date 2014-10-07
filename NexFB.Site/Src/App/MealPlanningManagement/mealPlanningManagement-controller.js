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
	$scope.addMealPlanning = function (mealPlanning, currentMealPlanning) {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/MealPlanningManagement/Templates/create.html',
			controller: "CreateMealPlanningController",
			resolve: {
				mealPlanning: function () {
					return mealPlanning;
				},
				menuOptions: function () {
					return $scope.menuOptions;
				},
				currentMealPlanning: function () {
					return currentMealPlanning;
				}
			},
			windowClass: "modal-confirm-center"
		});

		$scope.modalInstance.result.then(function (newMealPlanning) {
			mealPlanningFactory.insertMealPlanning(newMealPlanning)
			.success(function (response) {
				if (response.IsSuccessful === true) {
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("ErrorMessage");
				}
			})['finally'](function () {
				getAllMealPlanning($scope.filter.startDate.toDateString(), $scope.filter.endDate.toDateString(),
					foodMenuUtilities.getMealId($scope.mealsOption), foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption));
				$scope.modalInstance = '';
			});
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
['$scope', '$modalInstance', 'mealPlanning', 'mealPlanningFactory', 'ErrorFactory', 'notificationFactory',
'menuOptions', 'commonFactory', 'mealPlanningUtilities', 'currentMealPlanning',
function ($scope, $modalInstance, mealPlanning, mealPlanningFactory, ErrorFactory, notificationFactory,
menuOptions, commonFactory, mealPlanningUtilities, currentMealPlanning) {

	$scope.menuOptions = menuOptions;
	$scope.foodItems = [];
	$scope.currentMealPlanning = currentMealPlanning;

	$scope.calendar = {
		startDateOpened: false
	};

	$scope.mealPlanning = {
		menuId: mealPlanning !== '' ? mealPlanning.FoodMenu.MenuId : '',
		onDate: mealPlanning !== '' ? mealPlanning.Date : new Date(),
		isActive: mealPlanning !== '' ? mealPlanning.IsActive : ''
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

	getBasicFoodItems(null, $scope.mealPlanning.menuId, true);

	$scope.getFoodItems = function (menuId) {
		getBasicFoodItems(null, menuId, true);
	};

	/*
	** Set index to sort.
	*/
	$scope.setIndex = function (index) {
		$scope.selectedIndex = index;
	};

	/*
	** Upward or downward food item.
	*/
	$scope.upFoodItems = function (selectedIndex) {
		if (selectedIndex > 0) {
			var currentItem = angular.copy($scope.foodItems[selectedIndex]);

			$scope.foodItems[selectedIndex] = angular.copy($scope.foodItems[selectedIndex - 1]);
			$scope.foodItems[selectedIndex - 1] = currentItem;

			$scope.selectedIndex = selectedIndex - 1;
		}
	};

	$scope.downFoodItems = function (selectedIndex) {
		if (selectedIndex < $scope.foodItems.length - 1) {
			var currentItem = angular.copy($scope.foodItems[selectedIndex]);

			$scope.foodItems[selectedIndex] = angular.copy($scope.foodItems[selectedIndex + 1]);
			$scope.foodItems[selectedIndex + 1] = currentItem;

			$scope.selectedIndex = selectedIndex + 1;
		}
	};

	/*
	** Create meal planning.
	*/
	$scope.save = function () {
		$scope.disabledSubmitButton = true;
		$scope.mealPlanning.foodId = mealPlanningUtilities.getFoodId($scope.foodItems);

		if ($scope.mealPlanning.onDate instanceof Date) {
			$scope.mealPlanning.onDate = $scope.mealPlanning.onDate.toDateString();
		}

		if (mealPlanningUtilities.validateMealPlanning($scope.mealPlanning)) {
			$modalInstance.close($scope.mealPlanning);
		}
		else {
			$scope.disabledSubmitButton = false;
		}
	};

	/*
	** Create and continue create meal planning.
	*/
	$scope.saveAndContinue = function () {
		$scope.disabledSubmitButton = true;
		$scope.mealPlanning.foodId = mealPlanningUtilities.getFoodId($scope.foodItems);

		if ($scope.mealPlanning.onDate instanceof Date) {
			$scope.mealPlanning.onDate = $scope.mealPlanning.onDate.toDateString();
		}

		if (mealPlanningUtilities.validateMealPlanning($scope.mealPlanning)) {
			mealPlanningFactory.insertMealPlanning($scope.mealPlanning)
			.success(function (response) {
				if (response.IsSuccessful === true) {
					$scope.foodItems = [];
					$scope.$broadcast('resetMealPlanning');
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("ErrorMessage");
				}
			})['finally'](function () {
				$scope.disabledSubmitButton = false;
			});
		}
		else {
			$scope.disabledSubmitButton = false;
		}
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