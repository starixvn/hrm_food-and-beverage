'use strict';
/*global foodItemManagementModule*/

foodItemManagementModule.controller('CreateFoodItemController',
['$scope', '$modalInstance', 'setMenuItems', 'notificationFactory', 'ErrorFactory', 'foodItemFactory',
'foodItemUtilities',
function ($scope, $modalInstance, setMenuItems, notificationFactory, ErrorFactory, foodItemFactory,
foodItemUtilities) {
	/*
	** Declare variables.
	*/

	$scope.food = {
		foodEngName: '',
		foodVnmName: '',
		isActive: true,
		setMenuIds: [],
		setMenuId: ''
	};

	var getSetMenus = function (setMenuItems) {
		var setMenus = [];

		angular.forEach(setMenuItems, function (setMenu) {
			if (setMenu.IsActive) {
				setMenus.push(setMenu);
			}
		});

		return setMenus;
	};

	$scope.setMenuItems = getSetMenus(setMenuItems);

	/*
	** Set scope of form to current scope.
	*/
	$scope.setFormScope = function (formScope) {
		$scope.formScope = formScope;
	};

	/*
	** Create food item.
	*/
	foodItemUtilities.showMessageValidateFoodItem($scope);

	$scope.create = function (food) {
		$scope.submitted = true;
		$scope.disabledSubmitButton = true;

		if (foodItemUtilities.validateFoodItem(food)) {
			food.setMenuId = foodItemUtilities.createSetMenuId(food.setMenuIds);
			$modalInstance.close(food);
		}
		else {
			$scope.disabledSubmitButton = false;
		}
	};

	/*
	** Create and continue.
	*/
	$scope.createAndContinue = function (food) {
		$scope.submitted = true;
		$scope.disabledSubmitButton = true;

		if (foodItemUtilities.validateFoodItem(food)) {
			food.menuGroupId = foodItemUtilities.createSetMenuId(food.setMenuIds);

			foodItemFactory.insertFoodItem(food).success(function (response) {
				$scope.submitted = false;
				$scope.disabledSubmitButton = false;

				if (response.IsSuccessful) {
					$scope.$broadcast("resetFoodItem");
					notificationFactory.success("DataChangeSuccessfully");
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