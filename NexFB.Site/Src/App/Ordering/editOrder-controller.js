'use strict';
/*global orderingModule*/

orderingModule.controller('EditOrderController',
['$scope', '$modalInstance', '$modal', 'orderingFactory', 'orderingService', 'notificationFactory',
'orderId', 'locations', '$translate', 'commonFactory', 'ErrorFactory', 'orderingUtilities',
function ($scope, $modalInstance, $modal, orderingFactory, orderingService, notificationFactory,
	orderId, locations, $translate, commonFactory, ErrorFactory, orderingUtilities) {

	$scope.newMember = {};
	$scope.alterEdit = {};

	$scope.getMessage = function () {
		$translate('ConfirmMessageUpdateOrder').then(function (message) {
			$scope.confirmMessageUpdateOrder = message;
		});
		$translate('ConfirmMessageDeleteOrder').then(function (message) {
			$scope.confirmMessageDeleteOrder = message;
		});
		$translate('WarningMessageExistMember').then(function (message) {
			$scope.warningMessageExistMember = message;
		});
		$translate('MessageNewStatusOrder').then(function (message) {
			$scope.messageNewStatusOrder = message;
		});
		$translate('MessageReceivedStatusOrder').then(function (message) {
			$scope.messageReceivedStatusOrder = message;
		});
		$translate('MessageReducedStatusOrder').then(function (message) {
			$scope.messageReducedStatusOrder = message;
		});
		$translate('MessageReducedNoMemberStatusOrder').then(function (message) {
			$scope.messageReducedNoMemberStatusOrder = message;
		});
		$translate('MessageDeliveredStatusOrder').then(function (message) {
			$scope.messageDeliveredStatusOrder = message;
		});
		$translate('MessageDeliveredNoMemberStatusOrder').then(function (message) {
			$scope.messageDeliveredNoMemberStatusOrder = message;
		});
		$translate('MessageEditedStatusOrder').then(function (message) {
			$scope.messageEditedStatusOrder = message;
		});
		$translate('ConfirmMessageAlternativeMenu').then(function (message) {
			$scope.confirmMessageAlternativeMenu = message;
		});
	};
	$scope.getMessage();

	$scope.locations = locations;
	$(".ddlLocation").width($(".ddlLocationNew").width());

	orderingFactory.getOrderById(orderId)
		.success(function (data) {
			$scope.order = data;
			orderingService.setMemberEditOrder(data.Members);

			$scope.getStatusOrderMessage = function (order) {
				var message = '';
				var quantity = '';
				angular.forEach(order.MenuQuantity, function (value, index) {
					if (index === 0) {
						quantity = value.Quantity + ' "' + value.Menu.Name + '"';
					}
					else {
						quantity += ", " + value.Quantity + ' "' + value.Menu.Name + '"';
					}
				});

				switch (order.Status.StatusId) {
					case 1:
						message = commonFactory.formatString(
							$scope.messageNewStatusOrder, order.OrderNo, order.CreatorOrder.NickName, order.ModifiedTime, quantity);
						break;
					case 2:
						message = commonFactory.formatString(
							$scope.messageReceivedStatusOrder, order.OrderNo, order.CreatorOrder.NickName, order.ModifiedTime, quantity);
						break;
					case 3:
						message = commonFactory.formatString(
							$scope.messageReducedStatusOrder, order.OrderNo, order.CreatorOrder.NickName, order.ModifiedTime, quantity);
						break;
					case 4:
						message = commonFactory.formatString(
							$scope.messageDeliveredStatusOrder, order.OrderNo, order.CreatorOrder.NickName, order.ModifiedTime, quantity);
						break;
					case 5:
						message = commonFactory.formatString(
							$scope.messageEditedStatusOrder, order.OrderNo, order.CreatorOrder.NickName, order.ModifiedTime, quantity);
						break;
					default:
						message = "";
				}

				if (typeof order.Members !== 'undefined' && order.Members.length === 0) {
					if (order.Status.StatusId === 3) {
						message = commonFactory.formatString(
							$scope.messageReducedNoMemberStatusOrder, order.OrderNo, order.CreatorOrder.NickName, order.ModifiedTime);
					}
					if (order.Status.StatusId === 4) {
						message = commonFactory.formatString(
							$scope.messageDeliveredNoMemberStatusOrder, order.OrderNo, order.CreatorOrder.NickName, order.ModifiedTime);
					}
				}

				return message;
			};
		})
		.error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};

	var saveOrder = function (order) {
		$scope.modalConfirmSaveEditInstance = $modal.open({
			templateUrl: '/Src/Components/Extensions/Common/Templates/ModalConfirmContent2.html',
			controller: "ModalConfirmController2",
			backdrop: 'static',
			resolve: {
				content: function () {
					return $scope.confirmMessageUpdateOrder;
				}
			},
			windowClass: "modal-confirm-center1"
		});

		$scope.modalConfirmSaveEditInstance.result.then(function () {
			if (!order.IsSideDishOrder) {
				orderingFactory.editOrder(order).success(function (response) {
					if (response.IsSuccessful) {
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
				})
				.error(function (response, status) {
					ErrorFactory.errorHandle(status);
				});
			}
			else {
				orderingFactory.editOrderSideDish(order).success(function (response) {
					if (response.IsSuccessful) {
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
				})
				.error(function (response, status) {
					ErrorFactory.errorHandle(status);
				});
			}
		});
	};

	var validateNewOrder = function (order) {
		var isValid = true;

		angular.forEach(order.Members, function (member) {
			if (member.Menus !== undefined && member.Menus.length === 0) {
				isValid = false;
			}
		});

		if (!isValid) {
			notificationFactory.error("InvalidFoodItem");
		}

		if (order.Location === null || order.Location.LocationId === 0) {
			isValid = false;
			notificationFactory.error("InvalidLocation");
		}

		return isValid;
	};

	$scope.saveEditOrder = function (order) {
		if (order.IsSideDishOrder) {
			if (validateNewOrder(order)) {
				saveOrder(order);
			}
		}
		else {
			saveOrder(order);
		}
	};

	$scope.deleteOrder = function (orderId) {
		$scope.modalRemoveMemberInstance = $modal.open({
			templateUrl: '/Src/Components/Extensions/Common/Templates/ModalConfirmContent2.html',
			controller: "ModalConfirmController2",
			backdrop: 'static',
			resolve: {
				content: function () {
					return $scope.confirmMessageDeleteOrder;
				}
			},
			windowClass: "modal-confirm-center1"
		});

		$scope.modalRemoveMemberInstance.result.then(function () {
			orderingFactory.deleteOrder(orderId).success(function (response) {
				if (response.IsSuccessful) {
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
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
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

	$scope.getEmployee = function (searchKey) {
		orderingFactory.getEmployees(searchKey).then(
		function (employees) {
			$scope.searchEmployees = employees;
		},
		function (response) {
			ErrorFactory.errorHandle(response.status);
		});
	};

	$scope.add = function () {
		var newMember = $scope.newMember;
		if (newMember !== undefined && newMember.employee !== undefined) {
			var member = {
				Member: '',
				Menus: '',
				IsOrder: true
			};
			member.Member = newMember.employee;

			if (!$scope.order.IsSideDishOrder) {
				orderingFactory.getOrderNewOrder(member.Member.UserId)
				.success(function (order) {
					member.CurrentStatus = order.Status;

					if (order.Members && order.Members.length > 0) {
						member.Menus = order.Members[0].Menus;
					}

					var isExisted = false;

					if (member.CurrentStatus != null && $scope.order.OrderId !== order.OrderId) {
						notificationFactory.error('', $scope.warningMessageExistMember);
					}
					else if (member.CurrentStatus === null || (member.CurrentStatus !== null && $scope.order.OrderId === order.OrderId)) {
						if (member.CurrentStatus !== null && $scope.order.OrderId === order.OrderId) {
							$scope.existInCurrentOrder = true;
						}

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
							notificationFactory.error('', $scope.warningMessageExistMember);
						}
					}
				})
				.error(function (response, status) {
					ErrorFactory.errorHandle(status);
				});
			}
			else {
				orderingFactory.getOrderById($scope.order.OrderId).success(function (currentOrder) {
					var isExistedInOrder = false;

					angular.forEach(currentOrder.Members, function (rawMember) {
						if (rawMember.Member.UserId === member.Member.UserId) {
							isExistedInOrder = true;
							member.Menus = rawMember.Menus;
						}
					});

					var isExisted = false;
					var editMembers = orderingService.getMemberEditOrder();

					angular.forEach(editMembers, function (value) {
						if (value.Member.UserId === member.Member.UserId) {
							isExisted = true;
						}
					});
					if (!isExisted) {
						member.Menus = '';
						editMembers.push(member);
						orderingService.setMemberEditOrder(editMembers);
					}
					else {
						notificationFactory.error('', $scope.warningMessageExistMember);
					}
				}).error(function (response, status) {
					ErrorFactory.errorHandle(status);
				});
			}

			$('#txtEmployee').val('');
			$scope.newMember = {};
		}
	};

	/*
	** Get alternative menus.
	*/
	$scope.selectedMenu = [];

	$scope.getAlternativeMenus = function (member, index) {

		$scope.modalConfirmAlternativeMenuInstance = $modal.open({
			templateUrl: '/Src/Components/Extensions/Common/Templates/ModalConfirmContent2.html',
			controller: "ModalConfirmController2",
			backdrop: 'static',
			resolve: {
				content: function () {
					return $scope.confirmMessageAlternativeMenu;
				}
			},
			windowClass: "modal-confirm-center1"
		});

		$scope.modalConfirmAlternativeMenuInstance.result.then(function () {
			var userId = member.Member.UserId;

			orderingFactory.getAlternativeMenuByUserId(userId).success(function (allMenus) {
				member.AllMenus = allMenus;
				$scope.selectedMenu[index] = orderingUtilities.getSelectedMenu(member.Menus[0], allMenus);

				var key = 'alternativeMenu' + userId;
				$scope.alterEdit[key] = true;
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		}, function () {
		})['finally'](function () {
			$scope.modalConfirmAlternativeMenuInstance = undefined;
		});
	};

	/*
	** Edit side dish.
	*/
	$scope.editSideDish = function (member) {
		$scope.modalInstance = $modal.open({
			templateUrl: '/src/app/Ordering/Templates/SelectSideDish.html',
			controller: "SelectSideDishController",
			resolve: {
				member: function () {
					return angular.copy(member);
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		$scope.modalInstance.result.then(function (menus) {
			member.Menus = menus;
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};

	/*
	** Calculate total price of all member in order
	*/
	$scope.calculateTotalAmount = function (members) {
		var total = orderingUtilities.calculateTotalAmount(members);
		return total;
	};
} ]);