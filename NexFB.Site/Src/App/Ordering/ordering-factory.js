'use strict';
/*global orderingModule*/

orderingModule.factory("orderingFactory",
	['$http', function ($http) {
		return {
			getLocation: function () {
				return $http.get('/api/Common/GetAllLocation');
			},
			getAddNewOrder: function () {
				return $http.get('/api/Ordering/GetAddNew');
			},
			getSavedOrder: function () {
				return $http.get('/api/Ordering/GetSavedOrder');
			}
		};
	} ]
);