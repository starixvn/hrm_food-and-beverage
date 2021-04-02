'use strict';
/*global kitchenStaffModule*/
kitchenStaffModule.controller("KitchenStaffController",
    ['$translate', '$scope', '$route', '$timeout', '$modal', 'kitchenStaffFactory', 'orderingService',
        'notificationFactory', 'ErrorFactory', 'staffOrderingUtilities', 'baseUrl', 'commonFactory', 'setMenuFactory','foodMenuUtilities',
        function ($translate, $scope, $route, $timeout, $modal, kitchenStaffFactory, orderingService,
            notificationFactory, ErrorFactory, staffOrderingUtilities, baseUrl, commonFactory, setMenuFactory, foodMenuUtilities) {
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


		$translate('NoMenuAvailableWarning').then(function (message) {
			$scope.noMenuAvailableWarning = message;
		});

		$translate('OverOrderWarning').then(function (message) {
			$scope.overOrderWarning = message;
		});
		$translate('FoodMenuMealRequire').then(function (message) {
			$scope.foodMenuMealRequire = message;
		});
		$translate('MenusNotTheSame').then(function (message) {
			$scope.menusNotTheSame = message;
		});
	};

	/*
	** Declare variables.
	*/
	$scope.typeOrder = {
		newOrder: 0,
		savedOrder: 1,
		sideDishOrder: 2
	};

	$scope.orderStatus = {
		New: 1,
		Accepted: 2,
		Reduced: 3,
		Delivered: 4,
		Edited: 5,
		Rejected: 6
	};

	$scope.getMessage();
	$scope.newOrder = [];
	$scope.savedOrder = [];
	$scope.myOrder = [];
	$scope.searchNewMember = '';
	$scope.searchSavedMember = '';
	$scope.searchSideDishMember = '';

	var getEmployee = function (keyword) {
		return kitchenStaffFactory.getEmployees(keyword);
            };
    var startDate = new Date();
    $scope.filter = {
        startDate: new Date(startDate),
    };        

    $scope.calendar = {
        startDateOpened: false
        };

    /*
    ** Open calendar.
    */
    $scope.openPopupStartDate = function ($event) {
        commonFactory.openPopupStartDate($event, $scope);
    };


	kitchenStaffFactory.getAllMealTimesheet().success(function (meals) {
		$scope.meals = meals;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get Order Status.
	*/
	kitchenStaffFactory.getOrderStatusEnum().success(function (orderStatuses) {
		$scope.orderStatusesOptions = orderStatuses;
		setDefaultStatus($scope.orderStatusesOptions);
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	var setDefaultStatus = function (orderStatuses) {
		angular.forEach(orderStatuses, function (status) {
			if (status.Value === $scope.orderStatus.Delivered ) {
				status.ticked = true;
			}
		});
	};

    /*
    ** Get Set Menu bu Menu Id.
    */
	$scope.getSetMenu = function (menuId) {
		if (menuId) {
		angular.forEach($scope.menuOptions, function (value) {
			if (value.MenuId === menuId) {
				$scope.MenuEngName = value.MenuEngName;
			}
		});
		$scope.menuId = menuId;
		setMenuFactory.getSetMenu(menuId, true).success(function (setMenus) {
			$scope.setMenuItems = setMenus;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
		}
		else {
			$scope.setMenuItems = [];
			$scope.mealPlanning.setMenuId = '';
		}
	};

	$scope.getNewEmployee = function () {
		return getEmployee($scope.searchNewMember);
	};

	$scope.unSelectNewEmployee = function () {
		if ($scope.searchNewMember.trim() === '') {
			if ($scope.newMember && $scope.newMember.employee) {
				$scope.newMember.employee = null;
			}
		}
	};

	$scope.getSavedEmployee = function () {
		return getEmployee($scope.searchSavedMember);
	};

	$scope.unSelectSavedEmployee = function () {
		if ($scope.searchSavedMember.trim() === '') {
			if ($scope.savedMember && $scope.savedMember.employee) {
				$scope.savedMember.employee = null;
			}
		}
	};

	$scope.getSideDishEmployee = function () {
		return getEmployee($scope.searchSideDishMember);
	};

	$scope.unSelectSideDishEmployee = function () {
		if ($scope.searchSideDishMember.trim() === '') {
			if ($scope.sideDishMember && $scope.sideDishMember.employee) {
				$scope.sideDishMember.employee = null;
			}
		}
	};

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
					order.CurrentMenus = value.Menus;
				}
			});
		}

		if (isRelatedCurrentUser) {
			var data = orderingService.getMyOrder();
			order.Remarks = order.Remark ? order.Remark.split(",") : [];

			if (order.TakeAway) {
				if (order.Remarks.length !== 0) {
					order.Remarks.splice(0, 0, $scope.takeAway);
				}
				else {
					order.Remarks.push($scope.takeAway);
				}
			}

			data = kitchenStaffFactory.sortOrderIdByDesc(order, data);
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
			if (order.Status.StatusId === $scope.orderStatus.New && order.Members.length === 0) {
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
					order.CurrentMenus = value.Menus;
				}
			});
		}

		if (isRelatedCurrentUser) {
			order.Remarks = order.Remark ? order.Remark.split(",") : [];

			if (order.TakeAway) {
				if (order.Remarks.length !== 0) {
					order.Remarks.splice(0, 0, $scope.takeAway);
				}
				else {
					order.Remarks.push($scope.takeAway);
				}
			}

			data = kitchenStaffFactory.sortOrderIdByDesc(order, data);

			// Check user can continue order or not
			kitchenStaffFactory.getAddNewOrder()
			.success(function (data) {
                if (data.RawIsAllowOrder) {
                    data.Members = [];
					$scope.getDefaultForAddOrder(data);
				}
			})
			.error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});

			kitchenStaffFactory.getMyOrder()
			.success(function (data) {
				angular.forEach(data, function (order) {
					order.Remarks = order.Remark ? order.Remark.split(",") : [];

					if (order.TakeAway) {
						if (order.Remarks.length !== 0) {
							order.Remarks.splice(0, 0, $scope.takeAway);
						}
						else {
							order.Remarks.push($scope.takeAway);
						}
					}
				});

				orderingService.setMyOrder(data);
				$scope.myOrder = data;
			})
			.error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		}

//		orderingService.setMyOrder(data);
//		$scope.$apply();
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

	$.connection.hub.start();

	$scope.$watch(function () {
		return orderingService.getMyOrder();
	}, function (value) {
		$scope.myOrder = value;
	});


	// close Saved Order and Side Dish Order when don't have New Order
	$scope.$watch('newOrder.Status', function (newValue) {
		if (newValue === null) {
			$scope.$broadcast("closeOrderContent");
		}
	});

	$scope.loadData = function (callback) {
		kitchenStaffFactory.getCurrentUser()
		.success(function (data) {
			$scope.currentUser = data;
			orderingService.setCurrentUser(data);
		})
		.error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});

		kitchenStaffFactory.getMyMenu()
		.success(function (data) {
			$scope.menu = data;
		})
		.error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});

		var getNewOrder = function (data) {
			var newMembers = [];

			angular.forEach(data.Members, function (member) {
				// new order
				if (member.IsAllowOrder || member.Menus[0].MenuId === -1) {
					member.IsOrder = true;
					newMembers.push(member);
				}
			});

			data.Members = newMembers;
			data.TakeAway = false;

			$scope.newOrder = data;
			orderingService.setMemberNewOrder(data.Members);
		};

		var getSideDishOrder = function (data) {
			var newMembers = [];

			if (data.Members != null && data.Members.length > 0) {
				var member = data.Members[0];
				member.IsOrder = true;
				member.Menus = [];
				newMembers.push(member);
			}

			data.TakeAway = false;
			data.Members = newMembers;
			$scope.sideDishOrder = data;
			orderingService.setMemberSideDishOrder(data.Members);
		};

		var getOrder = function (data) {
			var newOrder = angular.copy(data),
				sideDishOrder = angular.copy(data);

			getNewOrder(newOrder);
			getSideDishOrder(sideDishOrder);
		};

		$scope.getDefaultForAddOrder = function (newOrderData) {
			if (!newOrderData) {
				kitchenStaffFactory.getAddNewOrder()
				.success(function (data) {
                    data.Members = [];
					getOrder(data);
				})
				.error(function (response, status) {
					ErrorFactory.errorHandle(status);
				});
			}
			else {
				getOrder(newOrderData);
			}
		};

		kitchenStaffFactory.getMyOrder()
			.success(function (data) {
				angular.forEach(data, function (order) {
					order.Remarks = order.Remark ? order.Remark.split(",") : [];

					if (order.TakeAway) {
						if (order.Remarks.length !== 0) {
							order.Remarks.splice(0, 0, $scope.takeAway);
						}
						else {
							order.Remarks.push($scope.takeAway);
						}
					}
				});

				orderingService.setMyOrder(data);
				$scope.myOrder = data;
			})
			.error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});

		kitchenStaffFactory.getLocation()
			.success(function (data) {
				angular.forEach(data, function (value) {
					if (value.LocationId === 31) {
						value.ticked = true
					}
				});
				$scope.locations = data;
				$scope.newOrderLocations = angular.copy(data);
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

			if ($scope.modalConfirmAlternativeMenuInstance) {
				$scope.modalConfirmAlternativeMenuInstance.dismiss('cancel');
			}

			if ($scope.modalInstance) {
				$scope.modalInstance.dismiss('cancel');
			}
		});

		$scope.ValidateNewOrder = function (order) {
			var isValid = true;

			if (order.Members.length < 1) {
				isValid = false;
				notificationFactory.error("InvalidInputData");
			}
			else {
				var overOrderMessage = '';
				var numberMembersOrder = 0;

				for (var i = 0; i < order.Members.length; i++) {
					var value = order.Members[i];

					if (value.IsOrder) {
						if (value.IsAllowOrder) {
							numberMembersOrder += 1;
							isValid = true;
						}
						else if (value.Menus[0].MenuId !== -1) {
							overOrderMessage += '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + value.Member.NickName;
						}
					}
				}

				if (numberMembersOrder === 0) {
					isValid = false;
					notificationFactory.error("InvalidInputData");
				}
				if (overOrderMessage) {
					isValid = false;
					overOrderMessage = $scope.overOrderWarning + overOrderMessage;
					notificationFactory.error("", overOrderMessage);
				}
			}

			if (!order.Location || !order.Location.LocationId) {
				isValid = false;
				notificationFactory.error("InvalidLocation");
			}

			var memberNonMenuMessage = '';

			angular.forEach(order.Members, function (member) {
				angular.forEach(member.Menus, function (menu) {
					if (menu.MenuId === -1 && member.IsOrder) {
						memberNonMenuMessage += '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + member.Member.NickName;
					}
				});
			});

			if (memberNonMenuMessage) {
				var message = $scope.noMenuAvailableWarning + memberNonMenuMessage;
				isValid = false;
				notificationFactory.error("", message);
			}

			return isValid;
		};


		$scope.saveAddOrder = function (order, typeOrder) {
			if (order.Location !== null) {
				order.Location.LocationId = staffOrderingUtilities.getOrderLocation(typeOrder, $scope);
			}

			var selectedStatusId = staffOrderingUtilities.getStatusValue($scope.orderStatusesOptions);
			order.StatusId = selectedStatusId;

			if ($scope.ValidateNewOrder(order)) {
				kitchenStaffFactory.checkDeliveryAreaExist(order.Location.LocationId).success(function (isExisted) {
					if (isExisted === 'true') {
						$scope.modalConfirmSaveOrderInstance = $modal.open({
							templateUrl: baseUrl + 'Src/Components/Extensions/Common/Templates/ModalConfirmContent2.html',
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
							kitchenStaffFactory.createOrder(order)
								.success(function (response) {
							if (response.result.IsSuccessful) {
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
						}, function () {
						})['finally'](function () {
							kitchenStaffFactory.resetSelectOrderLocation($scope);
							$scope.modalConfirmSaveOrderInstance = undefined;
						});
					}
					else {
						notificationFactory.error('DeliveryAreaNotExistError');
					}
				}).error(function (response, status) {
					ErrorFactory.errorHandle(status);
				});
			}
		};

		$scope.removeMember = function (index, memberId, typeOrder) {
			$scope.modalRemoveMemberInstance = $modal.open({
				templateUrl: baseUrl + 'Src/Components/Extensions/Common/Templates/ModalConfirmContent2.html',
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
				if (typeOrder === $scope.typeOrder.newOrder) {
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
					templateUrl: baseUrl + 'src/app/Ordering/Templates/EditOrder.html',
					controller: "EditOrderController",
					backdrop: 'static',
					resolve: {
						orderId: function () {
							return orderId;
						},
						locations: function () {
							return angular.copy($scope.locations);
						},
						startTime: function () {
							return new Date();
						}
					}
				});

				$scope.modalEditOrderInstance.result.then(function (receiveObject) {
					if (receiveObject.isDeleted) {
						$scope.sendDeleteOrder(receiveObject.orderId);
						$scope.getDefaultForAddOrder();
					}
					else {
						$scope.sendEditOrder(receiveObject.order.OrderId);
					}
				}, function () {
				})['finally'](function () {
					$scope.modalEditOrderInstance = undefined;
				});
			}
		};
	};


    $scope.add = function (filter) {
        if (filter) {
            var mealId = foodMenuUtilities.getMealId($scope.meals);
            var onDate = filter.startDate.toDateString();
            var newMember = $scope.newMember;
            if (newMember && newMember.employee) {
                var member = {
                    Member: '',
                    Menus: '',
                    AllMenus: '',
                    IsOrder: true,
                    IsAllowOrder: false
                };
                member.Member = newMember.employee;
                var userId = member.Member.UserId;

				if(mealId === "" || mealId === null)
				{
					notificationFactory.error('', $scope.foodMenuMealRequire);
				}
				else
				{
					kitchenStaffFactory.getOrderNewOrderByStaff(userId, mealId, onDate)
                    .success(function (order) {
                        member.CurrentStatus = order.Status;

                        if (order.Members && order.Members.length > 0) {
                            member.Menus = order.Members[0].Menus;
                            member.IsAllowOrder = order.Members[0].IsAllowOrder;
                        }

                        var isExisted = false;
						var isNotSameMenu = false;

						if (!member.IsAllowOrder) {
							if (member.Menus && member.Menus[0].MenuId === -1) {
								var memberNonMenuMessage = '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + member.Member.NickName,
									message = $scope.noMenuAvailableWarning + memberNonMenuMessage;
								notificationFactory.error("", message);
							}
							else {
								notificationFactory.error("AddOverOrderMemberWarning");
							}
						}
						else {
							// User has order or user has order but change menu
							if (order.Members.length === 0) {
								notificationFactory.error('', $scope.warningMessageExistMember);
							}
							else {
								var members = orderingService.getMemberNewOrder();

								angular.forEach(members, function (value) {
									if (value.Member.UserId === member.Member.UserId) {
										isExisted = true;
									}
									if(value.Menus[0].Name !== member.Menus[0].Name)
									{
										isNotSameMenu = true;
									}
								});

								if(isNotSameMenu)
								{
									notificationFactory.error('', $scope.menusNotTheSame);
								}
								else if (!isExisted) {
									members.push(member);
									orderingService.setMemberNewOrder(members);
									notificationFactory.success("DataChangeSuccessfully");
								}
								else {
									notificationFactory.error('', $scope.warningMessageExistMember);
								}
							}
						}

						$scope.$broadcast('clearSearchMemberInput');
						$scope.newMember = {};
                    })
                    .error(function (response, status) {
                        ErrorFactory.errorHandle(status);
                    });
					}
				}
        }
	};
	

	$scope.showMember = function (order) {
		order.IsShowMember = true;
	};

	$scope.hideMember = function (order) {
		order.IsShowMember = false;
	};

	$scope.showNameViet = function (menu, order) {
		var key = 'IsShowNameViet' + menu.MenuId;
		order[key] = true;
	};

	$scope.hideNameViet = function (menu, order) {
		var key = 'IsShowNameViet' + menu.MenuId;
		order[key] = false;
	};

	$scope.loadData(function () {
		$scope.bindAction();
	});

} ]);

kitchenStaffModule.controller('SelectSideDishController',
['$scope', 'member', 'kitchenStaffFactory', '$modalInstance', 'ErrorFactory', 'notificationFactory',
function ($scope, member, kitchenStaffFactory, $modalInstance, ErrorFactory, notificationFactory) {

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
	kitchenStaffFactory.getSideDishByUserId($scope.member.Member.UserId).success(function (sideDishes) {
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