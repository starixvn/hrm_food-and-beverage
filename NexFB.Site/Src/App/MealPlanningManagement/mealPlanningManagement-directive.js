'use strict';
/*global mealPlanningManagementModule*/

// multi-select use Chosen plugin
mealPlanningManagementModule.directive('mealPlanningMultiSelect', function () {

	var getIndexOfRemoveFood = function (foodItems, removedFood) {
		var removedIndex = -1;

		angular.forEach(foodItems, function (foodItem, index) {
			if (foodItem.FoodId === removedFood.FoodId) {
				removedIndex = index;
			}
		});

		return removedIndex;
	};

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
						indexInFoodItems = getIndexOfRemoveFood(scope.foodItems, removedFood);

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
				scope.mealPlanning.isActive = true;
				scope.mealPlanning.date = new Date();

				var menuChosenSelect = $("[name='foodSelect']");
				$(menuChosenSelect).val('').trigger("chosen:updated");
			});
		}
	};
});

mealPlanningManagementModule.directive('ieSelectFix', ['$document',
function ($document) {
	return {
		restrict: 'A',
		require: 'ngModel',
		link: function (scope, element, attributes) {
			var isIE = $document[0] && $document[0].attachEvent, control;

			if (!isIE) {
				return;
			}

			control = element[0];
			// to fix IE8 and IE9 issue with parent and detail controller, we need to depend on the parent controller
			scope.$watch(attributes.ieSelectFix, function () {
				// this will add and remove the options to trigger the rendering in IE8 and IE9
				var option = document.createElement("option");
				control.add(option, null);
				control.remove(control.options.length - 1);
			});
		}
	};
}
]);