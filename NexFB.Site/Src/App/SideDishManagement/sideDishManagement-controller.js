'use strict';
/*global sideDishManagementModule*/

sideDishManagementModule.controller('SideDishManagementController',
['$scope', 'sideDishManagementFactory', 'commonFactory', 'ErrorFactory', '$modal', 'foodMenuFactory',
'notificationFactory',
function ($scope, sideDishManagementFactory, commonFactory, ErrorFactory, $modal, foodMenuFactory,
notificationFactory) {

	/*
	** Paging.
	*/
	$scope.showEntries = function () {
		commonFactory.tablePagination($scope, $scope.sideDishes, $scope.entrySelected);
	};

	$scope.entrySelected = 10;
	$scope.originalPage = 0;
	commonFactory.numberEntryList($scope);

	/*
	** Get user permission on side dish management.
	*/
	sideDishManagementFactory.getPermissionOnSideDish().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

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
	** Get all employee type.
	*/
	foodMenuFactory.getAllEmployeeType().success(function (employeeTypes) {
		$scope.employeeTypesOption = employeeTypes;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all food unit.
	*/
	sideDishManagementFactory.getAllFoodUnit(null).success(function (units) {
		$scope.unitOptions = units;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all side dishes.
	*/
	var getAllSideDish = function (foodId, isActive) {
		sideDishManagementFactory.getAllSideDish(foodId, isActive).success(function (sideDishes) {
			$scope.sideDishes = sideDishes;
			commonFactory.tablePagination($scope, $scope.sideDishes, $scope.entrySelected);
			$scope.currentPage = $scope.originalPage;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	getAllSideDish(null, null);

	/*
	** Create side dish.
	*/
	$scope.addSideDish = function () {
		$scope.originalPage = $scope.currentPage;

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/SideDishManagement/Templates/create.html',
			controller: "CreateSideDishController",
			resolve: {
				employeeTypesOption: function () {
					return angular.copy($scope.employeeTypesOption);
				},
				unitOptions: function () {
					return angular.copy($scope.unitOptions);
				}
			},
			windowClass: "modal-confirm-center side-dish-modal"
		});

		$scope.modalInstance.result.then(function (sideDish) {
			sideDishManagementFactory.insertSideDish(sideDish).success(function (response) {
				if (response.IsSuccessful === true) {
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		})['finally'](function () {
			getAllSideDish(null, null);
			$scope.modalInstance = '';
		});
	};

	/*
	** Edit side dish.
	*/
	$scope.editSideDish = function (sideDish) {
		var updatedSideDish = function (sideDish, updatedSideDish) {
			sideDish.FoodCode = updatedSideDish.FoodCode;
			sideDish.FoodVnmName = updatedSideDish.FoodVnmName;
			sideDish.FoodEngName = updatedSideDish.FoodEngName;
			sideDish.IsActive = updatedSideDish.IsActive;
			sideDish.Price = isNaN(updatedSideDish.Price) ? parseInt(updatedSideDish.Price.replace(/\,/g, '')) : updatedSideDish.Price;
			sideDish.Unit = updatedSideDish.Unit;
			sideDish.EmployeeTypes = updatedSideDish.EmployeeTypes;
		};

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/SideDishManagement/Templates/edit.html',
			controller: "EditSideDishController",
			resolve: {
				sideDish: function () {
					return angular.copy(sideDish);
				},
				employeeTypesOption: function () {
					return angular.copy($scope.employeeTypesOption);
				},
				unitOptions: function () {
					return angular.copy($scope.unitOptions);
				}
			},
			windowClass: "modal-confirm-center side-dish-modal"
		});

		$scope.modalInstance.result.then(function (allInfoSideDish) {
			sideDishManagementFactory.updateSideDish(allInfoSideDish.updatedSideDish).success(function (response) {
				if (response.IsSuccessful === true) {
					updatedSideDish(sideDish, allInfoSideDish.sideDish);
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

	/*
	** Activate or deactivate side dish.
	*/
	$scope.confirmActive = function (sideDish) {
		var action = !sideDish.IsActive ? "activate" : "deactivate",
			newSideDish = {
				foodId: sideDish.FoodId,
				IsActive: !sideDish.IsActive
			};

		sideDish.IsActive = newSideDish.IsActive;

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			resolve: {
				name: function () {
					return sideDish.FoodVnmName;
				},
				action: function () {
					return action;
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		var keyMessage = '';

		$scope.modalInstance.result.then(function () {
			sideDishManagementFactory.activateSideDish(newSideDish).success(function (response) {
				if (response.IsSuccessful === true) {
					keyMessage = action === "activate" ? "ActivationSuccess" : "DeactivationSuccess";
					notificationFactory.success(keyMessage);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					sideDish.IsActive = !newSideDish.IsActive;

					notificationFactory.error(keyMessage);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					sideDish.IsActive = !newSideDish.IsActive;
					notificationFactory.error(keyMessage);
				}
			});
		}, function () {
			sideDish.IsActive = !newSideDish.IsActive;
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};

	/*
	** Delete side dish.
	*/
	var updatePaging = function (sideDish, currentPage) {
		var index = $scope.sideDishes.indexOf(sideDish);
		$scope.sideDishes.splice(index, 1);

		commonFactory.tablePagination($scope, $scope.sideDishes, $scope.entrySelected);
		var numberPage = $scope.pagedItems.length;

		if (currentPage <= numberPage - 1) {
			$scope.currentPage = currentPage;
		}
		else {
			$scope.currentPage = currentPage - 1;
		}
	};

	$scope.deleteSideDish = function (sideDish) {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			resolve: {
				action: function () {
					return 'delete';
				},
				name: function () {
					return sideDish.FoodVnmName;
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		$scope.modalInstance.result.then(function () {
			var currentPage = $scope.currentPage;

			sideDishManagementFactory.deleteSideDish(sideDish.FoodId).success(function (response) {
				if (response.IsSuccessful === true) {
					updatePaging(sideDish, currentPage);
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

sideDishManagementModule.controller('CreateSideDishController',
['$scope', '$modalInstance', 'unitOptions', 'employeeTypesOption', 'foodMenuUtilities', 'sideDishUtilities',
function ($scope, $modalInstance, unitOptions, employeeTypesOption, foodMenuUtilities, sideDishUtilities) {
	/*
	** Declare variables.
	*/
	$scope.unitOptions = unitOptions;
	$scope.employeeTypesOption = employeeTypesOption;

	$scope.sideDish = {
		foodCode: '',
		foodVnmName: '',
		foodEngName: '',
		unitId: '',
		isActive: false,
		price:'',
		employeeTypeId: ''
	};

	/*
	** Add new side dish.
	*/
	$scope.save = function (sideDish) {
		$scope.disabledSubmitButton = true;

		if (sideDish) {
			sideDish.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
		}

		if (sideDishUtilities.validateSideDish(sideDish)) {
			$modalInstance.close(sideDish);
		}
		else {
			$scope.disabledSubmitButton = false;
		}
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);

sideDishManagementModule.controller('EditSideDishController',
['$scope', '$modalInstance', 'sideDish', 'unitOptions', 'employeeTypesOption', 'foodMenuUtilities', 
'sideDishUtilities', 'commonFactory',
function ($scope, $modalInstance, sideDish, unitOptions, employeeTypesOption, foodMenuUtilities,
sideDishUtilities, commonFactory) {
	/*
	** Declare variables.
	*/
	$scope.sideDish = sideDish;
	$scope.unitOptions = unitOptions;
	$scope.employeeTypesOption = employeeTypesOption;

	/*
	** Check employee type that belong to side dish.
	*/
	var checkEmployeeTypeBelongToSideDish = function (employeeTypeId, employeeTypes) {
		var exist = false;

		angular.forEach(employeeTypes, function (employeeType) {
			if (employeeType.EmployeeTypeId === employeeTypeId) {
				exist = true;
			}
		});

		return exist;
	};

	var checkEmployeeTypeOptions = function () {
		angular.forEach($scope.employeeTypesOption, function (employeeType) {
			employeeType.ticked = checkEmployeeTypeBelongToSideDish(employeeType.EmployeeTypeId, $scope.sideDish.EmployeeTypes);
		});
	};

	checkEmployeeTypeOptions();

	/*
	** Get update unit.
	*/
	var getUpdateUnit = function (unitId, unitOptions) {
		for (var i = 0; i < unitOptions.length; i++) {
			var unit = unitOptions[i];

			if (unit.UnitId === unitId) {
				return unit;
			}
		}

		return '';
	};

	/*
	** Edit side dish.
	*/
	$scope.save = function (sideDish) {
		$scope.disabledSubmitButton = true;

		var updatedSideDish = {
			foodId: sideDish.FoodId,
			foodCode: sideDish.FoodCode,
			foodVnmName: sideDish.FoodVnmName,
			foodEngName: sideDish.FoodEngName,
			isActive: sideDish.IsActive,
			unitId: sideDish.Unit.UnitId,
			price: sideDish.Price,
			employeeTypeId: foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption)
		};

		if (sideDishUtilities.validateSideDish(updatedSideDish)) {
			sideDish.Unit = getUpdateUnit(updatedSideDish.unitId, $scope.unitOptions);
			sideDish.EmployeeTypes = commonFactory.updateEmployeeTypes(updatedSideDish.employeeTypeId, $scope.employeeTypesOption);

			var allInfoSideDish = {
				updatedSideDish: updatedSideDish,
				sideDish: sideDish
			};

			$modalInstance.close(allInfoSideDish);
		}
		else {
			$scope.disabledSubmitButton = false;
		}
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);