'use strict';
/*global mealPlanningManagementModule*/

mealPlanningManagementModule.controller('MealPlanningManagementController',
['$scope', '$modal', 'mealPlanningFactory', 'ErrorFactory', 'notificationFactory', 'commonFactory',
'foodMenuFactory', 'foodMenuUtilities', 'mealPlanningUtilities',
function ($scope, $modal, mealPlanningFactory, ErrorFactory, notificationFactory, commonFactory,
foodMenuFactory, foodMenuUtilities, mealPlanningUtilities) {
	/*
	** Declare variables.
	*/
	$scope.originalPage = 0;
	var toDay = new Date();

	var startDate = new Date(),
		endDate = new Date();

	$scope.filter = {
		startDate: new Date(startDate),
		endDate: new Date(endDate)
	};

	$scope.calendar = {
		startDateOpened: false,
		endDateOpened: false
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
	** Get all meal timesheet.
	*/
	mealPlanningFactory.getAllMealTimesheet().success(function (mealTimesheets) {
		$scope.mealTimesheets = mealTimesheets;
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});
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
	** Get user permission.
	*/
	mealPlanningFactory.getUserPermissionOnMealPlanning().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

	var checkRightMeal = function (mealPlannings) {
		angular.forEach(mealPlannings, function (mealPlanning) {
			if (!mealPlanning.IsDisable) {
				var numberMeals = mealPlanning.FoodMenu.Meals.length,
				lastMeal = numberMeals > 0 ? mealPlanning.FoodMenu.Meals[numberMeals - 1] : '';

				if (lastMeal) {
					mealPlanning.IsDisable = mealPlanningUtilities.checkMealTimesheet($scope.mealTimesheets, lastMeal, mealPlanning.Date);
				}
			}
		});
	};
	/*
	** Get all meal planning.
	*/
	var getAllMealPlanning = function (fromDate, toDate, mealId, employeeTypeId) {
		mealPlanningFactory.getAllMealPlanning(fromDate, toDate, mealId, employeeTypeId)
		.success(function (mealPlannings) {
			$scope.mealPlannings = mealPlannings;

			if ($scope.mealPlannings && $scope.mealPlannings !== 'null') {
				checkRightMeal($scope.mealPlannings);
			}

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
		$scope.originalPage = $scope.currentPage;

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
			});
		})['finally'](function () {
			var fromDate = angular.copy($scope.filter.startDate).toDateString(),
					toDate = angular.copy($scope.filter.endDate).toDateString(),
					mealId = foodMenuUtilities.getMealId($scope.mealsOption),
					employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
			getAllMealPlanning(fromDate, toDate, mealId, employeeTypeId);
			$scope.modalInstance = '';
		});
	};

	/*
	** Edit meal planning.
	*/
	$scope.edit = function (mealPlanning) {
		var udpatedData = function (updateMealPlanning, foodItems) {
			mealPlanning.IsActive = updateMealPlanning.isActive;
			mealPlanning.FoodMenu.FoodItems = foodItems;
		};

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/MealPlanningManagement/Templates/edit.html',
			controller: "EditMealPlanningController",
			resolve: {
				mealPlanning: function () {
					return angular.copy(mealPlanning);
				},
				menuOptions: function () {
					return $scope.menuOptions;
				},
				mealTimesheets: function () {
					return $scope.mealTimesheets;
				}
			},
			windowClass: "modal-confirm-center"
		});

		$scope.modalInstance.result.then(function (informationMealPlanning) {
			mealPlanningFactory.updateMealPlanning(informationMealPlanning.updateMealPlanning)
			.success(function (response) {
				if (response.IsSuccessful === true) {
					udpatedData(informationMealPlanning.updateMealPlanning, informationMealPlanning.foodItems);
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
				$scope.modalInstance = '';
			});
		});
	};

	/*
	** Activate or deactivate food menu.
	*/
	$scope.confirmActive = function (mealPlanning) {
		var action = !mealPlanning.IsActive ? "activate" : "deactivate",
			newMealPlanning = {
				menuId: mealPlanning.FoodMenu.MenuId,
				IsActive: !mealPlanning.IsActive,
				onDate: mealPlanning.Date
			};

		mealPlanning.IsActive = newMealPlanning.IsActive;

		var numberMeals = mealPlanning.FoodMenu.Meals.length,
				lastMeal = numberMeals > 0 ? mealPlanning.FoodMenu.Meals[numberMeals - 1] : '';

		if (lastMeal) {
			var disable = mealPlanningUtilities.checkMealTimesheet($scope.mealTimesheets, lastMeal, mealPlanning.Date);

			if (disable) {
				mealPlanning.IsActive = !newMealPlanning.IsActive;
				mealPlanning.IsDisable = disable;

				if (action === 'activate') {
					notificationFactory.error("MealPlanningActivateWarning");
				}
				else {
					notificationFactory.error("MealPlanningDeactivateWarning");
				}
			}
			else {
				$scope.modalInstance = $modal.open({
					templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
					controller: "ModalConfirmController",
					resolve: {
						name: function () {
							return mealPlanning.FoodMenu.MenuVnmName + ' ( on ' + new Date(mealPlanning.Date).toString("MMM dd, yyyy") + ')';
						},
						action: function () {
							return action;
						}
					},
					windowClass: "modal-confirm-center food-item-modal"
				});

				var keyMessage = '';

				$scope.modalInstance.result.then(function () {
					mealPlanningFactory.activateMealPlanning(newMealPlanning).success(function (response) {
						if (response.IsSuccessful === true) {
							keyMessage = action === "activate" ? "ActivationSuccess" : "DeactivationSuccess";
							notificationFactory.success(keyMessage);
						}
						else {
							keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
							mealPlanning.IsActive = !newMealPlanning.IsActive;

							notificationFactory.error(keyMessage);
						}
					}).error(function (response, status) {
						if (status === 405) {
							ErrorFactory.errorHandle(status);
						}
						else {
							keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
							mealPlanning.IsActive = !newMealPlanning.IsActive;
							notificationFactory.error(keyMessage);
						}
					});
				}, function () {
					mealPlanning.IsActive = !newMealPlanning.IsActive;
				})['finally'](function () {
					$scope.modalInstance = '';
				});
			}
		}
	};

	/*
	** Delete meal planning.
	*/
	var updateDeleteMealPlanning = function (mealPlanning) {
		mealPlanning.FoodMenu.FoodItems = [];
		mealPlanning.IsExist = false;
		mealPlanning.IsActive = false;
	};

	$scope.deleteMealPlanning = function (mealPlanning) {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			resolve: {
				action: function () {
					return 'delete';
				},
				name: function () {
					return mealPlanning.FoodMenu.MenuVnmName + ' ( on ' + new Date(mealPlanning.Date).toString("MMM dd, yyyy") + ')';
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		$scope.modalInstance.result.then(function () {
			var menuId = mealPlanning.FoodMenu.MenuId,
				onDate = mealPlanning.Date;

			mealPlanningFactory.deleteMealPlanning(menuId, onDate).success(function (response) {
				if (response.IsSuccessful === true) {
					updateDeleteMealPlanning(mealPlanning);
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
			});
		})['finally'](function () {
			$scope.modalInstance = '';
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
		onDate: '',
		isActive: mealPlanning !== '' ? mealPlanning.IsActive : '',
		date: mealPlanning !== '' ? mealPlanning.Date : new Date()
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
	mealPlanningUtilities.getBasicFoodItems(null, $scope.mealPlanning.menuId, true, $scope);

	$scope.getFoodItems = function (menuId) {
		mealPlanningUtilities.getBasicFoodItems(null, menuId, true, $scope);
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
		mealPlanningUtilities.upFoodItems(selectedIndex, $scope);
	};

	$scope.downFoodItems = function (selectedIndex) {
		mealPlanningUtilities.downFoodItems(selectedIndex, $scope);
	};

	/*
	** Create meal planning.
	*/
	$scope.save = function () {
		$scope.disabledSubmitButton = true;
		$scope.mealPlanning.foodId = mealPlanningUtilities.getFoodId($scope.foodItems);
		$scope.mealPlanning.onDate = $scope.mealPlanning.date;

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
		$scope.mealPlanning.onDate = $scope.mealPlanning.date;

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
['$scope', '$modalInstance', 'mealPlanning', 'menuOptions', 'commonFactory', 'mealPlanningFactory',
'mealPlanningUtilities', 'ErrorFactory', 'mealTimesheets', 'notificationFactory',
function ($scope, $modalInstance, mealPlanning, menuOptions, commonFactory, mealPlanningFactory,
mealPlanningUtilities, ErrorFactory, mealTimesheets, notificationFactory) {
	/*
	** Declare variables.
	*/
	$scope.mealPlanning = mealPlanning;
	$scope.menuOptions = menuOptions;
	$scope.foodItems = mealPlanning.FoodMenu.FoodItems;
	$scope.mealTimesheets = mealTimesheets;

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
	** Create list food item Id.
	*/
	var createFoodIds = function (foodItems) {
		var foodIds = [];

		angular.forEach(foodItems, function (foodItem) {
			foodIds.push(foodItem.FoodId);
		});

		return foodIds;
	};

	$scope.mealPlanning.rawFoodIds = createFoodIds($scope.mealPlanning.FoodMenu.FoodItems);

	/*
	** Get all food items of current menu.
	*/
	mealPlanningFactory.getFoodByOnDate(mealPlanning.Date, mealPlanning.FoodMenu.MenuId)
	.success(function (foodItems) {
		$scope.groupOptions = foodItems;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

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
		mealPlanningUtilities.upFoodItems(selectedIndex, $scope);
	};

	$scope.downFoodItems = function (selectedIndex) {
		mealPlanningUtilities.downFoodItems(selectedIndex, $scope);
	};

	/*
	** Check overdue meal planning.
	*/
	var checkOverdueMeal = function (mealPlanning) {
		var disable = false,
			numberMeals = mealPlanning.FoodMenu.Meals.length,
			lastMeal = numberMeals > 0 ? mealPlanning.FoodMenu.Meals[numberMeals - 1] : '';

		if (lastMeal) {
			disable = mealPlanningUtilities.checkMealTimesheet($scope.mealTimesheets, lastMeal, mealPlanning.Date);
		}

		return disable;
	};

	/*
	** Edit meal planning.
	*/
	$scope.editMealPlanning = function (mealPlanning) {
		$scope.disabledSubmitButton = true;

		var updateMealPlanning = {
			menuId: mealPlanning.FoodMenu.MenuId,
			onDate: mealPlanning.Date,
			isActive: mealPlanning.IsActive,
			foodId: mealPlanningUtilities.getFoodId($scope.foodItems)
		};

		var disable = checkOverdueMeal(mealPlanning);

		if (disable) {
			var action = !mealPlanning.IsActive ? "activate" : "deactivate";
			mealPlanning.IsActive = !updateMealPlanning.IsActive;
			mealPlanning.IsDisable = disable;

			if (action === 'activate') {
				notificationFactory.error("MealPlanningActivateWarning");
			}
			else {
				notificationFactory.error("MealPlanningDeactivateWarning");
			}
		}
		else {
			var informationMealPlanning = {
				updateMealPlanning: updateMealPlanning,
				foodItems: $scope.foodItems
			};

			if (mealPlanningUtilities.validateMealPlanning(updateMealPlanning)) {
				$modalInstance.close(informationMealPlanning);
			}
			else {
				$scope.disabledSubmitButton = false;
			}
		}
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);