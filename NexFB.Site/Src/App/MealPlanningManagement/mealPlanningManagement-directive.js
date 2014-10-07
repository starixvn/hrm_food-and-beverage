'use strict';
/*global mealPlanningManagementModule*/

// multi-select use Chosen plugin
mealPlanningManagementModule.directive('mealPlanningMultiSelect', function () {
	return {
		link: function (scope, iElement) {
			scope.$watchCollection('groupOptions', function () {
				iElement.trigger("chosen:updated");
			});

			iElement.chosen({ width: "95%" }).change(function (event, params) {
				var index = '';

				if (params.selected) {
					index = parseInt(params.selected);
					var addedFood = scope.groupOptions[index];
					scope.foodItems.push(addedFood);
				}
				else if (params.deselected) {
					index = parseInt(params.deselected);

					var removedFood = scope.groupOptions[index],
						indexInFoodItems = scope.foodItems.indexOf(removedFood);

					scope.foodItems.splice(indexInFoodItems, 1);
				}

				scope.$apply();
			});
		}
	};
});

mealPlanningManagementModule.directive('resetMealPlanning', function () {
	return {
		link: function (scope) {
			scope.$on('resetMealPlanning', function () {
				scope.mealPlanning.onDate = '';
				scope.mealPlanning.menuId = '';
				scope.mealPlanning.isActive = false;

				var menuChosenSelect = $("[name='foodSelect']");
				$(menuChosenSelect).val('').trigger("chosen:updated");
			});
		}
	};
});