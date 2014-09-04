'use strict';
/*global mealRegistrationModule*/

mealRegistrationModule.factory('mealRegistrationFactory', ['$http', function ($http) {
	return {
		getRegisterMealByRangeDate: function (systemId, fromDate, toDate, mealId) {
			return $http.get('/api/MealRegistration/GetRegisterMealByRangeDate',
				{ params: { systemId: systemId, fromDate: fromDate, toDate: toDate, mealId: mealId } });
		}
	};
} ]);

mealRegistrationModule.factory('mealRegistrationCacheFactory', ['$cacheFactory', function ($cacheFactory) {
	return $cacheFactory('mealRegistrationCache');
} ]);

mealRegistrationModule.factory("mealRegistrationUtilities",
['$translate', 'mealRegistrationFactory', 'mealRegistrationCacheFactory',
function ($translate, mealRegistrationFactory, mealRegistrationCacheFactory) {
	var utility = {};

	utility.getTabHeading = function (scope) {
		var monthlyHeadingCache = mealRegistrationCacheFactory.get("monthlyHeading");

		if (monthlyHeadingCache) {
			scope.monthlyHeading = monthlyHeadingCache;
		}
		else {
			$translate("Monthly").then(function (message) {
				scope.monthlyHeading = message;
				mealRegistrationCacheFactory.put("monthlyHeading", message);
			});
		}

		var deafultHeadingCache = mealRegistrationCacheFactory.get("defaultHeading");

		if (deafultHeadingCache) {
			scope.defaultHeading = deafultHeadingCache;
		}
		else {
			$translate("Default").then(function (message) {
				scope.defaultHeading = message;
				mealRegistrationCacheFactory.put("defaultHeading", message);
			});
		}
	};

	return utility;
} ]);