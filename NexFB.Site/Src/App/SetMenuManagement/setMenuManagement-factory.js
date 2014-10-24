'use strict';
/*global setMenuManagementModule*/

setMenuManagementModule.factory('setMenuFactory', ['$http', function ($http) {
	return {
		getSetMenu: function (menuId, isActive) {
			return $http.get('/api/SetMenuManagement/GetSetMenu',
				{ params: { menuId: menuId, isActive: isActive} });
		},
		getSetMenuById: function (setMenuId) {
			return $http.get('/api/SetMenuManagement/GetSetMenuById',
				{ params: { setMenuId: setMenuId } });
		},
		insertSetMenu: function (setMenu) {
			return $http.post('/api/SetMenuManagement/InsertSetMenu', setMenu);
		},
		updateSetMenu: function (setMenu) {
			return $http.put('/api/SetMenuManagement/UpdateSetMenu', setMenu);
		},
		updateStatusSetMenu: function (setMenu) {
			return $http.put('/api/SetMenuManagement/UpdateStatusSetMenu', setMenu);
		},
		deleteSetMenu: function (setMenuId) {
			return $http['delete']('/api/SetMenuManagement/DeleteSetMenu', { params: { setMenuId: setMenuId} });
		},
		getUserPermissionOnSetMenu: function () {
			return $http.get('/api/SetMenuManagement/GetUserPermissionOnSetMenu');
		}
	};
} ]);