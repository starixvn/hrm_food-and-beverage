'use strict';
/*global foodMenuManagementModule*/

foodMenuManagementModule.directive('resetFoodMenu', function () {
	var clearInputData = function (foodMenu) {
		foodMenu.menuVnmName = '';
		foodMenu.menuEngName =  '';
		foodMenu.isVegetarian = false;
		foodMenu.isActive = false;
		foodMenu.mealId = '';
		foodMenu.employeeTypeId = '';
		foodMenu.foodId ='';
		foodMenu.pricePerMeal = '';
		foodMenu.pricePerDay = '';
		foodMenu.rawFoodIds = '';
	};

	return {
		link: function (scope) {
			scope.$on('resetFoodMenu', function () {
				clearInputData(scope.foodMenu);

				var menuChosenSelect = $("[name='foodSelect']");
				$(menuChosenSelect).val('').trigger("chosen:updated");
			});
		}
	};
});