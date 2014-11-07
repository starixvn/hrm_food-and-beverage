'use strict';
/*global shiftMealMappingModule*/

shiftMealMappingModule.controller('ShiftMealMappingController',
['$scope', 'shiftMealMappingFactory', 'commonFactory', 'ErrorFactory', '$modal', 'shiftMealMappingUtilities',
'notificationFactory',
function ($scope, shiftMealMappingFactory, commonFactory, ErrorFactory, $modal, shiftMealMappingUtilities,
notificationFactory) {

	/*
	** Paging.
	*/
	$scope.showEntries = function () {
		commonFactory.tablePagination($scope, $scope.shiftMeals, $scope.entrySelected, 'ShiftName');
	};

	$scope.entrySelected = 15;
	$scope.originalPage = 0;
	commonFactory.numberEntryList($scope);

	/*
	** If route change, close dialog.
	*/
	$scope.$on('$routeChangeStart', function () {
		// if modal instance defined, dismiss dialog
		if ($scope.modalInstance) {
			$scope.modalInstance.dismiss('cancel');
		}
	});

	/*
	** Get permisson on Shift-Meal Mapping.
	*/
	shiftMealMappingFactory.getPermissionOnShiftMealMapping().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all department.
	*/
	shiftMealMappingFactory.getAllDepartment().success(function (departments) {
		$scope.departmentOptions = departments;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all shift meals.
	*/
	var getAllShiftMeals = function (departmentId) {
		shiftMealMappingFactory.getAllShiftMeal(departmentId).success(function (shiftMeals) {
			$scope.shiftMeals = shiftMeals;
			$scope.originalShiftMeals = angular.copy(shiftMeals);
			commonFactory.tablePagination($scope, $scope.shiftMeals, $scope.entrySelected, 'ShiftName');
			$scope.currentPage = $scope.originalPage;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	getAllShiftMeals(null);

	$scope.query = function (deaprtmentId) {
		getAllShiftMeals(deaprtmentId);
	};
	/*
	** Submit shit-meal mapping.
	*/
	var getOriginalShift = function (shiftId, originalShiftMeals) {
		var shift = '';

		if (originalShiftMeals) {
			var numberShiftMeals = originalShiftMeals.length;

			for (var i = 0; i < numberShiftMeals; i++) {
				var currentShift = originalShiftMeals[i];

				if (shiftId === currentShift.ShiftId) {
					shift = currentShift;
					i = numberShiftMeals;
				}
			}
		}

		return shift;
	};

	var revertCheckMealOfShift = function (originalShift, updateShift) {
		if (originalShift && updateShift) {
			updateShift.MealBreakfast = originalShift.MealBreakfast;
			updateShift.MealLunch = originalShift.MealLunch;
			updateShift.MealDinner = originalShift.MealDinner;
			updateShift.MealSupper = originalShift.MealSupper;
			updateShift.MealNextBreakfast = originalShift.MealNextBreakfast;
			updateShift.MealNextLunch = originalShift.MealNextLunch;
		}
	};

	$scope.submit = function (shift) {
		var shiftId = shift.ShiftId,
			originalShift = getOriginalShift(shiftId, $scope.originalShiftMeals);

		if (shiftMealMappingUtilities.validateShiftMealMapping(shift)) {
			$scope.modalInstance = $modal.open({
				templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
				controller: "ModalConfirmController",
				backdrop: 'static',
				resolve: {
					action: function () {
						return 'save';
					},
					name: function () {
						return shift.ShiftName;
					}
				},
				windowClass: "modal-confirm-center food-item-modal"
			});

			$scope.modalInstance.result.then(function () {

				var updateShift = {
					shiftId: shift.ShiftId,
					departmentId: shift.Department.DepartmentId,
					mealBreakfast: shift.MealBreakfast,
					mealLunch: shift.MealLunch,
					mealDinner: shift.MealDinner,
					mealSupper: shift.MealSupper,
					mealNextBreakfast: shift.MealNextBreakfast,
					mealNextLunch: shift.MealNextLunch
				};

				shiftMealMappingFactory.updateShiftMeal(updateShift).success(function (response) {
					if (response.IsSuccessful) {
						notificationFactory.success("DataChangeSuccessfully");
					}
					else {
						revertCheckMealOfShift(originalShift, shift);
						notificationFactory.error("", response.Message);
					}
				}).error(function (response, status) {
					revertCheckMealOfShift(originalShift, shift);
					ErrorFactory.errorHandle(status);
				});
			})['finally'](function () {
				$scope.modalInstance = '';
			});
		}
	};
} ]);