'use strict';
/*global sideDishManagementModule*/

sideDishManagementModule.controller('SideDishManagementController',
['$scope', 'sideDishManagementFactory', 'commonFactory', 'ErrorFactory', '$modal', 'foodMenuFactory',
'notificationFactory', '$translate',
function ($scope, sideDishManagementFactory, commonFactory, ErrorFactory, $modal, foodMenuFactory,
notificationFactory, $translate) {

	var getMessage = function () {
		$translate('CheckKitchenOnlyMsg').then(function (message) {
			$scope.checkKitchenOnlyMessage = message;
		});
		$translate('UncheckKitchenOnlyMsg').then(function (message) {
			$scope.uncheckKitchenOnlyMessage = message;
		});
	};

	getMessage();

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
			backdrop: 'static',
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

		$scope.modalInstance.result.then()['finally'](function () {
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
			sideDish.KitchenOnly = updatedSideDish.KitchenOnly;
			sideDish.Price = isNaN(updatedSideDish.Price) ? parseInt(updatedSideDish.Price.replace(/\,/g, '')) : updatedSideDish.Price;
			sideDish.Unit = updatedSideDish.Unit;
			sideDish.EmployeeTypes = updatedSideDish.EmployeeTypes;
		};

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/SideDishManagement/Templates/edit.html',
			controller: "EditSideDishController",
			backdrop: 'static',
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
			updatedSideDish(sideDish, allInfoSideDish.sideDish);
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
			backdrop: 'static',
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
				if (response.IsSuccessful) {
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

	$scope.confirmKitchenOnly = function (sideDish) {
		var messageContent = !sideDish.KitchenOnly ? $scope.checkKitchenOnlyMessage : $scope.uncheckKitchenOnlyMessage,
			newSideDish = {
				foodId: sideDish.FoodId,
				KitchenOnly: !sideDish.KitchenOnly
			};

		sideDish.KitchenOnly = newSideDish.KitchenOnly;

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent2.html',
			controller: "ModalConfirmController2",
			backdrop: 'static',
			resolve: {
				content: function () {
					return messageContent;
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		$scope.modalInstance.result.then(function () {
			sideDishManagementFactory.updateKitchenOnly(newSideDish).success(function (response) {
				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					sideDish.KitchenOnly = !newSideDish.KitchenOnly;
					notificationFactory.error("DataChangeFail");
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					sideDish.KitchenOnly = !newSideDish.KitchenOnly;
					notificationFactory.error("DataChangeFail");
				}
			});
		}, function () {
			sideDish.KitchenOnly = !newSideDish.KitchenOnly;
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
			backdrop: 'static',
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
				if (response.IsSuccessful) {
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
'sideDishManagementFactory', 'notificationFactory', 'ErrorFactory',
function ($scope, $modalInstance, unitOptions, employeeTypesOption, foodMenuUtilities, sideDishUtilities,
sideDishManagementFactory, notificationFactory, ErrorFactory) {
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
		isActive: true,
		kitchenOnly: false,
		price: '',
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
			sideDishManagementFactory.insertSideDish(sideDish).success(function (response) {
				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
					$modalInstance.close(sideDish);
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
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

sideDishManagementModule.controller('EditSideDishController',
['$scope', '$modalInstance', 'sideDish', 'unitOptions', 'employeeTypesOption', 'foodMenuUtilities',
'sideDishUtilities', 'commonFactory', 'sideDishManagementFactory', 'notificationFactory', 'ErrorFactory',
function ($scope, $modalInstance, sideDish, unitOptions, employeeTypesOption, foodMenuUtilities,
sideDishUtilities, commonFactory, sideDishManagementFactory, notificationFactory, ErrorFactory) {
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
			kitchenOnly: sideDish.KitchenOnly,
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

			sideDishManagementFactory.updateSideDish(allInfoSideDish.updatedSideDish).success(function (response) {
				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
					$modalInstance.close(allInfoSideDish);
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
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