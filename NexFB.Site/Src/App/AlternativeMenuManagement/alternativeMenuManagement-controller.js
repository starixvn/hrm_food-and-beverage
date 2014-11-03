'use strict';
/*global alternativeMenuModule*/

alternativeMenuModule.controller('AlternativeMenuController',
['$scope', 'alternativeMenuFactory', 'ErrorFactory', 'commonFactory', 'foodMenuFactory', '$modal',
'notificationFactory',
function ($scope, alternativeMenuFactory, ErrorFactory, commonFactory, foodMenuFactory, $modal,
notificationFactory) {

	/*
	** Paging.
	*/
	$scope.showEntries = function () {
		commonFactory.tablePagination($scope, $scope.alternativeMenus, $scope.entrySelected);
	};

	$scope.entrySelected = 10;
	$scope.originalPage = 0;
	commonFactory.numberEntryList($scope);

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
	** Get permisson on Alternative Menu.
	*/
	alternativeMenuFactory.getPermissionOnAlternativeMenu().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
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
	** Get all alternative menu.
	*/
	var getAllAlternativeMenu = function (alternativeMenuId, isActive) {
		alternativeMenuFactory.getAllAlternativeMenu(alternativeMenuId, isActive)
		.success(function (alternativeMenus) {
			$scope.alternativeMenus = alternativeMenus;
			commonFactory.tablePagination($scope, $scope.alternativeMenus, $scope.entrySelected);
			$scope.currentPage = $scope.originalPage;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	getAllAlternativeMenu(null, null);

	/*
	** Create alternative menu.
	*/
	$scope.addAltMenu = function () {
		$scope.originalPage = $scope.currentPage;

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/AlternativeMenuManagement/Templates/create.html',
			controller: "CreateAlternativeMenuController",
			resolve: {
				employeeTypesOption: function () {
					return angular.copy($scope.employeeTypesOption);
				}
			},
			windowClass: "modal-confirm-center side-dish-modal"
		});

		$scope.modalInstance.result.then()['finally'](function () {
			getAllAlternativeMenu(null, null);
			$scope.modalInstance = '';
		});
	};

	/*
	** Edit alternative menu.
	*/
	var updateAlternativeMenu = function (altMenu, updatedAltMenu) {
		altMenu.AlternativeMenuCode = updatedAltMenu.AlternativeMenuCode;
		altMenu.AlternativeMenuVnmName = updatedAltMenu.AlternativeMenuVnmName;
		altMenu.AlternativeMenuEngName = updatedAltMenu.AlternativeMenuEngName;
		altMenu.IsActive = updatedAltMenu.IsActive;
		altMenu.EmployeeTypes = updatedAltMenu.EmployeeTypes;
	};

	$scope.editAltMenu = function (altMenu) {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/AlternativeMenuManagement/Templates/edit.html',
			controller: "EditAlternativeMenuController",
			resolve: {
				altMenu: function () {
					return angular.copy(altMenu);
				},
				employeeTypesOption: function () {
					return angular.copy($scope.employeeTypesOption);
				}
			},
			windowClass: "modal-confirm-center side-dish-modal"
		});

		$scope.modalInstance.result.then(function (updatedAltMenu) {
			updateAlternativeMenu(altMenu, updatedAltMenu);
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};

	/*
	** Activate or deactivate alternative menu.
	*/
	$scope.confirmActive = function (altMenu) {
		var action = !altMenu.IsActive ? "activate" : "deactivate",
			newAltMenu = {
				alternativeMenuId: altMenu.AlternativeMenuId,
				IsActive: !altMenu.IsActive
			};

		altMenu.IsActive = newAltMenu.IsActive;

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			resolve: {
				name: function () {
					return altMenu.AlternativeMenuVnmName;
				},
				action: function () {
					return action;
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		var keyMessage = '';

		$scope.modalInstance.result.then(function () {
			alternativeMenuFactory.activateAlternativeMenu(newAltMenu).success(function (response) {
				if (response.IsSuccessful) {
					keyMessage = action === "activate" ? "ActivationSuccess" : "DeactivationSuccess";
					notificationFactory.success(keyMessage);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					altMenu.IsActive = !newAltMenu.IsActive;
					notificationFactory.error(keyMessage);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					altMenu.IsActive = !newAltMenu.IsActive;
					notificationFactory.error(keyMessage);
				}
			});
		}, function () {
			altMenu.IsActive = !newAltMenu.IsActive;
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};

	/*
	** Delete alternative menu.
	*/
	var updatePaging = function (altMenu, currentPage) {
		var index = $scope.alternativeMenus.indexOf(altMenu);
		$scope.alternativeMenus.splice(index, 1);

		commonFactory.tablePagination($scope, $scope.alternativeMenus, $scope.entrySelected);
		var numberPage = $scope.pagedItems.length;

		if (currentPage <= numberPage - 1) {
			$scope.currentPage = currentPage;
		}
		else {
			$scope.currentPage = currentPage - 1;
		}
	};

	$scope.deleteAltMenu = function (altMenu) {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			resolve: {
				action: function () {
					return 'delete';
				},
				name: function () {
					return altMenu.AlternativeMenuVnmName;
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		$scope.modalInstance.result.then(function () {
			var currentPage = $scope.currentPage;

			alternativeMenuFactory.deleteAlternativeMenu(altMenu.AlternativeMenuId).success(function (response) {
				if (response.IsSuccessful) {
					updatePaging(altMenu, currentPage);
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

alternativeMenuModule.controller('CreateAlternativeMenuController',
['$scope', '$modalInstance', 'employeeTypesOption', 'foodMenuUtilities', 'alternativeMenuUtilities',
'alternativeMenuFactory', 'ErrorFactory', 'notificationFactory',
function ($scope, $modalInstance, employeeTypesOption, foodMenuUtilities, alternativeMenuUtilities,
alternativeMenuFactory, ErrorFactory, notificationFactory) {
	$scope.employeeTypesOption = employeeTypesOption;

	$scope.altMenu = {
		alternativeMenuCode: '',
		alternativeMenuVnmName: '',
		alternativeMenuEngName: '',
		isActive: '',
		employeeTypeId: ''
	};

	/*
	** Add new alternative menu.
	*/
	$scope.save = function (altMenu) {
		$scope.disabledSubmitButton = true;

		if (altMenu) {
			altMenu.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);
		}

		if (alternativeMenuUtilities.validateAlternativeMenu(altMenu)) {
			alternativeMenuFactory.insertAlternativeMenu(altMenu).success(function (response) {
				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
					$modalInstance.close();
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
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

alternativeMenuModule.controller('EditAlternativeMenuController',
['$scope', '$modalInstance', 'employeeTypesOption', 'altMenu', 'foodMenuUtilities', 'alternativeMenuUtilities',
'alternativeMenuFactory', 'notificationFactory', 'ErrorFactory', 'commonFactory',
function ($scope, $modalInstance, employeeTypesOption, altMenu, foodMenuUtilities, alternativeMenuUtilities,
alternativeMenuFactory, notificationFactory, ErrorFactory, commonFactory) {

	$scope.employeeTypesOption = employeeTypesOption;
	$scope.altMenu = altMenu;

	/*
	** Check employee type that belong to alternative menu.
	*/
	var checkEmployeeTypeBelongToAltMenu = function (employeeTypeId, employeeTypes) {
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
			employeeType.ticked = checkEmployeeTypeBelongToAltMenu(employeeType.EmployeeTypeId, $scope.altMenu.EmployeeTypes);
		});
	};

	checkEmployeeTypeOptions();

	/*
	** Edit alternative menu.
	*/
	$scope.save = function (altMenu) {
		$scope.disabledSubmitButton = true;

		var updatedAltMenu = {
			alternativeMenuId: altMenu.AlternativeMenuId,
			alternativeMenuCode: altMenu.AlternativeMenuCode,
			alternativeMenuVnmName: altMenu.AlternativeMenuVnmName,
			alternativeMenuEngName: altMenu.AlternativeMenuEngName,
			isActive: altMenu.IsActive,
			employeeTypeId: foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption)
		};

		if (alternativeMenuUtilities.validateAlternativeMenu(updatedAltMenu)) {
			altMenu.EmployeeTypes = commonFactory.updateEmployeeTypes(updatedAltMenu.employeeTypeId, $scope.employeeTypesOption);

			alternativeMenuFactory.updateAlternativeMenu(updatedAltMenu).success(function (response) {
				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
					$modalInstance.close(altMenu);
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