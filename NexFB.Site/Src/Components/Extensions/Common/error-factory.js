/*global appModule*/
'use strict';

appModule.factory("ErrorFactory", ['$location', '$window', 'notificationFactory',
function ($location, $window, notificationFactory) {
	return {
		errorHandle: function (status) {
			debugger;
			if (status === 405) {
				$location.path('/Home/AccessDenied');
			}
			else if (status === 401) {
				$.connection.hub.stop();
				$window.location.href = "/Auth/Logout";
			}
			else {
				notificationFactory.error("ErrorMessage");
			}
		}
	};
} ]);