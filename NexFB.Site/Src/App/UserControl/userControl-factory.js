'use strict';
/*global userControlModule*/

userControlModule.factory('userControlFactory', ['$http', function ($http) {
	return {
		getFunctionByRole: function (roleId) {
			return $http.get('/api/UserControl/GetFunctionByRole', { params: { roleId: roleId} });
		},
		getAllSystemRole: function () {
			return $http.get('/api/UserControl/GetAllSystemRole');
		},
		assignPermissionToRole: function (roleFunction) {
			return $http.post('/api/UserControl/AssignPermissionToRole', roleFunction);
		}
	};
} ]);
