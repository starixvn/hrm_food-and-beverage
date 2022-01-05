'use strict';
/*global kitchenStaffModule*/
kitchenStaffModule.controller("KitchenStaffController",
	['$translate', '$scope', '$route', '$timeout', '$modal', 'orderingFactory', 'kitchenStaffFactory', 'orderingService',
		'notificationFactory', 'ErrorFactory', 'staffOrderingUtilities', 'baseUrl', 'commonFactory', 'setMenuFactory', 'foodMenuUtilities',
		function ($translate, $scope, $route, $timeout, $modal, orderingFactory, kitchenStaffFactory, orderingService,
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
				newOrder: 0
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
			$scope.myOrder = [];
			$scope.searchNewMember = '';
			$scope.searchSavedMember = '';
			$scope.searchSideDishMember = '';

			var getEmployee = function (keyword) {
				var allDeparmentIds = "1,2,3,4";
				return kitchenStaffFactory.getEmployees(keyword, allDeparmentIds, null, true);
			};
			var startDate = new Date();
			var onDate = new Date();
			$scope.filter = {
				startDate: new Date(startDate),
				onDate: new Date(onDate)
			};

			$scope.calendar = {
				startDateOpened: false,
				endDateOpened: false
			};

			/*
			** Open calendar.
			*/
			$scope.openPopupStartDate = function ($event) {
				commonFactory.openPopupStartDate($event, $scope);
			};

			$scope.openPopupEndDate = function ($event) {
				commonFactory.openPopupEndDate($event, $scope);
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
				var deliveredOption = [];
				angular.forEach(orderStatuses, function (status) {
					if (status.Value === $scope.orderStatus.Delivered) {
						status.ticked = true;
						deliveredOption.push(status);
					}
				});
				$scope.orderStatusesOptions = deliveredOption;

			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});

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

					data = staffOrderingUtilities.sortOrderIdByDesc(order, data);
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

					data = staffOrderingUtilities.sortOrderIdByDesc(order, data);

					// Check user can continue order or not
					orderingFactory.getAddNewOrder()
						.success(function (data) {
							if (data.RawIsAllowOrder) {
								data.Members = [];
								$scope.getDefaultForAddOrder(data);
							}
						})
						.error(function (response, status) {
							ErrorFactory.errorHandle(status);
						});

					kitchenStaffFactory.getStaffOrder($scope.filter.onDate.toDateString())
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


				var getOrder = function (data) {
					var newOrder = angular.copy(data);

					getNewOrder(newOrder);
				};

				$scope.getDefaultForAddOrder = function (newOrderData) {
					if (!newOrderData) {
						orderingFactory.getAddNewOrder()
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

				kitchenStaffFactory.getStaffOrder($scope.filter.onDate.toDateString())
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

				orderingFactory.getLocation()
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


				$scope.saveAddOrder = function (order, typeOrder, date) {
					if (order.Location !== null) {
						order.Location.LocationId = staffOrderingUtilities.getOrderLocation(typeOrder, $scope);
					}

					var selectedStatusId = staffOrderingUtilities.getStatusValue($scope.orderStatusesOptions);
					order.StatusId = selectedStatusId;
					order.OnDate = date.toDateString();
					order.MealID = foodMenuUtilities.getMealId($scope.meals);


					if ($scope.ValidateNewOrder(order)) {
						orderingFactory.checkDeliveryAreaExist(order.Location.LocationId).success(function (isExisted) {
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
												kitchenStaffFactory.getStaffOrder(onDate.toDateString())
													.success(function (data) {
														angular.forEach(data, function (order) {
															order.Remarks = order.Remark ? order.Remark.split(",") : [];
														});

														$scope.myOrder = data;
													});

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
									orderingFactory.resetSelectOrderLocation($scope);
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

				$scope.queryOrder = function (onDate) {
					kitchenStaffFactory.getStaffOrder(onDate.toDateString())
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
							templateUrl: baseUrl + 'src/app/KitchenStaff/Templates/EditOrder.html',
							controller: "EditStaffOrderController",
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

						if (mealId === "" || mealId === null) {
							notificationFactory.error('', $scope.foodMenuMealRequire);
						}
						else {
							kitchenStaffFactory.getNewOrderByStaff(userId, mealId, onDate)
								.success(function (order) {
									member.CurrentStatus = order.Status;

									if (order.Members && order.Members.length > 0) {
										member.Menus = order.Members[0].Menus;
										member.IsAllowOrder = order.Members[0].IsAllowOrder;
									}

									var isExisted = false;
									var isNotSameMenu = false;

									if (member.Menus && member.Menus[0].MenuId === -1) {
										var memberNonMenuMessage = '<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + member.Member.NickName,
											message = $scope.noMenuAvailableWarning + memberNonMenuMessage;
										notificationFactory.error("", message);
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
												if (value.Menus[0].Name !== member.Menus[0].Name) {
													isNotSameMenu = true;
												}
											});

											if (isNotSameMenu) {
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

		}]);