'use strict';
/*global orderingModule*/

orderingModule.factory("orderingFactory",
	['$http', 'baseUrl', function ($http, baseUrl) {
		return {
			getCurrentUser: function () {
				return $http.get(baseUrl + 'api/Ordering/GetCurrentUser');
			},
			getLocation: function () {
				return $http.get(baseUrl + 'api/Common/GetAllLocation');
			},
			getMyOrder: function () {
				return $http.get(baseUrl + 'api/Ordering/GetMyOrder');
			},
			getAddNewOrder: function () {
				return $http.get(baseUrl + 'api/Ordering/GetAddNew');
			},
			getOrderNewOrder: function (userId) {
				return $http.get(baseUrl + 'api/Ordering/GetOrderNewOrder',
					{
						params: { userId: userId }
					}
				);
			},
			getSavedOrder: function () {
				return $http.get(baseUrl + 'api/Ordering/GetSavedOrder');
			},
			getMyMenu: function () {
				return $http.get(baseUrl + 'api/Ordering/GetMyMenu');
			},
			getDepartment: function () {
				return $http.get(baseUrl + 'api/Common/GetAllDepartment');
			},
			getEmployees: function (keyWord) {
				return $http.get(baseUrl + 'api/Employee/GetEmployees',
					{
						params: { keyWord: keyWord }
					}
				);
			},
			getMenuByUserId: function (userId) {
				return $http.get(baseUrl + 'api/Ordering/GetMenuByUserId',
					{
						params: { userId: userId }
					}
				);
			},
			getOrderById: function (orderId) {
				return $http.get(baseUrl + 'api/Ordering/GetOrderById',
					{
						params: { orderId: orderId }
					}
				);
			},
			addMember: function (member) {
				return $http.post(baseUrl + 'api/OrderMember', member);
				//return $http['addMember']('/api/OrderMember', { params: { memberId: memberId} });
				//AddMember
			},
			removeMember: function (memberId) {
				return $http['delete'](baseUrl + 'api/OrderMember', { params: { memberId: memberId} });
			},
			createOrder: function (order) {
				return $http.post(baseUrl + 'api/Ordering/InsertOrder', order);
			},
			editOrder: function (order) {
				return $http.put(baseUrl + 'api/Ordering/UpdateOrder', order);
			},
			deleteOrder: function (orderId) {
				return $http['delete'](baseUrl + 'api/Ordering', { params: { orderId: orderId} });
			},
			getAlternativeMenuByUserId: function (userId) {
				return $http.get(baseUrl + 'api/Ordering/GetAlternativeMenusByUserId', { params: { userId: userId} });
			},
			getSideDishByUserId: function (userId) {
				return $http.get(baseUrl + 'api/Ordering/GetSideDishByUserId', { params: { userId: userId} });
			},
			createOrderSideDish: function (order) {
				return $http.post(baseUrl + 'api/Ordering/InsertOrderSideDish', order);
			},
			editOrderSideDish: function (order) {
				return $http.put(baseUrl + 'api/Ordering/UpdateOrderSideDish', order);
			},
			checkValidModifiedTime: function (startTime, orderId) {
				return $http.get(baseUrl + 'api/Ordering/CheckValidModifiedTime',
					{ params: { startTime: startTime, orderId: orderId } });
			}
		};
	} ]
);

	orderingModule.factory('orderingUtilities', function () {
		var utilities = {};

		/*
		** Calculate total price of all member in order
		*/
		utilities.calculateTotalAmount = function (members) {
			var total = 0;

			angular.forEach(members, function (member) {
				angular.forEach(member.Menus, function (menu) {
					total += menu.TotalAmount;
				});
			});

			return total;
		};

		utilities.getSelectedMenu = function (currentMenus, allMenus) {
			var resultMenu = '';

			angular.forEach(allMenus, function (menu) {
				if (menu.MenuId === currentMenus.MenuId && menu.IsMainMenu === currentMenus.IsMainMenu) {
					resultMenu = menu;
				}
			});

			return resultMenu;
		};

		utilities.sortOrderIdByDesc = function (order, orders) {
			if (orders.length === 0) {
				orders.push(order);
			}
			else {
				var smallest = true;

				for (var i = 0; i < orders.length; i++) {
					if (order.OrderId > orders[i].OrderId) {
						orders.splice(i, 0, order);
						i = orders.length;
						smallest = false;
					}
				}

				if (smallest) {
					orders.push(order);
				}
			}

			return orders;
		};

		utilities.setDefaultLocation = function (locations, locationId) {
			angular.forEach(locations, function (value) {
				value.ticked = false;

				if (value.LocationId === locationId) {
					value.ticked = true;
				}
			});
		};

		utilities.getOrderLocation = function (typeOrder, scope) {
			var locationId = '';

			// New Order
			if (typeOrder === 0) {
				angular.forEach(scope.newOrderLocations, function (value) {
					if (value.ticked) {
						locationId = value.LocationId;
					}
				});
			}
			// Saved Order
			else if (typeOrder === 1) {
				angular.forEach(scope.savedOrderLocations, function (value) {
					if (value.ticked) {
						locationId = value.LocationId;
					}
				});
			}
			// Side Dish Order
			else if (typeOrder === 2) {
				angular.forEach(scope.sideDishOrderLocations, function (value) {
					if (value.ticked) {
						locationId = value.LocationId;
					}
				});
			}

			return locationId;
		};

		utilities.resetSelectOrderLocation = function (scope) {
			angular.forEach(scope.newOrderLocations, function (value) {
				value.ticked = false;
			});

			angular.forEach(scope.sideDishOrderLocations, function (value) {
				value.ticked = false;
			});
		};

		return utilities;
	});
