'use strict';
/*global mealRegistrationModule*/

mealRegistrationModule.directive('onlyOneCheck', function () {
	return {
		restrict: 'A',
		link: function (scope, iElement) {
			iElement.on("click", function () {
				var mealId = $(this).val(),
					currentElementId = $(this).prop('id'),
					currentChecked = $(this).prop('checked'),
					isVegetarian = currentElementId.replace('meal' + mealId, '') !== '_non',
					anotherElementId = isVegetarian ? ('#meal' + mealId + '_non') : ('#meal' + mealId);

				if (currentChecked) {
					$(anotherElementId).prop('checked', !currentChecked);
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

					if (checkedElement != null && checkedElement.length > 0) {
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

// In daily registration form of the current date, Vegetarian options are disabled
mealRegistrationModule.directive('disableCurrentDay', function () {
	return {
		restrict: 'A',
		link: function (scope) {
			var workDate = scope.workDate,
				meal = scope.meal;

			if (workDate) {
				var toDay = new Date(),
					currentWorkDate = new Date(workDate);

				if (toDay.getMonth() === currentWorkDate.getMonth() && toDay.getFullYear() === currentWorkDate.getFullYear()) {
					var toDate = toDay.getDate(),
						currentDate = currentWorkDate.getDate();

					if (currentDate >= toDate && currentDate < toDate + 7) {
						scope.currentDate[meal.MealId] = true;
					}
				}
			}
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
					workDate = scope.workDate !== '' ? new Date(scope.workDate) : '',
					toDay = new Date(),
					validVegetarianCurrentDate = true;

				for (var i = 0; i < checkedElements.length; i++) {
					var checkedElement = checkedElements[i],
						mealId = $(checkedElement).val(),
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
					if (workDate && toDay.getMonth() === workDate.getMonth() && toDay.getFullYear() === workDate.getFullYear() && isVegetarian) {
						var dateOfToDay = toDay.getDate(),
							dateOfWorkDate = workDate.getDate();

						if (dateOfWorkDate >= dateOfToDay && dateOfWorkDate < dateOfToDay + 7) {
							validVegetarianCurrentDate = false;
						}
					}
				}

				scope.validateMealRegistration.call(this, validChecked, validVegetarianCurrentDate);
			});
		}
	};
});