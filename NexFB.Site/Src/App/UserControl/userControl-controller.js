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
			if (response.IsSuccessful === true) {
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

userControlModule.controller("ManageUserController",
['$scope', 'userControlFactory', 'notificationFactory',
function ($scope, userControlFactory, notificationFactory) {
	$scope.filter = {
		keyword: '',
		departmentId: '',
		teamId: '',
		roleId: ''
	};

	userControlFactory.getAllDepartment().success(function (departments) {
		$scope.departmentOptions = departments;
	}).error(function () {
		// TODO
	});

	$scope.getTeamByCompany = function (departmentId) {
		if (departmentId) {
			userControlFactory.getAllTeamByCompany(departmentId).success(function (teams) {
				$scope.teamOptions = teams;
			}).error(function () {
				// TODO
			});
		}
		else {
			$scope.teamOptions = '';
		}

		$scope.filter.teamId = '';
	};

	userControlFactory.getAllSystemRole().success(function (roles) {
		$scope.roleOptions = roles;
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

	function getAllEmployeeRole(filter) {
		userControlFactory.getAllEmployeeRole(filter.keyword, filter.departmentId, filter.teamId, filter.roleId)
		.success(function (employees) {
			$scope.employees = employees;
		})
		.error(function () {
			// TODO
		});
	}

	getAllEmployeeRole($scope.filter);

	function getSelectedRole(filter) {
		filter.roleId = '';

		angular.forEach($scope.roleOptions, function (value) {
			if (value.ticked === true) {
				var roleId = value.RoleId;
				filter.roleId = filter.roleId !== '' ? filter.roleId + ',' + roleId : roleId;
			}
		});
	}

	$scope.query = function (filter) {
		getSelectedRole(filter);
		getAllEmployeeRole(filter);
	};
} ]);
