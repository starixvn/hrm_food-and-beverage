'use strict';
/*global appModule*/

appModule.controller("LogoutController", ['$window', '$scope',
function ($window, $scope) {
	$scope.logout = function () {
		$window.location.href = "/Auth/Logout";
	};
} ]);