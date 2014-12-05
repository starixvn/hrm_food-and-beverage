'use strict';
/*global employeeModule*/

employeeModule.controller('employeeController',
['$scope', '$modal', 'employeeFactory', 'notificationFactory', 'commonFactory','ErrorFactory',
function ($scope, $modal, employeeFactory, notificationFactory, commonFactory, ErrorFactory) {
	/*
	** Declare variables.
	*/

	$scope.originalPage = 0;

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
	** Paging.
	*/
	$scope.showEntries = function () {
		commonFactory.tablePagination($scope, $scope.users, $scope.entrySelected);
	};

	$scope.entrySelected = 10;
	commonFactory.numberEntryList($scope);

	/*
	** Get all special users.
	*/
	var getSpecialUsers = function () {
		employeeFactory.getSpecialUsers().success(function (users) {
			$scope.users = users;
			commonFactory.tablePagination($scope, $scope.users, $scope.entrySelected);
			$scope.currentPage = $scope.originalPage;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	getSpecialUsers();


	/*
	** Add special user.
	*/
	$scope.addSpecialUser = function () {
		$scope.originalPage = $scope.currentPage;

		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/Employee/Templates/create.html',
			controller: "CreateEmployeeController",
			backdrop: 'static',
			windowClass: "modal-confirm-center set-menu-modal"
		});

		$scope.modalInstance.result.then(function () {
		})['finally'](function () {
			getSpecialUsers();
			$scope.modalInstance = '';
		});
	};

	/*
	** Edit special user.
	*/
	$scope.editSpecialUser = function (user) {
		employeeFactory.getSpecialUserById(user.UserId).success(function (data) {
			$scope.modalInstance = $modal.open({
				templateUrl: '/src/app/Employee/Templates/edit.html',
				controller: "EditEmployeeController",
				backdrop: 'static',
				resolve: {
					user: function () {
						return data;
					}
				},
				windowClass: "modal-confirm-center set-menu-modal"
			});

			$scope.modalInstance.result.then(function () {
			})['finally'](function () {
				getSpecialUsers();
				$scope.modalInstance = '';
			});
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	/*
	** Delete special user.
	*/
	var updatePaging = function (user, currentPage) {
		var index = $scope.setMenus.indexOf(user);
		$scope.users.splice(index, 1);

		commonFactory.tablePagination($scope, $scope.users, $scope.entrySelected);
		var numberPage = $scope.pagedItems.length;

		if (currentPage <= numberPage - 1) {
			$scope.currentPage = currentPage;
		}
		else {
			$scope.currentPage = currentPage - 1;
		}
	};

	$scope.deleteUser = function (user) {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			resolve: {
				action: function () {
					return 'delete';
				},
				name: function () {
					return user.Name;
				}
			},
			windowClass: "modal-confirm-center set-item-modal"
		});

		$scope.modalInstance.result.then(function () {
			var currentPage = $scope.currentPage;

			employeeFactory.deleteSpecialUser(user.UserId).success(function (response) {
				if (response.IsSuccessful) {
					updatePaging(user, currentPage);
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};
} ]);