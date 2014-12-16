'use strict';
/*global appModule*/

appModule.controller("AuthenticationController", ['$window', '$scope',  '$modal', 'baseUrl',
function ($window, $scope, $modal, baseUrl) {
	$scope.logout = function () {
		$.connection.hub.stop();
		$window.location.href = baseUrl + "Auth/Logout";
	};

	$scope.changePassword = function () {
		$scope.originalPage = $scope.currentPage;

		$scope.modalInstance = $modal.open({
			templateUrl: baseUrl + 'src/app/ChangePassword/Templates/index.html',
			controller: "ChangePasswordController",
			backdrop: 'static',
			windowClass: "modal-confirm-center modal-change-password"
		});

		$scope.modalInstance.result.then(function () {
		})['finally'](function () {
		});
	};
} ]);