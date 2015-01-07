'use strict';
/*global groupControlModule*/

groupControlModule.controller('CreateGroupController',
['$scope', '$modalInstance', 'groupControlFactory', 'notificationFactory', 'ErrorFactory', 'groupControlUtilities',
function ($scope, $modalInstance, groupControlFactory, notificationFactory, ErrorFactory, groupControlUtilities) {
	$scope.group = {
		groupName: '',
		isActive: true
	};

	/*
	** Set form scope to current scope.
	*/
	$scope.setFormScope = function (formScope) {
		$scope.formScope = formScope;
	};

	$scope.addGroup = function (group) {
		$scope.isDisabledSubmitButton = true;

		if (groupControlUtilities.validateGroup(group)) {
			groupControlFactory.insertGroup(group).success(function (response) {
				if (response.IsSuccessful) {
					$modalInstance.close();
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			})['finally'](function () {
				$scope.isDisabledSubmitButton = false;
			});
		}
		else {
			$scope.isDisabledSubmitButton = false;
		}
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);