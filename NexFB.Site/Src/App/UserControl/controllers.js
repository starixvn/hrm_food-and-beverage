'use strict';
/*global userControlModule*/

userControlModule.controller("ManageRoleController",
['$scope', '$route', 'userControlFactory', 'notificationFactory', 'ErrorFactory',
function ($scope, $route, userControlFactory, notificationFactory, ErrorFactory) {
	$scope.roleFunction = {
		functions: '',
		roleId: ''
	};

	userControlFactory.getAllSystemRole().success(function (roles) {
		if (roles) {
			var roleId = roles[0].RoleId;
			$scope.filter = {
				roleId: roleId
			};

			getFunctionByRole(roleId);
			$scope.roleOptions = roles;
		}
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

	var getFunctionByRole = function (roleId) {
		userControlFactory.getFunctionByRole(roleId).success(function (functionGroup) {
			debugger;
			$scope.functionGroup = functionGroup;
		}).error(function (response, status) {
			if (status === 405) {
				ErrorFactory.errorHandle(status);
			}
			else {
				notificationFactory.error("ErrorMessage");
			}
		});
	};

	$scope.query = function (roleId) {
		getFunctionByRole(roleId);
	};

	$scope.save = function (roleFunction) {
		$scope.disabledSubmitButton = true;

		userControlFactory.assignPermissionToRole(roleFunction).success(function (response) {
			if (response.isSuccessful === true) {
				notificationFactory.success("DataChangeSuccessfully");
			}
			else {
				notificationFactory.error("", response.message);
			}

			$scope.disabledSubmitButton = false;
		}).error(function (response, status) {
			if (status === 405) {
				ErrorFactory.errorHandle(status);
			}
			else {
				notificationFactory.error("ErrorMessage");
			}

			$scope.disabledSubmitButton = false;
		});
	};

	$scope.reload = function () {
		$route.reload();
	};
} ]);
