'use strict';
/*global orderingModule*/

orderingModule.controller('EditOrderController',
['$scope', '$modalInstance', '$modal', 'orderingFactory', 'orderingService', 'notificationFactory', 
'orderId', 'locations', '$translate', 'commonFactory', 'ErrorFactory',
function ($scope, $modalInstance, $modal, orderingFactory, orderingService, notificationFactory,
	orderId, locations, $translate, commonFactory, ErrorFactory) {

	$scope.newMember = {};
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
		$translate('MessageDeliveredStatusOrder').then(function (message) {
			$scope.messageDeliveredStatusOrder = message;
		});
		$translate('MessageEditedStatusOrder').then(function (message) {
			$scope.messageEditedStatusOrder = message;
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

				return message;
			};
		})
		.error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});

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
					return $scope.confirmMessageUpdateOrder;
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
			})
			.error(function (response, status) {
				ErrorFactory.errorHandle(status);
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
					return $scope.confirmMessageDeleteOrder;
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
		orderingFactory.getEmployees(searchKey).success(function (employees) {
			$scope.searchEmployees = employees;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	$scope.add = function () {
		var newMember = $scope.newMember;
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
					if (member.CurrentStatus != null) {
						notificationFactory.error('', $scope.warningMessageExistMember);
					}
					else {
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
			})
			.error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});

			$('#txtEmployee').val('');
			$scope.newMember = {};
		}
	};
} ]);