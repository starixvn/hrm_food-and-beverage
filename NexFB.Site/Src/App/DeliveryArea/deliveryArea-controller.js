'use strict';
/*global deliveryAreaModule*/

deliveryAreaModule.controller('DeliveryAreaController',
['$scope', 'deliveryAreaFactory', 'ErrorFactory', 'commonFactory', 'baseUrl', '$modal', 'notificationFactory',
function ($scope, deliveryAreaFactory, ErrorFactory, commonFactory, baseUrl, $modal, notificationFactory) {

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
		commonFactory.tablePagination($scope, $scope.deliveryAreaList, $scope.entrySelected);
	};

	$scope.entrySelected = 20;
	commonFactory.numberEntryList($scope);
	$scope.originalPage = 0;

	/*
	** Get user permission on Delivery Area Management page.
	*/
	deliveryAreaFactory.getUserPermissionOnDeliveryArea().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all delivery area.
	*/
	var getAllDeliveryArea = function (locationId, isActive, query) {
		deliveryAreaFactory.getAllDeliveryArea(locationId, isActive, query).success(function (deliveryAreaList) {
			$scope.deliveryAreaList = deliveryAreaList;
			commonFactory.tablePagination($scope, $scope.deliveryAreaList, $scope.entrySelected);
			$scope.currentPage = $scope.originalPage;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	getAllDeliveryArea(null, null, null);

	/*
	** Create delivery area.
	*/
	$scope.createDeliveryArea = function () {
		$scope.originalPage = $scope.currentPage;

		$scope.modalInstance = $modal.open({
			templateUrl: baseUrl + 'src/app/DeliveryArea/Templates/create.html',
			controller: "CreateDeliveryAreaController",
			backdrop: 'static',
			windowClass: "modal-confirm-center delivery-area"
		});

		$scope.modalInstance.result.then(function () {
		})['finally'](function () {
			getAllDeliveryArea(null, null, null);
			$scope.modalInstance = '';
		});
	};

	/*
	** Update delivery area.
	*/
	$scope.editDeliveryArea = function (deliveryArea) {
		$scope.originalPage = $scope.currentPage;

		$scope.modalInstance = $modal.open({
			templateUrl: baseUrl + 'src/app/DeliveryArea/Templates/edit.html',
			controller: "EditDeliveryAreaController",
			backdrop: 'static',
			resolve: {
				deliveryArea: function () {
					return deliveryArea;
				}
			},
			windowClass: "modal-confirm-center delivery-area"
		});

		$scope.modalInstance.result.then(function () {
		})['finally'](function () {
			getAllDeliveryArea(null, null, null);
			$scope.modalInstance = '';
		});
	};

	/*
	** Activate or deactivate delivery area.
	*/
	$scope.confirmActive = function (deliveryArea) {
		var action = !deliveryArea.IsActive ? "activate" : "deactivate";
		deliveryArea.IsActive = !deliveryArea.IsActive;

		$scope.modalInstance = $modal.open({
			templateUrl: baseUrl + 'src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			backdrop: 'static',
			resolve: {
				name: function () {
					return deliveryArea.NameEN;
				},
				action: function () {
					return action;
				}
			},
			windowClass: "modal-confirm-center food-item-modal delivery-area"
		});

		var keyMessage = '';

		$scope.modalInstance.result.then(function () {
			deliveryAreaFactory.activateDeliveryArea(deliveryArea).success(function (response) {
				if (response.IsSuccessful) {
					keyMessage = action === "activate" ? "ActivationSuccess" : "DeactivationSuccess";
					notificationFactory.success(keyMessage);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					deliveryArea.IsActive = !deliveryArea.IsActive;
					notificationFactory.error(keyMessage);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					deliveryArea.IsActive = !deliveryArea.IsActive;
					notificationFactory.error(keyMessage);
				}
			});
		}, function () {
			deliveryArea.IsActive = !deliveryArea.IsActive;
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};

	var updatePaging = function (deliveryArea, currentPage) {
		var index = $scope.deliveryAreaList.indexOf(deliveryArea);
		$scope.deliveryAreaList.splice(index, 1);

		commonFactory.tablePagination($scope, $scope.deliveryAreaList, $scope.entrySelected);
		var numberPage = $scope.pagedItems.length;

		if (currentPage <= numberPage - 1) {
			$scope.currentPage = currentPage;
		}
		else {
			$scope.currentPage = currentPage - 1;
		}
	};

	$scope.deleteDeliveryArea = function (deliveryArea) {
		$scope.modalInstance = $modal.open({
			templateUrl: baseUrl + 'src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			backdrop: 'static',
			resolve: {
				action: function () {
					return 'delete';
				},
				name: function () {
					return deliveryArea.NameEN;
				}
			},
			windowClass: "modal-confirm-center food-item-modal delivery-area"
		});

		$scope.modalInstance.result.then(function () {
			var currentPage = $scope.currentPage;

			deliveryAreaFactory.deleteDeliveryArea(deliveryArea.LocationId).success(function (response) {
				if (response.IsSuccessful) {
					updatePaging(deliveryArea, currentPage);
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

deliveryAreaModule.controller('CreateDeliveryAreaController',
['$scope', '$modalInstance', 'deliveryAreaFactory', 'ErrorFactory', 'notificationFactory', 'deliveryAreaUtilities',
function ($scope, $modalInstance, deliveryAreaFactory, ErrorFactory, notificationFactory, deliveryAreaUtilities) {

	$scope.deliveryArea = {
		NameVN: '',
		NameEN: '',
		FloorName: '',
		isActive: true
	};

	/*
	** Add new delivery area.
	*/
	$scope.save = function (deliveryArea) {
		$scope.disabledSubmitButton = true;

		if (deliveryAreaUtilities.validateDeliveryArea(deliveryArea)) {
			deliveryAreaFactory.insertDeliveryArea(deliveryArea).success(function (response) {
				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
					$modalInstance.close();
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

deliveryAreaModule.controller('EditDeliveryAreaController',
['$scope', 'deliveryArea', '$modalInstance', 'deliveryAreaFactory', 'ErrorFactory', 'notificationFactory', 'deliveryAreaUtilities',
function ($scope, deliveryArea, $modalInstance, deliveryAreaFactory, ErrorFactory, notificationFactory, deliveryAreaUtilities) {

	$scope.deliveryArea = deliveryArea;

	/*
	** Add new delivery area.
	*/
	$scope.save = function (deliveryArea) {
		$scope.disabledSubmitButton = true;

		if (deliveryAreaUtilities.validateDeliveryArea(deliveryArea)) {
			deliveryAreaFactory.updateDeliveryArea(deliveryArea).success(function (response) {
				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
					$modalInstance.close();
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