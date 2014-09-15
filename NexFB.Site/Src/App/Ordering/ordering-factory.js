'use strict';
/*global orderingModule*/

orderingModule.factory("orderingFactory",
	['$http', function ($http) {
		return {
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
			getEmployees: function (departmentId, keyWord) {
				return $http.get('/api/Ordering/GetEmployees',
					{
						params: { departmentId: departmentId, keyWord: keyWord }
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
			createOrder: function (order) {
				return $http.post('/api/Ordering', order);
			},
			editOrder: function (order) {
				return $http.put('/api/Ordering', order);
			},
			deleteOrder: function (orderId) {
				return $http['delete']('/api/Ordering', { params: { orderId: orderId} });
			}
		};
	} ]
);
