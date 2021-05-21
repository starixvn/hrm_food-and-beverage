'use strict';
/*global orderingModule*/

kitchenStaffModule.controller('EditStaffOrderController',
	['$scope', '$modalInstance', '$modal', 'orderingFactory', 'orderingService', 'notificationFactory',
		'orderId', 'locations', '$translate', 'commonFactory', 'ErrorFactory', 'orderingUtilities', 'startTime', 'kitchenStaffFactory',
		function ($scope, $modalInstance, $modal, orderingFactory, orderingService, notificationFactory,
			orderId, locations, $translate, commonFactory, ErrorFactory, orderingUtilities, startTime, kitchenStaffFactory) {

			$scope.newMember = {};
			$scope.alterEdit = {};
			$scope.startTime = startTime;

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

				$translate('MessageDeliveredStatusOrder').then(function (message) {
					$scope.messageDeliveredStatusOrder = message;
				});
				$translate('MessageEditedNoMemberStatusOrder').then(function (message) {
					$scope.messageEditedNoMemberStatusOrder = message;
				});
				$translate('Delivered').then(function (message) {
					$scope.delivered = message;
				});
				$translate('NoMenuAvailableWarning').then(function (message) {
					$scope.noMenuAvailableWarning = message;
				});
				$translate('MessageDeliveredNoMemberStatusOrder').then(function (message) {
					$scope.messageDeliveredNoMemberStatusOrder = message;
				});
			};
			$scope.getMessage();

			$scope.locations = locations;
			$(".ddlLocation").width($(".ddlLocationNew").width());

			orderingFactory.getOrderById(orderId)
				.success(function (data) {
					$scope.order = data;
					orderingService.setMemberEditOrder(data.Members);
					orderingUtilities.setDefaultLocation($scope.locations, data.Location.LocationId);

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

						if (order.Status.StatusId == 4) {
							message = commonFactory.formatString(
								$scope.messageDeliveredStatusOrder, order.OrderNo, order.CreatorOrder.NickName, order.ModifiedTime, quantity);
						}
						else {
							message = "";
						}

						if (typeof order.Members !== 'undefined' && order.Members.length === 0) {
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

			var getOrderLocation = function () {
				var locationId = '';

				angular.forEach($scope.locations, function (value) {
					if (value.ticked) {
						locationId = value.LocationId;
					}
				});

				return locationId;
			};

			var saveOrder = function (order) {
				if (order.Location !== null) {
					order.Location.LocationId = getOrderLocation();
				}

				orderingFactory.checkDeliveryAreaExist(order.Location.LocationId).success(function (isExisted) {
					if (isExisted === 'true') {
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
							kitchenStaffFactory.editOrder(order).success(function (response) {
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
						});
					}
					else {
						notificationFactory.error('DeliveryAreaNotExistError');
					}
				}).error(function (response, status) {
					ErrorFactory.errorHandle(status);
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
				saveOrder(order);
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
					kitchenStaffFactory.deleteOrder(orderId).success(function (response) {
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

				if ($scope.modalInstance) {
					$scope.modalInstance.dismiss('cancel');
				}

				if ($scope.modalConfirmAlternativeMenuInstance) {
					$scope.modalConfirmAlternativeMenuInstance.dismiss('cancel');
				}
			});

			$scope.getEmployee = function (searchKey) {
				return orderingFactory.getEmployees(searchKey);
			};

			$scope.unSelectEmployee = function (searchKey) {
				if (searchKey.trim() === '') {
					if ($scope.newMember && $scope.newMember.employee) {
						$scope.newMember.employee = null;
					}
				}
			};

			$scope.add = function () {
				var newMember = $scope.newMember;
				if (newMember && newMember.employee) {
					var member = {
						Member: '',
						Menus: '',
						IsOrder: true,
						IsAllowOrder: false
					};
					member.Member = newMember.employee;
					orderingFactory.getOrderNewOrder(member.Member.UserId)
						.success(function (order) {
							member.CurrentStatus = order.Status;

							if (order.Members && order.Members.length > 0) {
								member.Menus = order.Members[0].Menus;
								member.IsAllowOrder = order.Members[0].IsAllowOrder;
							}

							var isExisted = false;

							var editMembers = orderingService.getMemberEditOrder();
							angular.forEach(editMembers, function (value) {
								if (value.Member.UserId === member.Member.UserId) {
									isExisted = true;
								}
							});

							if (!isExisted) {
								if (member.IsAllowOrder || (!member.IsAllowOrder && $scope.order.OrderId === order.OrderId)) {
									editMembers.push(member);
									orderingService.setMemberEditOrder(editMembers);
								}
								else if (!member.IsAllowOrder) {
									if (member.Menus && member.Menus[0].MenuId === -1) {
										var memberNonMenuMessage = '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + member.Member.NickName,
											message = $scope.noMenuAvailableWarning + memberNonMenuMessage;
										notificationFactory.error("", message);
									}
									else {
										notificationFactory.error("AddOverOrderMemberWarning");
									}
								}
							}
							else {
								notificationFactory.error('', $scope.warningMessageExistMember);
							}
						})
						.error(function (response, status) {
							ErrorFactory.errorHandle(status);
						});
					$('#txtEmployee').val('');
					$scope.newMember = {};
				}
			};

			/*
			** Calculate total price of all member in order
			*/
			$scope.calculateTotalAmount = function (members) {
				var total = orderingUtilities.calculateTotalAmount(members);
				return total;
			};
		}]);