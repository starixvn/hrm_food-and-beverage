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
		link: function (scope) {
			scope.$on('resetFoodItem', function () {
				clearInputData(scope.food);

				var menuChosenSelect = $("[name='menuSelect']");
				$(menuChosenSelect).val('').trigger("chosen:updated");
			});
		}
	};
});