'use strict';
/*global employeeModule*/

employeeModule.controller('CreateSpecialEmployeeController',
['$scope', '$modalInstance', 'departmentItems', 'employeeFactory', 'notificationFactory', 'ErrorFactory',
function ($scope, $modalInstance, departmentItems, employeeFactory, notificationFactory, ErrorFactory) {
	/*
	** Declare variables.
	*/
	$scope.departmentItems = departmentItems;
	$scope.employeeTypeItems = [];

	$scope.defaultData = function () {
		$scope.user = {
			Code: '',
			NickName: '',
			EmployeeCard: '',
			IsActive: true,
			DepartmentId: '',
			EmployeeType: ''
		};
	};

	$scope.defaultData();

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

	$scope.saveSpecialUser = function (user, isContinue) {
		$scope.disabledSubmitButton = true;
		if ($scope.validateSpecialUser(user)) {
			employeeFactory.insertSpecialUser(user).success(function (response) {
				$scope.submitted = false;
				$scope.disabledSubmitButton = false;

				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
					if (!isContinue) {
						$modalInstance.close();
					}
					else {
						$scope.defaultData();
					}
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