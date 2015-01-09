/*global appModule*/
'use strict';

appModule.factory("ErrorFactory", ['$location', '$window', 'notificationFactory', 'baseUrl',
function ($location, $window, notificationFactory, baseUrl) {
	return {
		errorHandle: function (status) {
			if (status === 405) {
				$location.path('Home/AccessDenied');
			}
			else if (status === 401) {
				$.connection.hub.stop();
				$window.location.href = baseUrl + "Auth/Logout";
			}
			else {
				notificationFactory.error("ErrorMessage");
			}
		}
	};
} ]);