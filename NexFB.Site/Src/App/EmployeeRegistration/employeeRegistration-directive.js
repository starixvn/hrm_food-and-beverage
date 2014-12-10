'use strict';
/*global mealRegistrationModule*/

mealRegistrationModule.directive('onlyOneCheck', function () {
	return {
		restrict: 'A',
		link: function (scope, iElement) {
			iElement.on("click", function () {
				var currentElementId = $(this).prop('id'),
					currentChecked = $(this).prop('checked'),
					checkboxElements = $(this).parent().parent().find('input');

				for (var i = 0; i < checkboxElements.length; i++) {
					var element = checkboxElements[i];

					if ($(element).prop('id') !== currentElementId) {
						$(element).prop('checked', !currentChecked);
					}
				}
			});
		}
	};
});

mealRegistrationModule.directive('getMealRegistration', function () {
	return {
		link: function (scope) {
			scope.$on('getMealRegistration', function () {
				var meals = scope.meals,
					mealsTemplate = '<ROOT>',
					registrationMealTable = $("[name='registrationMeal']"),
					mealRegister = $(registrationMealTable).find('tbody tr');

				for (var i = 0; i < mealRegister.length; i++) {
					var rowMeal = mealRegister[i],
						checkedElement = $(rowMeal).find('input:checked'),
						template = '',
						mealId = '';

					if (checkedElement != null && checkedElement.prop('id')) {
						mealId = checkedElement.val();

						var checkedElementId = checkedElement.prop('id'),
							isVegetarian = checkedElementId.replace('meal' + mealId, '') === '_non' ? 0 : 1;

						template = '<Meal MealID="' + mealId + '" IsVeg="' + isVegetarian + '" />';
						meals[i].IsVegetarian = isVegetarian;
					}
					else {
						var firstMealElement = $(rowMeal).find('input:first');
						mealId = firstMealElement.val();
						template = '<Meal MealID="' + mealId + '"/>';
						meals[i].IsVegetarian = null;
					}

					mealsTemplate += template;
				}

				mealsTemplate += '</ROOT>';
				scope.save.call(this, meals, mealsTemplate);
			});
		}
	};
});

mealRegistrationModule.directive('checkMealTimesheet', function () {
	var getCurrentTimesheet = function (mealTimesheet, mealId) {
		var currentTimesheet = '';

		angular.forEach(mealTimesheet, function (timesheetItem) {
			if (timesheetItem.MealId === mealId) {
				currentTimesheet = timesheetItem;
			}
		});

		return currentTimesheet;
	};

	var checkRightMeal = function (currentTimesheet, workDate) {
		if (workDate) {
			var toDay = new Date(),
				currentWorkDate = new Date(workDate);

			if (toDay.getMonth() === currentWorkDate.getMonth() && toDay.getDate() === currentWorkDate.getDate() &&
					toDay.getFullYear() === currentWorkDate.getFullYear()) {
				var currentHour = toDay.getHours(),
					currentMinute = toDay.getMinutes(),
					registerHour = new Date(currentTimesheet.RegisterTime + 'Z').getUTCHours(),
					registerMinute = new Date(currentTimesheet.RegisterTime + 'Z').getUTCMinutes();

				if (currentHour > registerHour) {
					return false;
				}
				else if (currentHour === registerHour && currentMinute > registerMinute) {
					return false;
				}
			}
		}

		return true;
	};

	return {
		restrict: 'A',
		link: function (scope) {
			if (scope.mode === 'month') {
				var mealTimesheet = scope.mealTimesheet,
				meal = scope.meal,
				workDate = scope.workDate,
				currentTimesheet = getCurrentTimesheet(mealTimesheet, meal.MealId),
				rightMeal = checkRightMeal(currentTimesheet, workDate);

				scope.rightMeal[meal.MealId] = rightMeal;
			}
		}
	};
});

mealRegistrationModule.directive('getClassByName', function () {
	return {
		restrict: 'A',
		link: function (scope) {
			var mealName = scope.meal.ShortName,
				mealClass = '';

			if (mealName === 'B') {
				mealClass = 'lbl-breakfast';
			}
			else if (mealName === 'L') {
				mealClass = 'lbl-lunch';
			}
			else if (mealName === 'D') {
				mealClass = 'lbl-dinner';
			}
			else if (mealName === 'S') {
				mealClass = 'lbl-supper';
			}

			scope.mealClass = mealClass;
		}
	};
});

mealRegistrationModule.directive('validateMealRegistration', function () {
	return {
		restrict: 'A',
		link: function (scope, iElement) {
			iElement.on('click', function () {
				var registrationMealTable = $("[name='registrationMeal']"),
					checkedElements = $(registrationMealTable).find('tbody input:checked'),
					validChecked = true,
					isTypeMeal = '',
					workDate = scope.workDate ? new Date(scope.workDate) : '',
					validVegetarianCurrentDate = true;

				for (var i = 0; i < checkedElements.length; i++) {
					var checkedElement = checkedElements[i];

					if ($(checkedElement).prop('id')) {
						var mealId = $(checkedElement).val(),
						checkedElementId = $(checkedElement).prop('id'),
						isVegetarian = checkedElementId.replace('meal' + mealId, '') !== '_non';

						// Check user cannot register non-veggie and veggie.
						if (isTypeMeal === '') {
							isTypeMeal = isVegetarian;
						}
						else if (isTypeMeal !== isVegetarian) {
							validChecked = false;
							i = checkedElements.length;
						}

						// In daily registration form of the current date, Vegetarian options are disabled
						if (workDate && isVegetarian) {
							var todayNoTime = new Date(),
							workDateNoTime = new Date(scope.workDate),
							limitDateNoTime = new Date();

							todayNoTime.setHours(0, 0, 0, 0);
							workDateNoTime.setHours(0, 0, 0, 0);
							limitDateNoTime.setDate(todayNoTime.getDate() + scope.registerSetting.Order);
							limitDateNoTime.setHours(0, 0, 0, 0);

							if (+workDateNoTime >= +todayNoTime && workDateNoTime < limitDateNoTime) {
								validVegetarianCurrentDate = false;
							}
						}
					}
				}

				scope.validateMealRegistration.call(this, validChecked, validVegetarianCurrentDate);
			});
		}
	};
});