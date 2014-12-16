'use strict';
/*global setMenuManagementModule*/

setMenuManagementModule.factory('setMenuFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		getSetMenu: function (menuId, isActive) {
			return $http.get(baseUrl + 'api/SetMenuManagement/GetSetMenu',
				{ params: { menuId: menuId, isActive: isActive} });
		},
		getSetMenuById: function (setMenuId) {
			return $http.get(baseUrl + 'api/SetMenuManagement/GetSetMenuById',
				{ params: { setMenuId: setMenuId } });
		},
		insertSetMenu: function (setMenu) {
			return $http.post(baseUrl + 'api/SetMenuManagement/InsertSetMenu', setMenu);
		},
		updateSetMenu: function (setMenu) {
			return $http.put(baseUrl + 'api/SetMenuManagement/UpdateSetMenu', setMenu);
		},
		updateStatusSetMenu: function (setMenu) {
			return $http.put(baseUrl + 'api/SetMenuManagement/UpdateStatusSetMenu', setMenu);
		},
		deleteSetMenu: function (setMenuId) {
			return $http['delete'](baseUrl + 'api/SetMenuManagement/DeleteSetMenu', { params: { setMenuId: setMenuId} });
		},
		getUserPermissionOnSetMenu: function () {
			return $http.get(baseUrl + 'api/SetMenuManagement/GetUserPermissionOnSetMenu');
		}
	};
} ]);