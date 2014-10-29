'use strict';
/*global orderingModule*/

orderingModule.controller("OrderingController",
['$translate', '$scope', '$route', '$timeout', '$modal', 'orderingFactory', 'orderingService',
'notificationFactory', 'ErrorFactory', 'orderingUtilities',
function ($translate, $scope, $route, $timeout, $modal, orderingFactory, orderingService,
	notificationFactory, ErrorFactory, orderingUtilities) {
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
			if ((order.Status.StatusId === 1 || order.Status.StatusId === 99) && order.Members.length === 0) {
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

	// close Saved Order and Side Dish Order when don't have New Order
	$scope.$watch('newOrder.Status', function (newValue) {
		if (newValue === null) {
			$scope.$broadcast("closeOrderContent");
		}
	});

	$scope.loadData = function (callback) {
		orderingFactory.getCurrentUser()
		.success(function (data) {
			$scope.currentUser = data;
			orderingService.setCurrentUser(data);
		})
		.error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});

		orderingFactory.getMyMenu()
		.success(function (data) {
			$scope.menu = data;
		})
		.error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});

		$scope.getDefaultForAddOrder = function () {
			orderingFactory.getAddNewOrder()
			.success(function (data) {
				var newMembers = [];

				angular.forEach(data.Members, function (member) {
					// new order
					if (member.IsOrder) {
						newMembers.push(member);
					}
				});

				data.Members = newMembers;
				$scope.newOrder = data;
				orderingService.setMemberNewOrder(data.Members);
			})
			.error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});

			orderingFactory.getSavedOrder()
			.success(function (data) {
				$scope.savedOrder = data;
				orderingService.setMemberSavedOrder(data.Members);
			})
			.error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});

			// Side Dish order
			orderingFactory.getAddNewOrder()
			.success(function (data) {
				var newMembers = [];

				if (data.Members != null && data.Members.length > 0) {
					var member = data.Members[0];
					member.IsOrder = true;
					member.Menus = [];
					newMembers.push(member);
				}

				data.Members = newMembers;
				$scope.sideDishOrder = data;
				orderingService.setMemberSideDishOrder(data.Members);
			})
			.error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		};

		orderingFactory.getMyOrder()
			.success(function (data) {
				orderingService.setMyOrder(data);
				$scope.myOrder = data;
			})
			.error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});

		orderingFactory.getLocation()
			.success(function (data) {
				$scope.locations = data;
				$scope.getDefaultForAddOrder();
			})
			.error(function (response, status) {
				ErrorFactory.errorHandle(status);
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

		$scope.ValidateNewOrder = function (order, isSideDishOrder) {
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

			if (isSideDishOrder) {
				isValid = true;
				angular.forEach(order.Members, function (member) {
					if (member.Menus !== undefined && member.Menus.length === 0) {
						isValid = false;
					}
				});

				if (!isValid) {
					notificationFactory.error("InvalidFoodItem");
				}
			}

			if (order.Location === null || order.Location.LocationId === 0) {
				isValid = false;
				notificationFactory.error("InvalidLocation");
			}

			return isValid;
		};

		var closeAlternativeSelect = function () {
			var newMembers = orderingService.getMemberNewOrder(),
				savedMembers = orderingService.getMemberSavedOrder();

			angular.forEach(newMembers, function (member) {
				var userId = member.Member.UserId,
					key = 'alternativeMenu' + userId;

				$scope.alter[key] = false;
			});

			angular.forEach(savedMembers, function (member) {
				var userId = member.Member.UserId,
					key = 'alternativeMenu' + userId;

				$scope.alterSaved[key] = false;
			});
		};

		$scope.saveAddOrder = function (order, isSideDishOrder) {
			if ($scope.ValidateNewOrder(order, isSideDishOrder)) {
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
					closeAlternativeSelect();

					if (!isSideDishOrder) {
						orderingFactory.createOrder(order)
						.success(function (response) {
							if (response.result.IsSuccessful === true) {
								notificationFactory.success("DataChangeSuccessfully");
								$scope.sendNewOrder(response.orderId);
								$scope.getDefaultForAddOrder();
							}
							else {
								notificationFactory.error('', response.result.Message);
							}
						})
						.error(function (response, status) {
							ErrorFactory.errorHandle(status);
						});
					}
					else {
						orderingFactory.createOrderSideDish(order)
						.success(function (response) {
							if (response.result.IsSuccessful === true) {
								notificationFactory.success("DataChangeSuccessfully");
								$scope.sendNewOrder(response.orderId);
								$scope.getDefaultForAddOrder();
							}
							else {
								notificationFactory.error('', response.result.Message);
							}
						})
						.error(function (response, status) {
							ErrorFactory.errorHandle(status);
						});
					}
				}, function () {
				})['finally'](function () {
					$scope.modalConfirmSaveOrderInstance = undefined;
				});
			}
		};

		$scope.removeMember = function (index, memberId, typeOrder) {
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
				// New Order
				if (typeOrder === 1) {
					$scope.newOrder.Members.splice(index, 1);
					var key = 'alternativeMenu' + memberId;
					$scope.alter[key] = false;
				}
				// Saved Order
				else if (typeOrder === 2) {
					orderingFactory.removeMember(memberId).success(function (response) {
						if (response.IsSuccessful === true) {
							$scope.savedOrder.Members.splice(index, 1);
							notificationFactory.success("DataChangeSuccessfully");
						}
						else {
							notificationFactory.error('', response.Message);
						}
					})
					.error(function (response, status) {
						ErrorFactory.errorHandle(status);
					});
				}
				// Side dish Order
				else if (typeOrder === 3) {
					$scope.sideDishOrder.Members.splice(index, 1);
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

					closeAlternativeSelect();
					$scope.getDefaultForAddOrder();
				}, function () {
				})['finally'](function () {
					$scope.modalEditOrderInstance = undefined;
				});
			}
		};
	};

	$scope.getEmployee = function (searchKey) {
		orderingFactory.getEmployees(searchKey).then(
		function (employees) {
			$scope.searchEmployees = employees;
		},
		function (response) {
			ErrorFactory.errorHandle(response.status);
		});
	};

	$scope.add = function (isSavedOrder, isSideDishOrder) {
		var newMember = isSideDishOrder ? $scope.sideDishMember :
				(isSavedOrder ? $scope.savedMember : $scope.newMember);
		if (newMember !== undefined && newMember.employee !== undefined) {
			var member = {
				Member: '',
				Menus: '',
				AllMenus: '',
				IsOrder: true
			};
			member.Member = newMember.employee;
			var userId = member.Member.UserId;

			orderingFactory.getOrderNewOrder(userId)
				.success(function (order) {
					member.CurrentStatus = order.Status;

					if (order.Members && order.Members.length > 0) {
						member.Menus = order.Members[0].Menus;
					}

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

						$scope.$broadcast('clearSearchMemberInput');
						$scope.savedMember = {};
					}
					else if (!isSideDishOrder) {
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

						$scope.$broadcast('clearSearchMemberInput');
						$scope.newMember = {};
					}
					else if (isSideDishOrder) {
						var sideDishMembers = orderingService.getMemberSideDishOrder();

						angular.forEach(sideDishMembers, function (value) {
							if (value.Member.UserId === member.Member.UserId) {
								isExisted = true;
							}
						});

						if (!isExisted) {
							member.Menus = [];
							sideDishMembers.push(member);
							orderingService.setMemberSideDishOrder(sideDishMembers);
							notificationFactory.success("DataChangeSuccessfully");
						}
						else {
							notificationFactory.error('', $scope.warningMessageExistMember);
						}

						$scope.$broadcast('clearSearchMemberInput');
						$scope.sideDishMember = {};
					}
				})
				.error(function (response, status) {
					ErrorFactory.errorHandle(status);
				});
		}
	};
	/*
	** Show alternative menus dropdown.
	*/
	$scope.alter = {};
	$scope.alterSaved = {};

	$scope.getAlternativeMenus = function (member, isSavedOrder) {
		var userId = member.Member.UserId,
			key = '';

		orderingFactory.getAlternativeMenuByUserId(userId).success(function (allMenus) {
			member.AllMenus = allMenus;

			if (isSavedOrder) {
				key = 'alternativeMenu' + userId;
				$scope.alterSaved[key] = true;
			}
			else {
				key = 'alternativeMenu' + userId;
				$scope.alter[key] = true;
			}
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
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

	/*
	** Select side dish
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

orderingModule.controller('SelectSideDishController',
['$scope', 'member', 'orderingFactory', '$modalInstance', 'ErrorFactory', 'notificationFactory',
function ($scope, member, orderingFactory, $modalInstance, ErrorFactor, notificationFactory) {

	$scope.member = member;
	$scope.menus = $scope.member.Menus;

	var updateSideDish = function (menus, sideDishes) {
		angular.forEach(sideDishes, function (sideDish) {
			var sideDishId = sideDish.FoodId;
			sideDish.quantity = 1;

			angular.forEach(menus, function (menu) {
				if (menu.MenuId === sideDishId) {
					sideDish.quantity = parseInt(menu.QuantityOrder);
					sideDish.isOrder = true;
				}
			});
		});
	};

	/*
	** Get side dish by user Id.
	*/
	orderingFactory.getSideDishByUserId($scope.member.Member.UserId).success(function (sideDishes) {
		$scope.sideDishes = sideDishes;
		updateSideDish($scope.menus, $scope.sideDishes);
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Create menus from side dishes.
	*/
	var createMenusFromSideDishes = function (sideDishes) {
		var menus = [];

		angular.forEach(sideDishes, function (sideDish) {
			if (sideDish.isOrder) {
				var menu = {
					MenuId: sideDish.FoodId,
					MenuCode: sideDish.MenuCode,
					NameVn: sideDish.FoodVnmName,
					NameEN: sideDish.FoodEngName,
					Name: sideDish.FoodEngName,
					QuantityOrder: sideDish.quantity,
					TotalAmount: sideDish.quantity * sideDish.Price
				};

				menus.push(menu);
			}
		});

		return menus;
	};

	$scope.save = function () {
		var menus = createMenusFromSideDishes($scope.sideDishes);

		if (typeof menus !== undefined && menus.length > 0) {
			$modalInstance.close(menus);
		}
		else {
			notificationFactory.error("InvalidFoodItem");
		}
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);