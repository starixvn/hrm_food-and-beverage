'use strict';
/*global employeeModule*/

employeeModule.factory('employeeFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		getSpecialUsers: function (departmentId, employeeTypeId, isActive) {
			return $http.get(baseUrl + 'api/Employee/GetSpecialUsers', { params: { departmentId: departmentId, employeeTypeId: employeeTypeId, isActive: isActive} });
		},
		getSpecialUserById: function (userId) {
			return $http.get(baseUrl + 'api/Employee/GetSpecialUserById',
				{ params: { userId: userId} });
		},
		insertSpecialUser: function (user) {
			return $http.post(baseUrl + 'api/Employee/InsertSpecialUser', user);
		},
		updateSpecialUser: function (user) {
			return $http.put(baseUrl + 'api/Employee/UpdateSpecialUser', user);
		},
		deleteSpecialUser: function (userId) {
			return $http['delete'](baseUrl + 'api/Employee/DeleteSpecialUser', { params: { userId: userId} });
		},
		updateStatusSpecialUser: function (user) {
			return $http.put(baseUrl + 'api/Employee/UpdateStatusSpecialUser', user);
		},
		getEmployeeType: function (departmentId) {
			return $http.get(baseUrl + 'api/Common/GetAllEmployeeType', 
				{ params: { departmentId: departmentId } });
		},
		getAllDepartment: function () {
			return $http.get(baseUrl + 'api/Common/GetAllDepartment');
		},
		getPermissonSpecialEmployee: function () {
			return $http.get(baseUrl + 'api/Employee/GetUserPermissionOnSpecialEmployee');
		}
	};
} ]);