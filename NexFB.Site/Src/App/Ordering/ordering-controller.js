'use strict';
/*global orderingModule*/

orderingModule.controller("OrderingController",
['$scope', '$route', '$timeout', '$modal', 'orderingFactory', 'orderingService', 'notificationFactory',
function ($scope, $route, $timeout, $modal, orderingFactory, orderingService, notificationFactory) {
	$scope.newOrder = [];
	$scope.savedOrder = [];
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
			orderingService.setMemberNewOrder(data.Members);
		})
		.error(function () {
			// TODO
		});

	orderingFactory.getSavedOrder()
		.success(function (data) {
			$scope.savedOrder = data;
			orderingService.setMemberSavedOrder(data.Members);
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
		$modal.open({
			templateUrl: '/src/app/Ordering/Templates/AddMember.html',
			controller: "AddMemberController",
			resolve: {
				isSavedOrder: function () {
					return isSavedOrder;
				}
			}
		});
	};

	$scope.removeMember = function (index, isSavedOrder) {
		if (isSavedOrder) {
			$scope.savedOrder.Members.splice(index, 1);
		}
		else {
			$scope.newOrder.Members.splice(index, 1);
		}
	};

	$scope.saveAddOrder = function (order) {
		//validate
		orderingFactory.createOrder(order).success(function (response) {
			if (response.IsSuccessful === true) {
				notificationFactory.success("DataChangeSuccessfully");
			}
			else {
				notificationFactory.error('', response.Message);
			}
		});
	};

	$scope.editOrder = function (orderId) {
		$modal.open({
			templateUrl: '/src/app/Ordering/Templates/EditOrder.html',
			controller: "EditOrderController",
			resolve: {
				orderId: function () {
					return orderId;
				}
			}
		});
	};
} ]);

orderingModule.controller('AddMemberController',
['$scope', '$modalInstance', 'orderingFactory', 'orderingService', 'isSavedOrder',
function ($scope, $modalInstance, orderingFactory, orderingService, isSavedOrder) {
	$scope.newMember = {};
	$scope.departmentId = '1';
	$scope.add = function (isContinue) {
		var newMember = $scope.newMember;
		var member = {
			Member: '',
			Menu: '',
			IsOrder: true
		};
		member.Member = newMember.employee;
		orderingFactory.getMenuByUserId(member.Member.UserId)
			.success(function (data) {
				member.Menu = data;
				orderingFactory.getOrderByUserId(member.Member.UserId)
				.success(function (dataStatus) {
					member.CurrentStatus = dataStatus.Status;
				})
				.error(function () {
					// TODO
				});
				if (isSavedOrder) {
					var savedMembers = orderingService.getMemberSavedOrder();
					savedMembers.push(member);
					orderingService.setMemberSavedOrder(savedMembers);
				}
				else {
					var members = orderingService.getMemberNewOrder();
					members.push(member);
					orderingService.setMemberNewOrder(members);
				}

				if (!isContinue) {
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

orderingModule.controller('EditOrderController',
['$scope', '$modalInstance', '$modal', 'orderingFactory', 'orderingService', 'notificationFactory', 'orderId',
function ($scope, $modalInstance, $modal, orderingFactory, orderingService, notificationFactory, orderId) {
	//$scope.editOrder = '';
	orderingFactory.getLocation()
		.success(function (data) {
			$scope.locations = data;
		})
		.error(function () {
			// TODO
		});

	orderingFactory.getOrderById(orderId)
		.success(function (data) {
			$scope.order = data;
			orderingService.setMemberNewOrder(data.Members);
		})
		.error(function () {
			// TODO
		});

	$scope.showAddMember = function () {
		$modal.open({
			templateUrl: '/src/app/Ordering/Templates/AddMember.html',
			controller: "AddMemberController",
			resolve: {
				isSavedOrder: function () {
					return false;
				}
			}
		});
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};

	$scope.saveEditOrder = function (order) {
		orderingFactory.editOrder(order).success(function (response) {
			if (response.IsSuccessful === true) {
				notificationFactory.success("DataChangeSuccessfully");
				$modalInstance.close(order);
			}
			else {
				notificationFactory.error('', response.Message);
			}
		});
	};

	$scope.deleteOrder = function (orderId) {
		orderingFactory.deleteOrder(orderId).success(function (response) {
			if (response.IsSuccessful === true) {
				notificationFactory.success("DataChangeSuccessfully");
				$modalInstance.close();
			}
			else {
				notificationFactory.error('', response.Message);
			}
		});
	};

	$scope.removeMember = function (index) {
		$scope.order.Members.splice(index, 1);
	};
} ]);