'use strict';
/*global mealTimeManagementModule*/

mealTimeManagementModule.controller('MealTimeManagementController',
['$scope', 'mealTimeManagementFactory', 'ErrorFactory', 'notificationFactory',
function ($scope, mealTimeManagementFactory, ErrorFactory, notificationFactory) {
	$scope.isValid = true;
	$scope.errMessage = '';
	/*
	** Get all meal timesheet.
	*/
	mealTimeManagementFactory.getAllMealTimesheet().success(function (mealTimesheets) {
		$scope.originalMealTimesheets = angular.copy(mealTimesheets);
		$scope.mealTimesheets = mealTimesheets;
		angular.forEach($scope.mealTimesheets, function (mealTimesheet) {
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



	var checkConsecuteMeals = function () {
		var numberEdit = 0;
		angular.forEach($scope.mealTimesheets, function (timesheet) {
			if (timesheet.isEdit === true) {
				numberEdit++;
			}
		});

		if (numberEdit > 1) {
			return false;
		}

		return true;
	};

	$scope.onChange = function (mealTimesheet) {
		if (parseInt(mealTimesheet.HHRegisterTime) < 24 && parseInt(mealTimesheet.MMRegisterTime) < 60) {
			if (mealTimesheet.HHRegisterTime.length === 1) {
				mealTimesheet.HHRegisterTime = '0' + mealTimesheet.HHRegisterTime;
			}

			if (mealTimesheet.MMRegisterTime.length === 1) {
				mealTimesheet.MMRegisterTime = '0' + mealTimesheet.MMRegisterTime;
			}
		}
		else {
			$scope.isValid = false;
			$scope.errMessage = 'The Register time must be between 00:00 and 23:59.';
			return;
		}

		if (parseInt(mealTimesheet.HHStartTime) < 24 && parseInt(mealTimesheet.MMStartTime) < 60) {
			if (mealTimesheet.HHStartTime.length === 1) {
				mealTimesheet.HHStartTime = '0' + mealTimesheet.HHStartTime;
			}

			if (mealTimesheet.MMStartTime.length === 1) {
				mealTimesheet.MMStartTime = '0' + mealTimesheet.MMStartTime;
			}
		}
		else {
			$scope.isValid = false;
			$scope.errMessage = 'The Start time must be between 00:00 and 23:59.';
			return;
		}

		if (parseInt(mealTimesheet.HHEndTime) < 24 && parseInt(mealTimesheet.MMStartTime) < 60) {
			if (mealTimesheet.HHEndTime.length === 1) {
				mealTimesheet.HHEndTime = '0' + mealTimesheet.HHEndTime;
			}

			if (mealTimesheet.MMEndTime.length === 1) {
				mealTimesheet.MMEndTime = '0' + mealTimesheet.MMEndTime;
			}
		}
		else {
			$scope.isValid = false;
			$scope.errMessage = 'The End time must be between 00:00 and 23:59.';
			return;
		}
	};

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
		angular.forEach($scope.originalMealTimesheets, function (timesheet) {
			if (mealTimesheet.MealId === timesheet.MealId) {
				mealTimesheet.HHRegisterTime = timesheet.HHRegisterTime;
				mealTimesheet.MMRegisterTime = timesheet.MMRegisterTime;
				mealTimesheet.HHStartTime = timesheet.HHStartTime;
				mealTimesheet.MMStartTime = timesheet.MMStartTime;
				mealTimesheet.HHEndTime = timesheet.HHEndTime;
				mealTimesheet.MMEndTime = timesheet.MMEndTime;
			}
		});

		mealTimesheet.isEdit = false;
	};

	var saveMealTime = function (mealTimesheet) {
		var criteria = {
			mealId: mealTimesheet.MealId,
			registerTime: mealTimesheet.HHRegisterTime + ':' + mealTimesheet.MMRegisterTime,
			startTime: mealTimesheet.HHStartTime + ':' + mealTimesheet.MMStartTime,
			endTime: mealTimesheet.HHEndTime + ':' + mealTimesheet.MMEndTime
		};

		mealTimeManagementFactory.updateMealTimesheet(criteria)
		.success(function (response) {
			if (response.IsSuccessful) {
				notificationFactory.success("DataChangeSuccessfully");
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

	/*
	** Save.
	*/
	$scope.save = function (mealTimesheet) {
		if (checkConsecuteMeals()) {
			if ($scope.isValid) {
				saveMealTime(mealTimesheet);
			} else {
				notificationFactory.error('', $scope.errMessage);
			}
		} else {
			notificationFactory.error('ValidateNoconsecutiveMeal');
		}
	};
} ]);