'use strict';
/*global orderingModule*/

orderingModule.controller("OrderingController",
['$translate', '$scope', '$route', '$timeout', '$modal', 'orderingFactory', 'orderingService', 'notificationFactory',
function ($translate, $scope, $route, $timeout, $modal, orderingFactory, orderingService, notificationFactory) {
	$scope.newOrder = [];
	$scope.savedOrder = [];

	$.connection.hub.start();
	var orderSignalR = $.connection.order;
	$.connection.wpf;
	$scope.sendNewOrder = function (orderId) {
		orderSignalR.server.sendNewOrder(orderId);
	};

	orderSignalR.client.newOrderForWeb = function (order) {
		var isRelatedCurrentUser = false;
		if ($scope.currentUser.UserId === order.CreatorOrder.UserId) {
			isRelatedCurrentUser = true;
		}
		else {
			angular.forEach(order.Members, function (value) {
				if (value.Member.UserId === $scope.currentUser.UserId) {
					isRelatedCurrentUser = true;
					order.MyMeal = value.Menu;
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
		if (order.Members.length > 0) {
			if ($scope.currentUser.UserId === order.CreatorOrder.UserId) {
				isRelatedCurrentUser = true;
			}
			else {
				angular.forEach(order.Members, function (value) {
					if (value.Member.UserId === $scope.currentUser.UserId) {
						isRelatedCurrentUser = true;
						order.MyMeal = value.Menu;
					}
				});
			}
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

	$scope.getDefaultForAddOrder = function () {
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
	};

	$scope.getDefaultForAddOrder();

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
		$scope.modalAddMemberInstance = $modal.open({
			templateUrl: '/src/app/Ordering/Templates/AddMember.html',
			controller: "AddMemberController",
			backdrop: 'static',
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

	$scope.removeMember = function (index, memberId, isSavedOrder) {
		$scope.modalRemoveMemberInstance = $modal.open({
			templateUrl: '/Src/Components/Extensions/Common/Templates/ModalConfirmContent2.html',
			controller: "ModalConfirmController2",
			backdrop: 'static',
			resolve: {
				content: function () {
					return 'Are you sure you want to remove this member?';
				}
			},
			windowClass: "modal-confirm-center1"
		});

		$scope.modalRemoveMemberInstance.result.then(function () {
			if (isSavedOrder) {
				orderingFactory.removeMember(memberId).success(function (response) {
					if (response.IsSuccessful === true) {
						$scope.savedOrder.Members.splice(index, 1);
						notificationFactory.success("DataChangeSuccessfully");
					}
					else {
						notificationFactory.error('', response.Message);
					}
				});
			}
			else {
				$scope.newOrder.Members.splice(index, 1);
			}
		});
	};

	$scope.saveAddOrder = function (order) {
		$scope.modalConfirmSaveOrderInstance = $modal.open({
			templateUrl: '/Src/Components/Extensions/Common/Templates/ModalConfirmContent2.html',
			controller: "ModalConfirmController2",
			backdrop: 'static',
			resolve: {
				content: function () {
					return 'Are you sure you want to save this order?';
				}
			},
			windowClass: "modal-confirm-center1"
		});

		$scope.modalConfirmSaveOrderInstance.result.then(function () {
			orderingFactory.createOrder(order).success(function (response) {
				if (response.result.IsSuccessful === true) {
					notificationFactory.success("DataChangeSuccessfully");
					$scope.sendNewOrder(response.orderId);
					$scope.getDefaultForAddOrder();
				}
				else {
					notificationFactory.error('', response.result.Message);
				}
			});
		});
	};

	$scope.editOrder = function (orderId) {
		$scope.modalEditOrderInstance = $modal.open({
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

		$scope.modalEditOrderInstance.result.then(function (receiveObject) {
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

	$scope.$on('$routeChangeStart', function () {
		if ($scope.modalEditOrderInstance) {
			$scope.modalEditOrderInstance.dismiss('cancel');
		}

		if ($scope.modalConfirmSaveOrderInstance) {
			$scope.modalConfirmSaveOrderInstance.dismiss('cancel');
		}

		if ($scope.modalRemoveMemberInstance) {
			$scope.modalRemoveMemberInstance.dismiss('cancel');
		}

		if ($scope.modalAddMemberInstance) {
			$scope.modalAddMemberInstance.dismiss('cancel');
		}
	});
} ]);

orderingModule.controller('AddMemberController',
['$scope', '$modalInstance', 'orderingFactory', 'orderingService', 'isSavedOrder', 'isEditOrder',
'notificationFactory',
function ($scope, $modalInstance, orderingFactory, orderingService, isSavedOrder, isEditOrder, notificationFactory) {
	$scope.newMember = {};
	$scope.departmentId = '0';
	var message = 'The selected member has already been in the order.';
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
					if (dataStatus.Status != null) {
						notificationFactory.error('', message);
					}
					else {
						member.CurrentStatus = dataStatus.Status;

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
								notificationFactory.error('', message);
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
									orderingFactory.addMember(member).success(function (response) {
										if (response.IsSuccessful === true) {
											savedMembers.push(member);
											orderingService.setMemberSavedOrder(savedMembers);
											notificationFactory.success("DataChangeSuccessfully");
										}
										else {
											notificationFactory.error('', response.Message);
										}
									});
								}
								else {
									notificationFactory.error('', message);
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
									notificationFactory.error('', message);
								}
							}
						}
					}
				})
				.error(function () {
					// TODO
				});


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
		$scope.modalAddMemberInstance = $modal.open({
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
		$scope.modalConfirmSaveEditInstance = $modal.open({
			templateUrl: '/Src/Components/Extensions/Common/Templates/ModalConfirmContent2.html',
			controller: "ModalConfirmController2",
			backdrop: 'static',
			resolve: {
				content: function () {
					return 'Are you sure you want to save this order? When the order is received, all removed member cannot be added again.';
				}
			},
			windowClass: "modal-confirm-center1"
		});

		$scope.modalConfirmSaveEditInstance.result.then(function () {
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
		});
	};

	$scope.deleteOrder = function (orderId) {
		$scope.modalRemoveMemberInstance = $modal.open({
			templateUrl: '/Src/Components/Extensions/Common/Templates/ModalConfirmContent2.html',
			controller: "ModalConfirmController2",
			backdrop: 'static',
			resolve: {
				content: function () {
					return 'Are you sure you want to delete this order?';
				}
			},
			windowClass: "modal-confirm-center1"
		});

		$scope.modalRemoveMemberInstance.result.then(function () {
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

	$scope.$on('$routeChangeStart', function () {
		if ($scope.modalRemoveMemberInstance) {
			$scope.modalRemoveMemberInstance.dismiss('cancel');
		}

		if ($scope.modalConfirmSaveEditInstance) {
			$scope.modalConfirmSaveEditInstance.dismiss('cancel');
		}

		if ($scope.modalAddMemberInstance) {
			$scope.modalAddMemberInstance.dismiss('cancel');
		}
	});
} ]);