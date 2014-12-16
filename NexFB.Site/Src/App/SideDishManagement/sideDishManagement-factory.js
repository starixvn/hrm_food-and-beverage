'use strict';
/*global sideDishManagementModule*/

sideDishManagementModule.factory('sideDishManagementFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		getAllSideDish: function (foodId, isActive) {
			return $http.get(baseUrl + 'api/SideDishManagement/GetAllSideDish',
				{ params: { foodId: foodId, isActive: isActive} });
		},
		getAllFoodUnit: function (fooUnitId) {
			return $http.get(baseUrl + 'api/SideDishManagement/GetAllFoodUnit', { params: { foodUnitId: fooUnitId} });
		},
		insertSideDish: function (sideDish) {
			return $http.post(baseUrl + 'api/SideDishManagement/InsertSideDish', sideDish);
		},
		updateSideDish: function (sideDish) {
			return $http.put(baseUrl + 'api/SideDishManagement/UpdateSideDish', sideDish);
		},
		activateSideDish: function (sideDish) {
			return $http.put(baseUrl + 'api/SideDishManagement/ActivateSideDish', sideDish);
		},
		deleteSideDish: function (foodId) {
			return $http['delete'](baseUrl + 'api/SideDishManagement/DeleteSideDish', { params: { foodId: foodId} });
		},
		getPermissionOnSideDish: function () {
			return $http.get(baseUrl + 'api/SideDishManagement/GetUserPermissionOnSideDish');
		},
		updateKitchenOnly: function (sideDish) {
			return $http.put(baseUrl + 'api/SideDishManagement/UpdateKitchenOnlyByFoodId', sideDish);
		}
	};
} ]);

sideDishManagementModule.factory('sideDishUtilities', ['notificationFactory', function (notificationFactory) {
	var utilities = {};

	utilities.validateSideDish = function (sideDish) {
		var valid = true;

		if (!sideDish) {
			return false;
		}
		else {
			if (!sideDish.employeeTypeId) {
				valid = false;
				notificationFactory.error('FoodMenuEmployeeTypeRequire');
			}
			if (sideDish.price === '' || sideDish.price === undefined) {
				valid = false;
				notificationFactory.error('PriceRequire');
			}
			if (!sideDish.unitId) {
				valid = false;
				notificationFactory.error('UnitRequire');
			}
			if (!sideDish.foodEngName) {
				valid = false;
				notificationFactory.error('FoodEngNameRequire');
			}
			if (!sideDish.foodVnmName) {
				valid = false;
				notificationFactory.error('FoodVnmNameRequire');
			}
			if (!sideDish.foodCode) {
				valid = false;
				notificationFactory.error('CodeRequire');
			}
		}

		return valid;
	};

	return utilities;
} ]);