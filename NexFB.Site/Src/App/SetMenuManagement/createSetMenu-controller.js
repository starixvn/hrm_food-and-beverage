'use strict';
/*global setMenuManagementModule*/

setMenuManagementModule.controller('CreateSetMenuController',
['$scope', '$modalInstance', 'menuItems', 'foodItems',
'setMenuFactory', 'notificationFactory', 'ErrorFactory',
function ($scope, $modalInstance, menuItems, foodItems,
setMenuFactory, notificationFactory, ErrorFactory) {
	/*
	** Declare variables.
	*/
	$scope.menuItems = menuItems;
	$scope.foodItems = foodItems;

	$scope.clearData = function () {
		$scope.setMenu = {
			SetMenuCode: '',
			SetMenuVnmName: '',
			IsActive: true,
			FoodItems: [],
			Menus: [],
			RawFoodIds: [],
			RawMenuIds: []
		};
	};

	$scope.clearData();

	/*
	** Add new food menu.
	*/

	$scope.validateNewSetMenu = function (setMenu) {
		var isValid = true;

		if (setMenu.SetMenuVnmName.length < 1) {
			isValid = false;
			notificationFactory.error("SetMenuNameRequire");
		}

		if (setMenu.RawFoodIds.length < 1) {
			isValid = false;
			notificationFactory.error("FoodItemRequire");
		}

		return isValid;
	};

	$scope.addSetMenu = function (setMenu, isContinue) {
		$scope.disabledSubmitButton = true;

		if ($scope.validateNewSetMenu(setMenu)) {
			setMenuFactory.insertSetMenu(setMenu).success(function (response) {
				$scope.submitted = false;
				$scope.disabledSubmitButton = false;

				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
					if (!isContinue) {
						$modalInstance.close();
					}
					else {
						$scope.$broadcast("resetSetMenu");
					}
				}
				else if (response.Code === 100) {
					notificationFactory.error("DuplicateSetMenuMessage");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		}

		$scope.disabledSubmitButton = false;
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);