'use strict';
/*global shiftMealMappingModule*/

shiftMealMappingModule.factory('shiftMealMappingFactory', ['$http', function ($http) {
	return {
		getAllShiftMeal: function (departmentId) {
			return $http.get('/api/ShiftMealMapping/GetAllShiftMeal',
				{ params: { departmentId: departmentId} });
		},
		updateShiftMeal: function (shift) {
			return $http.post('/api/ShiftMealMapping/UpdateShiftMeal', shift);
		},
		getAllDepartment: function () {
			return $http.get('/api/Common/GetAllDepartment');
		},
		getPermissionOnShiftMealMapping: function () {
			return $http.get('/api/ShiftMealMapping/GetUserPermissionOnShiftMealMapping');
		}
	};
} ]);

shiftMealMappingModule.factory('shiftMealMappingUtilities',
['notificationFactory', function (notificationFactory) {
	var utilities = {};

	var getNumberRegisterMeal = function (shift) {
		var count = 0;

		if (shift.MealBreakfast) {
			count++;
		}
		if (shift.MealLunch) {
			count++;
		}
		if (shift.MealDinner) {
			count++;
		}
		if (shift.MealSupper) {
			count++;
		}
		if (shift.MealNextBreakfast) {
			count++;
		}
		if (shift.MealNextLunch) {
			count++;
		}

		return count;
	};

	utilities.validateShiftMealMapping = function (shift) {
		var valid = true;

		if (!shift) {
			valid = false;
		}
		else {
			var numberRegisterMeal = getNumberRegisterMeal(shift);

			if (numberRegisterMeal === 0) {
				notificationFactory.error('UnderMealInShift');
				valid = false;
			}
			else if (numberRegisterMeal > 4) {
				notificationFactory.error('OverdueMealInShift');
				valid = false;
			}
		}

		return valid;
	};

	return utilities;
} ]);