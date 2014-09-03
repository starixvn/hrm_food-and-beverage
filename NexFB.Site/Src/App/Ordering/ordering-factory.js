'use strict';
/*global orderingModule*/

orderingModule.factory("orderingFactory",
	['$http', function ($http) {
		return {
			getLocation: function () {
				return $http.get('/api/Common/GetAllLocation');
			},
			getOrderForAddNew: function () {
				return $http.get('/api/Ordering/GetAddNew');
			}
		};
	} ]
);