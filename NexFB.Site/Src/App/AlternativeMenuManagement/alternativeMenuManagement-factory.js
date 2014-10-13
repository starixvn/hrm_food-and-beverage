'use strict';
/*global alternativeMenuModule*/

alternativeMenuModule.factory('alternativeMenuFactory', ['$http', function ($http) {
	return {
		getAllAlternativeMenu: function (alternativeMenuId, isActive) {
			return $http.get('/api/AlternativeMenu/GetAllAlternativeMenu',
				{ params: { alternativeMenu: alternativeMenuId, isActive: isActive} });
		},
		insertAlternativeMenu: function (alternativeMenu) {
			return $http.post('/api/AlternativeMenu/InsertAlternativeMenu', alternativeMenu);
		},
		updateAlternativeMenu: function (alternativeMenu) {
			return $http.put('/api/AlternativeMenu/UpdateAlternativeMenu', alternativeMenu);
		},
		activateAlternativeMenu: function (alternativeMenu) {
			return $http.put('/api/AlternativeMenu/ActivateAlternativeMenu', alternativeMenu);
		},
		deleteAlternativeMenu: function (alternativeMenuId) {
			return $http['delete']('/api/AlternativeMenu/DeleteAlternativeMenu',
				{ params: { alternativeMenuId: alternativeMenuId} });
		},
		getPermissionOnAlternativeMenu: function () {
			return $http.get('/api/AlternativeMenu/GetUserPermissionOnAlternativeMenu');
		}
	};
} ]);

alternativeMenuModule.factory('alternativeMenuUtilities', ['notificationFactory', function (notificationFactory) {
	var utilities = {};

	utilities.validateAlternativeMenu = function (altMenu) {
		var valid = true;

		if (!altMenu) {
			return false;
		}
		else {
			if (!altMenu.employeeTypeId) {
				valid = false;
				notificationFactory.error('FoodMenuEmployeeTypeRequire');
			}
			if (!altMenu.alternativeMenuEngName) {
				valid = false;
				notificationFactory.error('MenuNameEngRequire');
			}
			if (!altMenu.alternativeMenuVnmName) {
				valid = false;
				notificationFactory.error('MenuNameVietRequire');
			}
			if (!altMenu.alternativeMenuCode) {
				valid = false;
				notificationFactory.error('MenuCodeRequire');
			}
		}

		return valid;
	};

	return utilities;
} ]);