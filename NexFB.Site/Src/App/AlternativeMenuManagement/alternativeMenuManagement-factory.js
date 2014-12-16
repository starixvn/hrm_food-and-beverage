'use strict';
/*global alternativeMenuModule*/

alternativeMenuModule.factory('alternativeMenuFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		getAllAlternativeMenu: function (alternativeMenuId, isActive) {
			return $http.get(baseUrl + 'api/AlternativeMenu/GetAllAlternativeMenu',
				{ params: { alternativeMenu: alternativeMenuId, isActive: isActive} });
		},
		insertAlternativeMenu: function (alternativeMenu) {
			return $http.post(baseUrl + 'api/AlternativeMenu/InsertAlternativeMenu', alternativeMenu);
		},
		updateAlternativeMenu: function (alternativeMenu) {
			return $http.put(baseUrl + 'api/AlternativeMenu/UpdateAlternativeMenu', alternativeMenu);
		},
		activateAlternativeMenu: function (alternativeMenu) {
			return $http.put(baseUrl + 'api/AlternativeMenu/ActivateAlternativeMenu', alternativeMenu);
		},
		deleteAlternativeMenu: function (alternativeMenuId) {
			return $http['delete'](baseUrl + 'api/AlternativeMenu/DeleteAlternativeMenu',
				{ params: { alternativeMenuId: alternativeMenuId} });
		},
		getPermissionOnAlternativeMenu: function () {
			return $http.get(baseUrl+ 'api/AlternativeMenu/GetUserPermissionOnAlternativeMenu');
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