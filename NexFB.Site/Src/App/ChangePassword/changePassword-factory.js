'use strict';
/*global appModule*/

appModule.factory('changePasswordFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		updatePassword: function (changePassword) {
			return $http.put(baseUrl + 'api/ChangePassword/UpdatePassword', changePassword);
		}
	};
} ]);