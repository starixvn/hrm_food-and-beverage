'use strict';
/*global employeeModule*/

employeeModule.controller('EditSpecialEmployeeController',
['$scope', '$modalInstance', 'departmentItems', 'employeeFactory', 'notificationFactory', 'ErrorFactory', 'user',
function ($scope, $modalInstance, departmentItems, employeeFactory, notificationFactory, ErrorFactory, user) {
	/*
	** Declare variables.
	*/
	$scope.departmentItems = departmentItems;
	$scope.user = user;

	/*
	** Get all employee type.
	*/
	var getEmployeeType = function (departmentId) {
		employeeFactory.getEmployeeType(departmentId).success(function (employeeTypes) {
			$scope.employeeTypeItems = employeeTypes;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	$scope.getEmployeeTypeByDepartment = function (departmentId) {
		getEmployeeType(departmentId);
	};

	getEmployeeType(user.DepartmentId);

	$scope.validateSpecialUser = function (user) {
		var isValid = true;

		if (!user.Code) {
			isValid = false;
			notificationFactory.error("RequireEmployeeCode");
		}

		if (!user.NickName) {
			isValid = false;
			notificationFactory.error("RequireEmployeeName");
		}

		if (!user.DepartmentId) {
			isValid = false;
			notificationFactory.error("RequireDepartment");
		}

		if (!user.EmployeeType) {
			isValid = false;
			notificationFactory.error("RequireEmployeeType");
		}

		return isValid;
	};

	$scope.saveSpecialUser = function (user) {
		$scope.disabledSubmitButton = true;
		if ($scope.validateSpecialUser(user)) {
			employeeFactory.updateSpecialUser(user).success(function (response) {
				$scope.submitted = false;
				$scope.disabledSubmitButton = false;

				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
					$modalInstance.close();
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
				$scope.disabledSubmitButton = false;
			});
		}

		$scope.disabledSubmitButton = false;
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);