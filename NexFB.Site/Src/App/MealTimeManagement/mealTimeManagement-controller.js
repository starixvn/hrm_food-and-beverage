'use strict';
/*global mealTimeManagementModule*/

mealTimeManagementModule.controller('MealTimeManagementController',
['$scope', 'mealTimeManagementFactory', 'ErrorFactory', 'notificationFactory',
function ($scope, mealTimeManagementFactory, ErrorFactory, notificationFactory) {
	$scope.hhPattern = /((2[0-3])|([01]?\d))/;
	$scope.mmPattern = /:[0-5]\d/;
	/*
	** Get all meal timesheet.
	*/
	mealTimeManagementFactory.getAllMealTimesheet().success(function (mealTimesheets) {
		$scope.mealTimesheets = mealTimesheets;
		angular.forEach(mealTimesheets, function (mealTimesheet) {
			mealTimesheet.isEdit = false;
		});
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get user permission.
	*/
	mealTimeManagementFactory.getUserPermissionOnMealTimeManagement().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Edit.
	*/
	$scope.edit = function (mealTimesheet) {
		mealTimesheet.isEdit = true;
	};

	/*
	** Cancel.
	*/
	$scope.cancel = function (mealTimesheet) {
		mealTimesheet.isEdit = false;
	};

	/*
	** Save.
	*/
	$scope.save = function (mealTimesheet) {
		var criteria = {
			registerTime: mealTimesheet.HHRegisterTime + ':' + mealTimesheet.MMRegisterTime,
			startTime: mealTimesheet.HHStartTime + ':' + mealTimesheet.MMStartTime,
			endTime: mealTimesheet.HHEndTime + ':' + mealTimesheet.MMEndTime
		};

		mealTimeManagementFactory.updateMealTimesheet(criteria)
		.success(function (response) {
			if (response.IsSuccessful) {
				notificationFactory.success("DataChangeSuccessfully");
				$modalInstance.close(informationMealPlanning);
			}
			else {
				notificationFactory.error("", response.Message);
			}
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		})['finally'](function () {
			mealTimesheet.isEdit = false;
		});
	};
} ]);