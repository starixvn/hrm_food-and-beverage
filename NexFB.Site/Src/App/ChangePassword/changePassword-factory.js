'use strict';
/*global appModule*/

appModule.factory('changePasswordFactory', ['$http', function ($http) {
	return {
		updatePassword: function (changePassword) {
			return $http.put('/api/ChangePassword/UpdatePassword', changePassword);
		}
	};
} ]);