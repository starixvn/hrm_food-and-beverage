'use strict';
/*global sideDishManagementModule*/

sideDishManagementModule.factory('sideDishManagementFactory', ['$http', function ($http) {
	return {
		getAllSideDish: function (foodId, isActive) {
			return $http.get('/api/SideDishManagement/GetAllSideDish',
				{ params: { foodId: foodId, isActive: isActive} });
		},
		getAllFoodUnit: function (fooUnitId) {
			return $http.get('/api/SideDishManagement/GetAllFoodUnit', { params: { foodUnitId: fooUnitId} });
		},
		insertSideDish: function (sideDish) {
			return $http.post('/api/SideDishManagement/InsertSideDish', sideDish);
		},
		updateSideDish: function (sideDish) {
			return $http.put('/api/SideDishManagement/UpdateSideDish', sideDish);
		},
		activateSideDish: function (sideDish) {
			return $http.put('/api/SideDishManagement/ActivateSideDish', sideDish);
		},
		deleteSideDish: function (foodId) {
			return $http['delete']('/api/SideDishManagement/DeleteSideDish', { params: { foodId: foodId} });
		},
		getPermissionOnSideDish: function () {
			return $http.get('/api/SideDishManagement/GetUserPermissionOnSideDish');
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
			if (!sideDish.foodVnmName) {
				valid = false;
				notificationFactory.error('ItemRequire');
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