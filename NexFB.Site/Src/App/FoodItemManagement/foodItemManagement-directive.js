'use strict';
/*global foodItemManagementModule*/

foodItemManagementModule.directive('resetFoodItem', function () {
	var clearInputData = function (food) {
		food.foodEngName = '';
		food.foodVnmName = '';
		food.isActive = '';
		food.groupIds = '';
		food.menuGroupId = '';
	};

	return {
		link: function (scope, iElement) {
			iElement.on('click', function () {
				var newFood = angular.copy(scope.food);
				clearInputData(scope.food);

				var menuChosenSelect = $("[name='menuSelect']");
				$(menuChosenSelect).val('').trigger("chosen:updated");

				scope.createAndContinue.call(this, newFood);
			});
		}
	};
});