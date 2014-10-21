'use strict';
/*global orderingModule*/

orderingModule.factory("orderingFactory",
	['$http', function ($http) {
		return {
			getCurrentUser: function () {
				return $http.get('/api/Ordering/GetCurrentUser');
			},
			getLocation: function () {
				return $http.get('/api/Common/GetAllLocation');
			},
			getMyOrder: function () {
				return $http.get('/api/Ordering/GetMyOrder');
			},
			getAddNewOrder: function () {
				return $http.get('/api/Ordering/GetAddNew');
			},
			getOrderByUserId: function (userId) {
				return $http.get('/api/Ordering/GetOrderByUserId',
					{
						params: { userId: userId }
					}
				);
			},
			getSavedOrder: function () {
				return $http.get('/api/Ordering/GetSavedOrder');
			},
			getMyMenu: function () {
				return $http.get('/api/Ordering/GetMyMenu');
			},
			getDepartment: function () {
				return $http.get('/api/Common/GetAllDepartment');
			},
			getEmployees: function (keyWord) {
				return $http.get('/api/Ordering/GetEmployees',
					{
						params: { keyWord: keyWord }
					}
				);
			},
			getMenuByUserId: function (userId) {
				return $http.get('/api/Ordering/GetMenuByUserId',
					{
						params: { userId: userId }
					}
				);
			},
			getOrderById: function (orderId) {
				return $http.get('/api/Ordering/GetOrderById',
					{
						params: { orderId: orderId }
					}
				);
			},
			addMember: function (member) {
				return $http.post('/api/OrderMember', member);
				//return $http['addMember']('/api/OrderMember', { params: { memberId: memberId} });
				//AddMember
			},
			removeMember: function (memberId) {
				return $http['delete']('/api/OrderMember', { params: { memberId: memberId} });
			},
			createOrder: function (order) {
				return $http.post('/api/Ordering/InsertOrder', order);
			},
			editOrder: function (order) {
				return $http.put('/api/Ordering', order);
			},
			deleteOrder: function (orderId) {
				return $http['delete']('/api/Ordering', { params: { orderId: orderId} });
			},
			getAlternativeMenuByUserId: function (userId) {
				return $http.get('/api/Ordering/GetAlternativeMenusByUserId', { params: { userId: userId} });
			},
			getSideDishByUserId: function (userId) {
				return $http.get('/api/Ordering/GetSideDishByUserId', { params: { userId: userId } });
			},
			createOrderSideDish: function (order) {
				return $http.post('/api/Ordering/InsertOrderSideDish', order);
			}
		};
	} ]
);
