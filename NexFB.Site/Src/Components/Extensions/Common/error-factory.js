/*global appModule*/
'use strict';

appModule.factory("ErrorFactory", ['$location', function ($location) {
	return {
		errorHandle: function (status) {
			if (status === 405) {
				$location.path('/Home/AccessDenied');
			}
		}
	};
} ]);