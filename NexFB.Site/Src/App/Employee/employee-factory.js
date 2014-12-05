'use strict';
/*global employeeModule*/

employeeModule.factory('employeeFactory', ['$http', function ($http) {
	return {
		getSpecialUsers: function () {
			return $http.get('/api/Employee/GetSpecialUsers');
		},
		getSpecialUserById: function (userId) {
			return $http.get('/api/Employee/GetSpecialUserById',
				{ params: { userId: userId} });
		},
		insertSpecialUser: function (setMenu) {
			return $http.post('/api/Employee/InsertSpecialUser', setMenu);
		},
		updateSpecialUser: function (setMenu) {
			return $http.put('/api/Employee/UpdateSpecialUser', setMenu);
		},
		deleteSpecialUser: function (userId) {
			return $http['delete']('/api/Employee/DeleteSpecialUser', { params: { userId: userId} });
		}
	};
} ]);