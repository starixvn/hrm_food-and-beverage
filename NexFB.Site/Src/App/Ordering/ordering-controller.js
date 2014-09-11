'use strict';
/*global orderingModule*/

orderingModule.controller("OrderingController",
['$scope', '$route', '$timeout', '$modal', 'orderingFactory', 'orderingService',
function ($scope, $route, $timeout, $modal, orderingFactory, orderingService) {

	//	$scope.counter = 0;
	//	$scope.onTimeout = function () {
	//		$scope.counter++;
	//		mytimeout = $timeout($scope.onTimeout, 1000);
	//		//chat.server.sendMessage('aaaa');
	//	}
	//	var mytimeout = $timeout($scope.onTimeout, 1000);

	$.connection.hub.start();
	var chat = $.connection.chat;
	//chat.hubs.start();
	$scope.messages = [];
	$scope.sendMessage = function () {
		//signalRSvc.sendRequest();
		chat.server.sendMessage($scope.newMessage);
		$scope.newMessage = "";
	};


	//	$scope.$parent.$on("newMessage", function (e, message) {
	//		$scope.messages.push({ message: message });
	//		$scope.$apply();
	//	});

	chat.client.newMessage = function (message) {
		$scope.messages.push({ message: message });
		$scope.$apply();
		console.log(message);
	};

	orderingFactory.getMyOrder()
		.success(function (data) {
			$scope.myOrder = data;
		})
		.error(function () {
			// TODO
		});

	orderingFactory.getAddNewOrder()
		.success(function (data) {
			$scope.newOrder = data;
			orderingService.setMemberNewOrder(data.Members)
		})
		.error(function () {
			// TODO
		});

	orderingFactory.getSavedOrder()
		.success(function (data) {
			$scope.savedOrder = data;
			orderingService.setMemberSavedOrder(data.Members)
		})
		.error(function () {
			// TODO
		});

	orderingFactory.getLocation()
		.success(function (data) {
			$scope.locations = data;
		})
		.error(function () {
			// TODO
		});

	orderingFactory.getMyMenu()
		.success(function (data) {
			$scope.menu = data;
		})
		.error(function () {
			// TODO
		});

	$scope.$watch(function () {
		return orderingService.getMemberNewOrder();
	}, function (value) {
		$scope.newOrder.Members = value;
	});

	$scope.$watch(function () {
		return orderingService.getMemberSavedOrder();
	}, function (value) {
		$scope.savedOrder.Members = value;
	});

	$scope.showAddMember = function (isSavedOrder) {
		var addMemberModalInstance = $modal.open({
			templateUrl: '/src/app/Ordering/Templates/AddMember.html',
			controller: "AddMemberController",
			resolve: {
				isSavedOrder: function () {
					return isSavedOrder;
				},
				action: function () {
					return "delete";
				}
			}
		});
	}
} ]);

orderingModule.controller('AddMemberController',
['$scope', '$modalInstance', 'orderingFactory', 'orderingService', 'isSavedOrder',
function ($scope, $modalInstance, orderingFactory, orderingService, isSavedOrder) {
	$scope.newMember = {};
	$scope.departmentId = '1';
	$scope.add = function (isContinue) {
		var newMember = $scope.newMember;
		var member = []
		member.Member = newMember.employee;
		orderingFactory.getMenuByUserId(member.Member.UserId)
			.success(function (data) {
				member.Menu = data;
				member.IsOrder = true;
				if (!isSavedOrder) {
					var members = orderingService.getMemberNewOrder();
					members.push(member);
					orderingService.setMemberNewOrder(members);
				}
				else {
					var savedMembers = orderingService.getMemberSavedOrder();
					savedMembers.push(member);
					orderingService.setMemberSavedOrder(savedMembers);
				}
				if (isContinue) {
					
				}
				else {
					$modalInstance.close(newMember);
				}
			})
			.error(function () {
				// TODO
			});



	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};

	orderingFactory.getDepartment()
		.success(function (data) {
			$scope.departments = data;
		})
		.error(function () {
			// TODO
		});

	$scope.getEmployee = function (departmentId, searchKey) {
		orderingFactory.getEmployees(departmentId, searchKey).then(function (employees) {
			$scope.searchEmployees = employees;
		});
	};

} ]);