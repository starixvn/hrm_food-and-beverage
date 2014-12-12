'use strict';
/*global mealPlanningManagementModule*/

mealPlanningManagementModule.controller('MealPlanningManagementController',
['$scope', '$modal', 'mealPlanningFactory', 'ErrorFactory', 'notificationFactory', 'commonFactory',
'foodMenuFactory', 'foodMenuUtilities', 'mealPlanningUtilities', 'foodItemFactory',
function ($scope, $modal, mealPlanningFactory, ErrorFactory, notificationFactory, commonFactory,
foodMenuFactory, foodMenuUtilities, mealPlanningUtilities, foodItemFactory) {
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
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
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
	** Get all foods item.
	*/
	foodItemFactory.getAllFoodItem(null, true).success(function (foods) {
		$scope.foodOptions = foods;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all food menu to filter.
	*/
	foodMenuFactory.getAllFoodMenuToFilter().success(function (foodMenus) {
		$scope.menuOptions = foodMenus;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all meal timesheet.
	*/
	foodMenuFactory.getAllMealTimesheet().success(function (meals) {
		$scope.mealsOption = meals;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all employee type.
	*/
	foodMenuFactory.getAllEmployeeType(null, null).success(function (employeeTypes) {
		$scope.employeeTypesOption = employeeTypes;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get user permission.
	*/
	mealPlanningFactory.getUserPermissionOnMealPlanning().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
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
			ErrorFactory.errorHandle(status);
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
			backdrop: 'static',
			resolve: {
				mealPlanning: function () {
					return mealPlanning;
				},
				menuOptions: function () {
					return $scope.menuOptions;
				},
				foodOptions: function () {
					return $scope.foodOptions;
				},
				currentMealPlanning: function () {
					return currentMealPlanning;
				}
			},
			windowClass: "modal-confirm-center meal-planning-modal"
		});

		$scope.modalInstance.result.then()['finally'](function () {
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
		var udpatedData = function (updateMealPlanning, setMenu, foodItems) {
			mealPlanning.IsActive = updateMealPlanning.isActive;

			if (setMenu) {
				mealPlanning.SetMenu = setMenu;
			}

			mealPlanning.SetMenu.FoodItems = foodItems;
		};

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/MealPlanningManagement/Templates/edit.html',
			controller: "EditMealPlanningController",
			backdrop: 'static',
			resolve: {
				mealPlanning: function () {
					return angular.copy(mealPlanning);
				},
				menuOptions: function () {
					return $scope.menuOptions;
				},
				foodOptions: function () {
					return $scope.foodOptions;
				},
				mealTimesheets: function () {
					return $scope.mealTimesheets;
				},
				isOriginalActive: function () {
					return mealPlanning.IsActive;
				}
			},
			windowClass: "modal-confirm-center meal-planning-modal"
		});

		$scope.modalInstance.result.then(function (informationMealPlanning) {
			udpatedData(informationMealPlanning.updateMealPlanning, informationMealPlanning.setMenu, informationMealPlanning.foodItems);
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};

	/*
	** Activate or deactivate food menu.
	*/
	$scope.confirmActive = function (mealPlanning) {
		var action = !mealPlanning.IsActive ? "activate" : "deactivate",
			newMealPlanning = {
				menuId: mealPlanning.Menu.MenuId,
				IsActive: !mealPlanning.IsActive,
				onDate: mealPlanning.Date
			};

		mealPlanning.IsActive = newMealPlanning.IsActive;

		var mealId = mealPlanningUtilities.createMealId(mealPlanning.Menu.Meals),
			employeeTypeId = mealPlanningUtilities.createEmployeeTypeId(mealPlanning.Menu.EmployeeTypes);

		mealPlanningFactory.checkDisableMealPlanning(mealPlanning.Date.toString(), mealId, employeeTypeId, mealPlanning.Menu.MenuId)
		.success(function (isDisable) {
			if (isDisable === 'true') {
				mealPlanning.IsActive = !newMealPlanning.IsActive;
				mealPlanning.IsDisable = isDisable;

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
					backdrop: 'static',
					resolve: {
						name: function () {
							return mealPlanning.Menu.MenuVnmName + ' ( on ' + new Date(mealPlanning.Date).toString("MMM dd, yyyy") + ')';
						},
						action: function () {
							return action;
						}
					},
					windowClass: "modal-confirm-center"
				});

				var keyMessage = '';

				$scope.modalInstance.result.then(function () {
					mealPlanningFactory.activateMealPlanning(newMealPlanning).success(function (response) {
						if (response.IsSuccessful) {
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
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	/*
	** Delete meal planning.
	*/
	var updateDeleteMealPlanning = function (mealPlanning) {
		mealPlanning.SetMenu.SetMenuId = '';
		mealPlanning.SetMenu.SetMenuVnmName = '';
		mealPlanning.SetMenu.FoodItems = [];
		mealPlanning.IsExist = false;
		mealPlanning.IsActive = false;
	};

	$scope.deleteMealPlanning = function (mealPlanning) {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			backdrop: 'static',
			resolve: {
				action: function () {
					return 'delete';
				},
				name: function () {
					return mealPlanning.Menu.MenuVnmName + ' ( on ' + new Date(mealPlanning.Date).toString("MMM dd, yyyy") + ')';
				}
			},
			windowClass: "modal-confirm-center"
		});

		$scope.modalInstance.result.then(function () {
			var menuId = mealPlanning.Menu.MenuId,
				onDate = mealPlanning.Date;

			mealPlanningFactory.deleteMealPlanning(menuId, onDate).success(function (response) {
				if (response.IsSuccessful) {
					updateDeleteMealPlanning(mealPlanning);
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};
} ]);