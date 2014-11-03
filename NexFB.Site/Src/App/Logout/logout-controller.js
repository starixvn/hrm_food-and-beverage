'use strict';
/*global appModule*/

appModule.controller("LogoutController", ['$window', '$scope',
function ($window, $scope) {
	$scope.logout = function () {
		$.connection.hub.stop();
		$window.location.href = "/Auth/Logout";
	};
} ]);