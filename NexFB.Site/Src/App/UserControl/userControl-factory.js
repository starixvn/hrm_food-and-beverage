'use strict';
/*global userControlModule*/

userControlModule.factory('userControlFactory', ['$http', function ($http) {
	return {
		getFunctionByRole: function (roleId) {
			return $http.get('/api/UserControl/GetFunctionByRole', { params: { roleId: roleId} });
		},
		getAllSystemRole: function (exceptSystemAdmin) {
			return $http.get('/api/UserControl/GetAllSystemRole', { params: { exceptSystemAdmin: exceptSystemAdmin } });
		},
		assignPermissionToRole: function (roleFunction) {
			return $http.post('/api/UserControl/AssignPermissionToRole', roleFunction);
		},
		getAllEmployeeRole: function (keyword, departmentId, teamId, roleId) {
			return $http.get('/api/UserControl/GetAllEmployeeRole',
					{ params: { keyword: keyword, departmentId: departmentId, teamId: teamId, roleId: roleId} });
		},
		getEmployeeRoleById: function (systemId) {
			return $http.get('/api/UserControl/GetEmployeeRoleById', { params: { systemId: systemId} });
		},
		getAllDepartment: function () {
			return $http.get('/api/Common/GetAllDepartment');
		},
		getAllTeamByCompany: function (departmentId) {
			return $http.get('/api/Common/GetAllTeamByCompany', { params: { departmentId: departmentId} });
		},
		assignRoleToUser: function (userRoles) {
			return $http.post('/api/UserControl/AssignRoleToUser', userRoles);
		},
		getManageRolePermission: function () {
			return $http.get('/api/UserControl/GetManageRolePermission');
		},
		getManageUserPermission: function () {
			return $http.get('/api/UserControl/GetManageUserPermission');
		}
	};
} ]);
