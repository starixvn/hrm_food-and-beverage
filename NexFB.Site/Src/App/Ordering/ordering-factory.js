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
			getSavedOrder: function () {
				return $http.get('/api/Ordering/GetSavedOrder');
			},
			getMenu: function () {
				return $http.get('/api/Ordering/GetMenu');
			}
		};
	} ]
);
