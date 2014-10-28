'use strict';
/*global mealPlanningManagementModule*/

// multi-select use Chosen plugin
mealPlanningManagementModule.directive('mealPlanningMultiSelect', function () {
	return {
		link: function (scope, iElement) {
			scope.$watchCollection('foodOptions', function () {
				iElement.trigger("chosen:updated");
			});

			iElement.chosen({ width: "100%" }).change(function (event, params) {
				var index = '';

				if (params.selected) {
					index = parseInt(params.selected);
					var addedFood = scope.foodOptions[index];
					scope.foodItems.push(addedFood);
				}
				else if (params.deselected) {
					index = parseInt(params.deselected);

					var removedFood = scope.foodOptions[index],
						indexInFoodItems = scope.foodItems.indexOf(removedFood);

					scope.foodItems.splice(indexInFoodItems, 1);
				}

				scope.$apply();
			});
		}
	};
});

mealPlanningManagementModule.directive('foodAndSetmenu', function () {
	return {
		link: function (scope, iElement) {
			iElement.change(function () {
				scope.getFoodItemsBySetMenu(scope.mealPlanning.setMenuId);
				scope.$apply();
				var menuChosenSelect = $("[name='foodSelect']");
				$(menuChosenSelect).trigger("chosen:updated");
			});
		}
	};
});

mealPlanningManagementModule.directive('foodAndSetmenuEdit', function () {
	return {
		link: function (scope, iElement) {
			iElement.change(function () {
				scope.getFoodItemsBySetMenu(scope.mealPlanning.SetMenu.SetMenuId);
				scope.$apply();
				var menuChosenSelect = $("[name='foodSelect']");
				$(menuChosenSelect).trigger("chosen:updated");
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
				scope.mealPlanning.setMenuId = '';
				scope.mealPlanning.isActive = false;
				scope.mealPlanning.date = new Date();

				var menuChosenSelect = $("[name='foodSelect']");
				$(menuChosenSelect).val('').trigger("chosen:updated");
			});
		}
	};
});