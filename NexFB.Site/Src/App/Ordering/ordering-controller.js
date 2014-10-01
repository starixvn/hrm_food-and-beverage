'use strict';
/*global orderingModule*/

orderingModule.controller("OrderingController",
['$translate', '$scope', '$route', '$timeout', '$modal', 'orderingFactory', 'orderingService', 'notificationFactory',
function ($translate, $scope, $route, $timeout, $modal, orderingFactory, orderingService, notificationFactory) {
	$scope.getMessage = function () {
		$translate('ConfirmMessageSaveOrder').then(function (message) {
			$scope.confirmMessageSaveOrder = message;
		});

		$translate('ConfirmMessageRemoveMember').then(function (message) {
			$scope.confirmMessageRemoveMember = message;
		});

		$translate('WarningMessageExistMember').then(function (message) {
			$scope.warningMessageExistMember = message;
		});
	};

	$scope.getMessage();

	$scope.newOrder = [];
	$scope.savedOrder = [];
	$scope.myOrder = [];

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
			var data = orderingService.getMyOrder();
			data.push(order);
			orderingService.setMyOrder(data);
			$scope.$apply();
		}
	};

	$scope.sendEditOrder = function (orderId) {
		orderSignalR.server.sendEditOrder(orderId);
	};

	orderSignalR.client.editOrderForWeb = function (order) {
		var data = orderingService.getMyOrder();
		var indexObject = -1;
		angular.forEach($scope.myOrder, function (value, index) {
			if (value.OrderId === order.OrderId) {
				indexObject = index;
			}
		});

		if (indexObject > -1) {
			data.splice(indexObject, 1);
		}

		var isRelatedCurrentUser = false;
		if ($scope.currentUser.UserId === order.CreatorOrder.UserId) {
			if (order.Status.StatusId !== 3 && order.Members.length === 0) {
				isRelatedCurrentUser = false;
			}
			else {
				isRelatedCurrentUser = true;
			}
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
			data.push(order);
		}

		orderingService.setMyOrder(data);
		$scope.$apply();
	};

	$scope.sendDeleteOrder = function (orderId) {
		orderSignalR.server.sendDeleteOrder(orderId);
	};

	orderSignalR.client.deleteOrderForWeb = function (orderId) {
		var data = orderingService.getMyOrder();
		var indexObject = -1;
		angular.forEach($scope.myOrder, function (value, index) {
			if (value.OrderId === orderId) {
				indexObject = index;
			}
		});

		if (indexObject > -1) {
			data.splice(indexObject, 1);
			orderingService.setMyOrder(data);
			$scope.$apply();
		}
	};

	$scope.$watch(function () {
		return orderingService.getMyOrder();
	}, function (value) {
		$scope.myOrder = value;
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

	$scope.loadData = function (callback) {
		orderingFactory.getCurrentUser()
		.success(function (data) {
			$scope.currentUser = data;
			orderingService.setCurrentUser(data);
		})
		.error(function () {
			notificationFactory.error("ErrorMessage");
		});

		orderingFactory.getMyMenu()
		.success(function (data) {
			$scope.menu = data;
		})
		.error(function () {
			notificationFactory.error("ErrorMessage");
		});

		$scope.getDefaultForAddOrder = function () {
			orderingFactory.getAddNewOrder()
			.success(function (data) {
				$scope.newOrder = data;
				orderingService.setMemberNewOrder(data.Members);
			})
			.error(function () {
				notificationFactory.error("ErrorMessage");
			});

			orderingFactory.getSavedOrder()
			.success(function (data) {
				$scope.savedOrder = data;
				orderingService.setMemberSavedOrder(data.Members);
			})
			.error(function () {
				notificationFactory.error("ErrorMessage");
			});
		};

		orderingFactory.getMyOrder()
			.success(function (data) {
				orderingService.setMyOrder(data);
				$scope.myOrder = data;
			})
			.error(function () {
				notificationFactory.error("ErrorMessage");
			});

		orderingFactory.getLocation()
			.success(function (data) {
				$scope.locations = data;
				$scope.getDefaultForAddOrder();
			})
			.error(function () {
				notificationFactory.error("ErrorMessage");
			});

		callback();
	};

	$scope.bindAction = function () {
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

		$scope.ValidateNewOrder = function (order) {
			var isValid = true;

			if (order.Members.length < 1) {
				isValid = false;
				notificationFactory.error("InvalidInputData");
			}
			else {
				isValid = false;
				angular.forEach(order.Members, function (value) {
					if (value.IsOrder) {
						isValid = true;
					}
				});

				if (!isValid) {
					notificationFactory.error("InvalidInputData");
				}
			}

			if (order.Location === null || order.Location.LocationId === 0) {
				isValid = false;
				notificationFactory.error("InvalidLocation");
			}

			return isValid;
		};

		$scope.saveAddOrder = function (order) {
			if ($scope.ValidateNewOrder(order)) {
				$scope.modalConfirmSaveOrderInstance = $modal.open({
					templateUrl: '/Src/Components/Extensions/Common/Templates/ModalConfirmContent2.html',
					controller: "ModalConfirmController2",
					backdrop: 'static',
					resolve: {
						content: function () {
							return $scope.confirmMessageSaveOrder;
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
				}, function () {
				})['finally'](function () {
					$scope.modalConfirmSaveOrderInstance = undefined;
				});
			}
		};

		$scope.removeMember = function (index, memberId, isSavedOrder) {
			$scope.modalRemoveMemberInstance = $modal.open({
				templateUrl: '/Src/Components/Extensions/Common/Templates/ModalConfirmContent2.html',
				controller: "ModalConfirmController2",
				backdrop: 'static',
				resolve: {
					content: function () {
						return $scope.confirmMessageRemoveMember;
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
			}, function () {
			})['finally'](function () {
				$scope.modalRemoveMemberInstance = undefined;
			});
		};

		$scope.editOrder = function (orderId) {
			if ($scope.locations.length > 0) {
				$scope.modalEditOrderInstance = $modal.open({
					templateUrl: '/src/app/Ordering/Templates/EditOrder.html',
					controller: "EditOrderController",
					backdrop: 'static',
					resolve: {
						orderId: function () {
							return orderId;
						},
						locations: function () {
							return $scope.locations;
						}
					}
				});

				$scope.modalEditOrderInstance.result.then(function (receiveObject) {
					if (receiveObject.isDeleted) {
						$scope.sendDeleteOrder(receiveObject.orderId);
					}
					else {
						$scope.sendEditOrder(receiveObject.order.OrderId);
					}

					$scope.getDefaultForAddOrder();
				}, function () {
				})['finally'](function () {
					$scope.modalEditOrderInstance = undefined;
				});
			}
		};
	};

	$scope.getEmployee = function (searchKey) {
		orderingFactory.getEmployees(searchKey).then(function (employees) {
			$scope.searchEmployees = employees;
		});
	};

	$scope.add = function (isSavedOrder) {
		var newMember = isSavedOrder ? $scope.savedMember : $scope.newMember;
		if (newMember !== undefined && newMember.employee !== undefined) {
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
					var isExisted = false;
					if (isSavedOrder) {
						if (member.CurrentStatus != null) {
							member.IsOrder = false;
						}
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
							notificationFactory.error('', $scope.warningMessageExistMember);
						}

						$('#txtEmployeeForSavedOrder').val('');
						$scope.savedMember = {};
					}
					else {
						if (member.CurrentStatus != null) {
							notificationFactory.error('', $scope.warningMessageExistMember);
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
								notificationFactory.success("DataChangeSuccessfully");
							}
							else {
								notificationFactory.error('', $scope.warningMessageExistMember);
							}
						}

						$('#txtEmployeeForNewOrder').val('');
						$scope.newMember = {};
					}
				})
				.error(function () {
					notificationFactory.error("ErrorMessage");
				});
			})
			.error(function () {
				notificationFactory.error("ErrorMessage");
			});
		}
	};

	$scope.showMember = function (order) {
		order.IsShowMember = true;
	};

	$scope.hideMember = function (order) {
		order.IsShowMember = false;
	};

	$scope.loadData(function () {
		$scope.bindAction();
	});

} ]);