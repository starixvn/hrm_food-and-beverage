'use strict';
/*global foodItemManagementModule*/

foodItemManagementModule.directive('resetFoodItem', function () {
	var clearInputData = function (food) {
		food.foodEngName = '';
		food.foodVnmName = '';
		food.isActive = false;
		food.setMenuIds = [];
		food.setMenuId = '';
	};

	return {
		link: function (scope) {
			scope.$on('resetFoodItem', function () {
				clearInputData(scope.food);

				var setMenuChosenSelect = $("[name='setMenuSelect']");
				$(setMenuChosenSelect).val('').trigger("chosen:updated");
			});
		}
	};
});