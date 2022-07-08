'use strict';
/*global shiftMealMappingModule*/

shiftMealMappingModule.factory('shiftMealMappingFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
    return {
        getAllShiftMeal: function (departmentId, teamCode, shiftCode) {
            return $http.get(baseUrl + 'api/ShiftMealMapping/GetAllShiftMeal',
                { params: { departmentId: departmentId, shiftCode: shiftCode, teamCode: teamCode } });
        },
        updateShiftMeal: function (shift) {
            return $http.post(baseUrl + 'api/ShiftMealMapping/UpdateShiftMeal', shift);
        },
        getAllDepartment: function () {
            return $http.get(baseUrl + 'api/Common/GetAllDepartment');
        },
        getPermissionOnShiftMealMapping: function () {
            return $http.get(baseUrl + 'api/ShiftMealMapping/GetUserPermissionOnShiftMealMapping');
        }
    };
}]);

shiftMealMappingModule.factory('shiftMealMappingUtilities',
    ['notificationFactory', function (notificationFactory) {
        var utilities = {};

        var getNumberRegisterMeal = function (shift) {
            var count = 0;

            if (shift.MealPreviousSupper) {
                count++;
            }
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
            if (shift.MealNextDinner) {
                count++;
            }

            return count;
        };

        var checkMealContinuous = function (shift) {
            var maxNumberMealContinuous = 0,
                numberMealContinuous = 0,
                numberMeals = 0,
                continuous = true;

            if (shift.MealPreviousSupper) {
                numberMealContinuous++;
                numberMeals++;
            }
            else {
                maxNumberMealContinuous = maxNumberMealContinuous > numberMealContinuous ? maxNumberMealContinuous : numberMealContinuous;
                numberMealContinuous = 0;
            }

            if (shift.MealBreakfast) {
                numberMealContinuous++;
                numberMeals++;
            }
            else {
                maxNumberMealContinuous = maxNumberMealContinuous > numberMealContinuous ? maxNumberMealContinuous : numberMealContinuous;
                numberMealContinuous = 0;
            }

            if (shift.MealLunch) {
                numberMealContinuous++;
                numberMeals++;
            }
            else {
                maxNumberMealContinuous = maxNumberMealContinuous > numberMealContinuous ? maxNumberMealContinuous : numberMealContinuous;
                numberMealContinuous = 0;
            }

            if (shift.MealDinner) {
                numberMealContinuous++;
                numberMeals++;
            }
            else {
                maxNumberMealContinuous = maxNumberMealContinuous > numberMealContinuous ? maxNumberMealContinuous : numberMealContinuous;
                numberMealContinuous = 0;
            }

            if (shift.MealSupper) {
                numberMealContinuous++;
                numberMeals++;
            }
            else {
                maxNumberMealContinuous = maxNumberMealContinuous > numberMealContinuous ? maxNumberMealContinuous : numberMealContinuous;
                numberMealContinuous = 0;
            }

            if (shift.MealNextBreakfast) {
                numberMealContinuous++;
                numberMeals++;
            }
            else {
                maxNumberMealContinuous = maxNumberMealContinuous > numberMealContinuous ? maxNumberMealContinuous : numberMealContinuous;
                numberMealContinuous = 0;
            }

            if (shift.MealNextLunch) {
                numberMealContinuous++;
                numberMeals++;
            }
            else {
                maxNumberMealContinuous = maxNumberMealContinuous > numberMealContinuous ? maxNumberMealContinuous : numberMealContinuous;
                numberMealContinuous = 0;
            }

            if (shift.MealNextDinner) {
                numberMealContinuous++;
                numberMeals++;
            }
            else {
                maxNumberMealContinuous = maxNumberMealContinuous > numberMealContinuous ? maxNumberMealContinuous : numberMealContinuous;
                numberMealContinuous = 0;
            }

            maxNumberMealContinuous = maxNumberMealContinuous > numberMealContinuous ? maxNumberMealContinuous : numberMealContinuous;

            if (maxNumberMealContinuous < numberMeals) {
                continuous = false;
            }

            return continuous;
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

                var checkContinuous = checkMealContinuous(shift);

                if (!checkContinuous) {
                    notificationFactory.error('NotContinuousMealInShift');
                    valid = false;
                }
            }

            return valid;
        };

        return utilities;
    }]);