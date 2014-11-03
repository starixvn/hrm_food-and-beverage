'use strict';
/*global foodMenuManagementModule*/

foodMenuManagementModule.directive('resetFoodMenu', function () {
	var clearMealsData = function (mealsOption) {
		angular.forEach(mealsOption, function (value) {
			value.ticked = false;
		});
	};

	var clearEmployeeTypesData = function (employeeTypesOption) {
		angular.forEach(employeeTypesOption, function (value) {
			value.ticked = false;
		});
	};

	var clearInputData = function (foodMenu) {
		foodMenu.menuVnmName = '';
		foodMenu.menuEngName = '';
		foodMenu.isVegetarian = false;
		foodMenu.isActive = true;
		foodMenu.mealId = '';
		foodMenu.employeeTypeId = '';
		foodMenu.setMenuId = '';
		foodMenu.pricePerMeal = '';
		foodMenu.pricePerDay = '';
		foodMenu.rawSetMenuIds = [];
	};

	return {
		link: function (scope) {
			scope.$on('resetFoodMenu', function () {
				clearInputData(scope.foodMenu);
				clearMealsData(scope.mealsOption);
				clearEmployeeTypesData(scope.employeeTypesOption);

				var menuChosenSelect = $("[name='setMenuSelect']");
				$(menuChosenSelect).val('').trigger("chosen:updated");
			});
		}
	};
});