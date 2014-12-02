'use strict';
/*global appModule*/

appModule.controller("ChangePasswordController", ['$scope', '$modalInstance', 'changePasswordFactory', 'notificationFactory', 'ErrorFactory',
function ($scope, $modalInstance, changePasswordFactory, notificationFactory, ErrorFactory) {
	$scope.disabledSubmitButton = false;
	$scope.changePassword = {
		OldPassword: '',
		NewPassword: '',
		ConfirmPassword: ''
	};

	$scope.validateChangePassword = function (changePassword) {
		var isValid = true;

		if (changePassword.OldPassword.length < 1) {
			isValid = false;
			notificationFactory.error("RequireOldPassword");
		}

		if (changePassword.NewPassword.length < 1) {
			isValid = false;
			notificationFactory.error("RequireNewPassword");
		}
		else if (changePassword.NewPassword.length < 6) {
			isValid = false;
			notificationFactory.error("ValidateLengthPassword");
		}

		if (changePassword.ConfirmPassword.length < 1) {
			isValid = false;
			notificationFactory.error("RequireConfirmPassword");
		}

		if (isValid && changePassword.NewPassword !== changePassword.ConfirmPassword) {
			isValid = false;
			notificationFactory.error("ValidateWrongConfirmPassword");
		}

		return isValid;
	};

	$scope.save = function (changePassword) {
		$scope.disabledSubmitButton = true;
		if ($scope.validateChangePassword(changePassword)) {
			changePasswordFactory.updatePassword(changePassword).success(function (response) {
				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
					$modalInstance.close();
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		}

		$scope.disabledSubmitButton = false;
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);