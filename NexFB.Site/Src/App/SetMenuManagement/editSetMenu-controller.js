'use strict';
/*global setMenuManagementModule*/

setMenuManagementModule.controller('EditSetMenuController',
['$scope', '$modalInstance', 'menuItems', 'foodItems', 'setMenu',
'setMenuFactory', 'notificationFactory', 'ErrorFactory',
function ($scope, $modalInstance, menuItems, foodItems, setMenu,
setMenuFactory, notificationFactory, ErrorFactory) {
	/*
	** Declare variables.
	*/
	$scope.menuItems = menuItems;
	$scope.foodItems = foodItems;
	$scope.setMenu = setMenu;

	$scope.validateEditSetMenu = function (setMenu) {
		var isValid = true;

		if (setMenu.RawFoodIds !== undefined && setMenu.RawFoodIds.length === 0) {
			isValid = false;
			notificationFactory.error("FoodItemRequire");
		}
		if (!setMenu.SetMenuVnmName) {
			isValid = false;
			notificationFactory.error("SetMenuNameRequire");
		}

		return isValid;
	};

	$scope.editSetMenu = function (setMenu) {
		$scope.disabledSubmitButton = true;

		if ($scope.validateEditSetMenu(setMenu)) {
			setMenuFactory.updateSetMenu(setMenu).success(function (response) {
				$scope.submitted = false;
				$scope.disabledSubmitButton = false;

				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
					$modalInstance.close();
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