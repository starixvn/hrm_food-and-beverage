'use strict';
/*global mealTimeManagementModule*/

mealTimeManagementModule.controller('MealTimeManagementController',
['$scope', '$modal', 'baseUrl', 'mealTimeManagementFactory', 'ErrorFactory', 'notificationFactory',
function ($scope, $modal, baseUrl, mealTimeManagementFactory, ErrorFactory, notificationFactory) {
	$scope.isEdit = false;
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

	$scope.onChange = function (mealTimesheet) {
		if (mealTimesheet.HHRegisterTime.length === 1) {
			mealTimesheet.HHRegisterTime = '0' + mealTimesheet.HHRegisterTime;
		}

		if (mealTimesheet.MMRegisterTime.length === 1) {
			mealTimesheet.MMRegisterTime = '0' + mealTimesheet.MMRegisterTime;
		}

		if (mealTimesheet.HHStartTime.length === 1) {
			mealTimesheet.HHStartTime = '0' + mealTimesheet.HHStartTime;
		}

		if (mealTimesheet.MMStartTime.length === 1) {
			mealTimesheet.MMStartTime = '0' + mealTimesheet.MMStartTime;
		}

		if (mealTimesheet.HHEndTime.length === 1) {
			mealTimesheet.HHEndTime = '0' + mealTimesheet.HHEndTime;
		}

		if (mealTimesheet.MMEndTime.length === 1) {
			mealTimesheet.MMEndTime = '0' + mealTimesheet.MMEndTime;
		}
	};

	/*
	** Edit.
	*/
	$scope.edit = function (mealTimesheet) {
		if ($scope.isEdit) {
			var modalConfirm = $modal.open({
				templateUrl: baseUrl + 'src/app/MealTimeManagement/Templates/modelConfirm.html',
				controller: "ModelConfirmController",
				backdrop: 'static',
				resolve: {
					meal: function () {
						var meal = {};
						angular.forEach($scope.mealTimesheets, function (timesheet) {
							if (timesheet.isEdit === true) {
								meal = timesheet;
							}
						});
						return meal;
					}
				},
				windowClass: "modal-confirm-edit"
			});

			modalConfirm.result.then(function (meal) {
				saveMealTime(meal);
				mealTimesheet.isEdit = true;
			});
		}
		else {
			$scope.isEdit = true;
			mealTimesheet.isEdit = true;
		}
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

		$scope.isEdit = false;
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
				$scope.isEdit = false;
				mealTimesheet.isEdit = false;
			}
			else {
				notificationFactory.error("", response.Message);
			}
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	/*
	** Save.
	*/
	$scope.save = function (mealTimesheet) {
		saveMealTime(mealTimesheet);
	};
} ]);

mealTimeManagementModule.controller('ModelConfirmController',
['$scope', '$modalInstance', 'meal',
function ($scope, $modalInstance, meal) {
	$scope.save = function () {
		$modalInstance.close(meal);
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);