'use strict';
/*global userControlModule*/

userControlModule.controller("ManageRoleController",
['$scope', '$route', 'userControlFactory', 'notificationFactory', 'ErrorFactory',
function ($scope, $route, userControlFactory, notificationFactory, ErrorFactory) {
	$scope.roleFunction = {
		functions: '',
		roleId: ''
	};

	userControlFactory.getAllSystemRole(true).success(function (roles) {
		if (roles) {
			var roleId = roles[0].RoleId;
			$scope.filter = {
				roleId: roleId
			};

			getFunctionByRole(roleId);
			$scope.roleOptions = roles;
		}
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	userControlFactory.getManageRolePermission().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	var getFunctionByRole = function (roleId) {
		userControlFactory.getFunctionByRole(roleId).success(function (functionGroup) {
			$scope.functionGroup = functionGroup;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	$scope.query = function (roleId) {
		getFunctionByRole(roleId);
	};

	$scope.save = function (roleFunction) {
		$scope.disabledSubmitButton = true;

		userControlFactory.assignPermissionToRole(roleFunction).success(function (response) {
			if (response.IsSuccessful) {
				notificationFactory.success("DataChangeSuccessfully");
			}
			else {
				notificationFactory.error("", response.message);
			}

			$scope.disabledSubmitButton = false;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
			$scope.disabledSubmitButton = false;
		});
	};

	$scope.reload = function () {
		$route.reload();
	};
} ]);

userControlModule.controller("ManageUserController",
['$scope', 'userControlFactory', 'notificationFactory', '$route', '$modal', 'ErrorFactory', 'commonFactory',
function ($scope, userControlFactory, notificationFactory, $route, $modal, ErrorFactory, commonFactory) {
	$scope.filter = {
		keyword: '',
		departmentId: '',
		teamId: '',
		roleId: ''
	};

	userControlFactory.getManageUserPermission().success(function (permission) {
		$scope.policy = {
			permission: parseInt(permission)
		};
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	userControlFactory.getAllDepartment().success(function (departments) {
		$scope.departmentOptions = departments;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	$scope.getTeamByCompany = function (departmentId) {
		if (departmentId) {
			userControlFactory.getAllTeamByCompany(departmentId).success(function (teams) {
				$scope.teamOptions = teams;
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		}
		$scope.teamOptions = '';
		$scope.filter.teamId = '';
	};

	userControlFactory.getAllSystemRole().success(function (roles) {
		$scope.roleOptions = roles;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	$scope.showEntries = function () {
		commonFactory.tablePagination($scope, $scope.employees, $scope.entrySelected);
	};

	$scope.entrySelected = 20;
	commonFactory.numberEntryList($scope);

	function getAllEmployeeRole(filter) {
		userControlFactory.getAllEmployeeRole(filter.keyword, filter.departmentId, filter.teamId, filter.roleId)
		.success(function (employees) {
			$scope.employees = employees;
			commonFactory.tablePagination($scope, employees, $scope.entrySelected);
		})
		.error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	}

	getAllEmployeeRole($scope.filter);

	function getSelectedRole(filter) {
		filter.roleId = '';

		angular.forEach($scope.roleOptions, function (value) {
			if (value.ticked) {
				var roleId = value.RoleId;
				filter.roleId = filter.roleId !== '' ? filter.roleId + ',' + roleId : roleId;
			}
		});
	}

	$scope.query = function (filter) {
		getSelectedRole(filter);
		getAllEmployeeRole(filter);
	};

	$scope.assignRole = function (employee) {

		var modalAssignInstance = $modal.open({
			templateUrl: '/src/app/UserControl/Templates/UserPermission/assignRole.html',
			controller: "AssignRoleToUserController",
			backdrop: 'static',
			resolve: {
				userId: function () {
					return employee.UserId;
				}
			}
		});

		modalAssignInstance.result.then(function (userRoles) {

			userControlFactory.assignRoleToUser(userRoles).success(function (response) {
				if (response.IsSuccessful) {
					employee.Roles = userRoles.rolesName;
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					notificationFactory.error("DataChangeFail");
				}
			});
		});
	};

	$scope.reload = function () {
		$route.reload();
	};
} ]);

userControlModule.controller('AssignRoleToUserController',
['$scope', '$modalInstance', 'userId', 'userControlFactory', 'ErrorFactory',
function ($scope, $modalInstance, userId, userControlFactory, ErrorFactory) {

	userControlFactory.getEmployeeRoleById(userId).success(function (employee) {
		$scope.employee = employee;
		$scope.roles = employee.RoleItems;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	userControlFactory.getAllSystemRole(false).success(function (systemRoles) {
		$scope.systemRoles = systemRoles;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	userControlFactory.getAllDepartment().success(function (departments) {
		$scope.departments = departments;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	$scope.checkRolePermission = function (roleId) {
		var isChecked = false;
		angular.forEach($scope.roles, function (role) {
			if (role.RoleId === roleId) {
				isChecked = true;
			}
		});

		return isChecked;
	};

	$scope.checkDepartmentPermission = function (roleId, departmentId) {
		var isChecked = false;
		angular.forEach($scope.roles, function (role) {
			if (role.RoleId === roleId) {
				angular.forEach(role.Departments, function (department) {
					if (department.DepartmentId === departmentId) {
						isChecked = true;
					}
				});
			}
		});

		return isChecked;
	};

	$scope.save = function (roles, roleDepartments, rolesName) {
		var userRoles = {
			systemID: userId,
			roleID: roles,
			roleDepartments: roleDepartments,
			rolesName: rolesName
		};
		$modalInstance.close(userRoles);
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);