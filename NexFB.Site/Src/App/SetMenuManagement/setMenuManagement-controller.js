'use strict';
/*global setMenuManagementModule*/

setMenuManagementModule.controller('SetMenuManagementController',
['$scope', '$modal', 'setMenuFactory', 'notificationFactory', 'commonFactory', 'foodItemFactory',
 'foodMenuFactory', 'ErrorFactory',
function ($scope, $modal, setMenuFactory, notificationFactory, commonFactory, foodItemFactory,
foodMenuFactory, ErrorFactory) {
	/*
	** Declare variables.
	*/
	$scope.filter = {
		menuId: '',
		isActive: ''
	};

	$scope.originalPage = 0;

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
		commonFactory.tablePagination($scope, $scope.setMenus, $scope.entrySelected);
	};

	$scope.entrySelected = 10;
	commonFactory.numberEntryList($scope);

	/*
	** Get all menu.
	*/
	foodMenuFactory.getAllFoodMenu('', '', null).success(function (menus) {
		$scope.menuItems = menus;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all foods item.
	*/
	foodItemFactory.getAllFoodItem(null, true).success(function (foods) {
		$scope.foodItems = foods;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get user permission on food menu.
	*/
	setMenuFactory.getUserPermissionOnSetMenu().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all set menu.
	*/
	var getSetMenu = function (menuId, isActive) {
		setMenuFactory.getSetMenu(menuId, isActive).success(function (setMenus) {
			$scope.setMenus = setMenus;
			commonFactory.tablePagination($scope, $scope.setMenus, $scope.entrySelected);
			$scope.currentPage = $scope.originalPage;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	getSetMenu(null, null);

	/*
	** Query to filter data.
	*/
	var getMenuId = function (menuItems) {
		var menuId = '';

		angular.forEach(menuItems, function (value) {
			if (value.ticked) {
				menuId = menuId !== '' ? menuId + ',' + value.MenuId : value.MenuId;
			}
		});

		return menuId;
	};

	$scope.query = function (filter) {
		if (filter) {
			filter.menuId = getMenuId($scope.menuItems);
			getSetMenu(filter.menuId, filter.isActive);
		}
	};

	/*
	** Add set menu.
	*/
	$scope.addSetMenu = function () {
		$scope.originalPage = $scope.currentPage;

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/SetMenuManagement/Templates/create.html',
			controller: "CreateSetMenuController",
			backdrop: 'static',
			resolve: {
				menuItems: function () {
					return angular.copy($scope.menuItems);
				},
				foodItems: function () {
					return $scope.foodItems;
				}
			},
			windowClass: "modal-confirm-center set-menu-modal"
		});

		$scope.modalInstance.result.then(function () {
		})['finally'](function () {
			getSetMenu(null, null);
			$scope.modalInstance = '';
		});
	};

	/*
	** Edit set menu.
	*/
	$scope.editSetMenu = function (setMenu) {
		setMenuFactory.getSetMenuById(setMenu.SetMenuId).success(function (data) {
			$scope.modalInstance = $modal.open({
				templateUrl: '/src/app/SetMenuManagement/Templates/edit.html',
				controller: "EditSetMenuController",
				backdrop: 'static',
				resolve: {
					setMenu: function () {
						return data;
					},
					menuItems: function () {
						return angular.copy($scope.menuItems);
					},
					foodItems: function () {
						return $scope.foodItems;
					}
				},
				windowClass: "modal-confirm-center set-menu-modal"
			});

			$scope.modalInstance.result.then(function () {
			})['finally'](function () {
				getSetMenu(null, null);
				$scope.modalInstance = '';
			});
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	/*
	** Activate or deactivate set menu.
	*/
	$scope.confirmActive = function (setMenu) {
		setMenu.IsActive = !setMenu.IsActive;
		var action = setMenu.IsActive ? "activate" : "deactivate";

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			resolve: {
				name: function () {
					return setMenu.Name;
				},
				action: function () {
					return action;
				}
			},
			windowClass: "modal-confirm-center set-item-modal"
		});

		var keyMessage = '';

		$scope.modalInstance.result.then(function () {
			setMenuFactory.updateStatusSetMenu(setMenu).success(function (response) {
				if (response.IsSuccessful) {
					keyMessage = action === "activate" ? "ActivationSuccess" : "DeactivationSuccess";
					notificationFactory.success(keyMessage);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					setMenu.IsActive = !setMenu.IsActive;

					if (response.Code === 100) {
						notificationFactory.error("DuplicateSetMenuMessage");
					}
					else {
						notificationFactory.error(keyMessage);
					}
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		}, function () {
			setMenu.IsActive = !setMenu.IsActive;
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};

	/*
	** Delete set menu.
	*/
	var updatePaging = function (setMenu, currentPage) {
		var index = $scope.setMenus.indexOf(setMenu);
		$scope.setMenus.splice(index, 1);

		commonFactory.tablePagination($scope, $scope.setMenus, $scope.entrySelected);
		var numberPage = $scope.pagedItems.length;

		if (currentPage <= numberPage - 1) {
			$scope.currentPage = currentPage;
		}
		else {
			$scope.currentPage = currentPage - 1;
		}
	};

	$scope.deleteSetMenu = function (setMenu) {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			resolve: {
				action: function () {
					return 'delete';
				},
				name: function () {
					return setMenu.Name;
				}
			},
			windowClass: "modal-confirm-center set-item-modal"
		});

		$scope.modalInstance.result.then(function () {
			var currentPage = $scope.currentPage;

			setMenuFactory.deleteSetMenu(setMenu.SetMenuId).success(function (response) {
				if (response.IsSuccessful) {
					updatePaging(setMenu, currentPage);
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