'use strict';
/*global employeeModule*/

employeeModule.factory('employeeFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		getSpecialUsers: function () {
			return $http.get(baseUrl + 'api/Employee/GetSpecialUsers');
		},
		getSpecialUserById: function (userId) {
			return $http.get(baseUrl + 'api/Employee/GetSpecialUserById',
				{ params: { userId: userId} });
		},
		insertSpecialUser: function (setMenu) {
			return $http.post(baseUrl + 'api/Employee/InsertSpecialUser', setMenu);
		},
		updateSpecialUser: function (setMenu) {
			return $http.put(baseUrl + 'api/Employee/UpdateSpecialUser', setMenu);
		},
		deleteSpecialUser: function (userId) {
			return $http['delete'](baseUrl + 'api/Employee/DeleteSpecialUser', { params: { userId: userId} });
		}
	};
} ]);