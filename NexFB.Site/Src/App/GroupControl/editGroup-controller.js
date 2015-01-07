'use strict';
/*global groupControlModule*/

groupControlModule.controller('EditGroupController',
['$scope', '$modalInstance', 'group', 'groupControlFactory', 'notificationFactory', 'ErrorFactory', 'groupControlUtilities',
function ($scope, $modalInstance, group, groupControlFactory, notificationFactory, ErrorFactory, groupControlUtilities) {

	$scope.group = group;

	$scope.updateGroup = function (grp) {
		$scope.isDisabledSubmitButton = true;
		var editedGroup = angular.copy(grp);

		if (groupControlUtilities.validateGroup(editedGroup)) {
			groupControlFactory.updateGroup(editedGroup).success(function (response) {
				if (response.IsSuccessful) {
					notificationFactory.success("DataChangeSuccessfully");
					$modalInstance.close(group);
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