'use strict';
/*global orderingModule*/

orderingModule.controller("OrderingController",
['$scope', '$route', '$timeout', '$modal', 'orderingFactory', 'orderingService', 'notificationFactory',
function ($scope, $route, $timeout, $modal, orderingFactory, orderingService, notificationFactory) {
	$scope.newOrder = [];
	$scope.savedOrder = [];

	$.connection.hub.start();
	var orderSignalR = $.connection.order;
	$scope.sendNewOrder = function (orderId) {
		orderSignalR.server.sendNewOrder(orderId);
	};

	orderSignalR.client.newOrderForWeb = function (order) {
		//$scope.currentUser
		var isRelatedCurrentUser = false;
		if ($scope.currentUser.UserId === order.CreatorOrder.UserId) {
			isRelatedCurrentUser = true;
		}
		else {
			angular.forEach(order.Members, function (value) {
				if (value.Member.UserId === $scope.currentUser.UserId) {
					isRelatedCurrentUser = true;
				}
			});
		}

		if (isRelatedCurrentUser) {
			$scope.myOrder.push(order);
			$scope.$apply();
		}
	};

	$scope.sendEditOrder = function (orderId) {
		orderSignalR.server.sendEditOrder(orderId);
	};

	orderSignalR.client.editOrderForWeb = function (order) {
		//$scope.currentUser
		var indexObject = -1;
		angular.forEach($scope.myOrder, function (value, index) {
				if (value.OrderId === order.OrderId) {
					indexObject = index;
				}
			});
		
		if (indexObject > -1) {
			$scope.myOrder.splice(indexObject, 1);
			$scope.$apply();
		}

		var isRelatedCurrentUser = false;
		if ($scope.currentUser.UserId === order.CreatorOrder.UserId) {
			isRelatedCurrentUser = true;
		}
		else {
			angular.forEach(order.Members, function (value) {
				if (value.Member.UserId === $scope.currentUser.UserId) {
					isRelatedCurrentUser = true;
				}
			});
		}

		if (isRelatedCurrentUser) {
			$scope.myOrder.push(order);
			$scope.$apply();
		}
	};

	$scope.sendDeleteOrder = function (orderId) {
		orderSignalR.server.sendDeleteOrder(orderId);
	};

	orderSignalR.client.deleteOrderForWeb = function (orderId) {
		var indexObject = -1;
		angular.forEach($scope.myOrder, function (value, index) {
				if (value.OrderId === orderId) {
					indexObject = index;
				}
			});

		if (indexObject > -1) {
			$scope.myOrder.splice(indexObject, 1);
			$scope.$apply();
		}
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
				},
				isEditOrder: function () {
					return false;
				}
			},
			windowClass: "modal-confirm-center1"
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
			if (response.result.IsSuccessful === true) {
				notificationFactory.success("DataChangeSuccessfully");
				$scope.sendNewOrder(response.orderId);
			}
			else {
				notificationFactory.error('', response.Message);
			}
		});
	};

	$scope.editOrder = function (orderId) {
		var modalInstance = $modal.open({
			templateUrl: '/src/app/Ordering/Templates/EditOrder.html',
			controller: "EditOrderController",
			backdrop: 'static',
			resolve: {
				orderId: function () {
					return orderId;
				}
			},
			windowClass: "modal-confirm-center"
		});

		modalInstance.result.then(function (receiveObject) {
			if (receiveObject.isDeleted) {
				$scope.sendDeleteOrder(receiveObject.orderId);
			}
			else {
				$scope.sendEditOrder(receiveObject.order.OrderId);
			}
		}, function () {
			// TODO
		});
	};

	orderingFactory.getCurrentUser()
		.success(function (data) {
			$scope.currentUser = data;
			orderingService.setCurrentUser(data);
		})
		.error(function () {
			// TODO
		});
} ]);

orderingModule.controller('AddMemberController',
['$scope', '$modalInstance', 'orderingFactory', 'orderingService', 'isSavedOrder', 'isEditOrder',
'notificationFactory',
function ($scope, $modalInstance, orderingFactory, orderingService, isSavedOrder, isEditOrder, notificationFactory) {
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

				var isExisted = false;
				if (isEditOrder) {
					var editMembers = orderingService.getMemberEditOrder();
					angular.forEach(editMembers, function (value) {
						if (value.Member.UserId === member.Member.UserId) {
							isExisted = true;
						}
					});

					if (!isExisted) {
						editMembers.push(member);
						orderingService.setMemberEditOrder(editMembers);
					}
					else {
						notificationFactory.error('', 'User is existed in order.');
					}
				}
				else {
					if (isSavedOrder) {
						var savedMembers = orderingService.getMemberSavedOrder();
						angular.forEach(savedMembers, function (value) {
							if (value.Member.UserId === member.Member.UserId) {
								isExisted = true;
							}
						});

						if (!isExisted) {
							savedMembers.push(member);
							orderingService.setMemberSavedOrder(savedMembers);
						}
						else {
							notificationFactory.error('', 'User is existed in order.');
						}
					}
					else {
						var members = orderingService.getMemberNewOrder();

						angular.forEach(members, function (value) {
							if (value.Member.UserId === member.Member.UserId) {
								isExisted = true;
							}
						});

						if (!isExisted) {
							members.push(member);
							orderingService.setMemberNewOrder(members);
						}
						else {
							notificationFactory.error('', 'User is existed in order.');
						}
					}
				}
				if (!isContinue) {
					$modalInstance.close(newMember);
				}
			})
			.error(function () {
				// TODO
			});

		$('#txtEmployee').val('');
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
			orderingService.setMemberEditOrder(data.Members);
		})
		.error(function () {
			// TODO
		});

	$scope.showAddMember = function () {
		$modal.open({
			templateUrl: '/src/app/Ordering/Templates/AddMember.html',
			controller: "AddMemberController",
			backdrop: 'static',
			resolve: {
				isSavedOrder: function () {
					return false;
				},
				isEditOrder: function () {
					return true;
				}
			},
			windowClass: "modal-confirm-center1"
		});
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};

	$scope.saveEditOrder = function (order) {
		orderingFactory.editOrder(order).success(function (response) {
			if (response.IsSuccessful === true) {
				notificationFactory.success("DataChangeSuccessfully");
				var returnObject = {
					order: order,
					isDeleted: false,
					orderId: 0
				};
				$modalInstance.close(returnObject);
			}
			else {
				notificationFactory.error('', response.Message);
			}
		});
	};

	$scope.deleteOrder = function (orderId, orderNo) {
		var modalRemoveMemberInstance = $modal.open({
			templateUrl: '/Src/Components/Extensions/Common/Templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			backdrop: 'static',
			resolve: {
				name: function () {
					return "order #" + orderNo;
				},
				action: function () {
					return "cancel";
				}
			},
			windowClass: "modal-confirm-center1"
		});

		modalRemoveMemberInstance.result.then(function () {
			orderingFactory.deleteOrder(orderId).success(function (response) {
				if (response.IsSuccessful === true) {
					notificationFactory.success("DataChangeSuccessfully");
					var returnObject = {
						order: [],
						isDeleted: true,
						orderId: orderId
					};
					$modalInstance.close(returnObject);
				}
				else {
					notificationFactory.error('', response.Message);
				}
			});
		});
	};

	$scope.removeMember = function (index) {
		$scope.order.Members.splice(index, 1);
	};
} ]);