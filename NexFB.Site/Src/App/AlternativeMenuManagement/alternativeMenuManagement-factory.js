'use strict';
/*global alternativeMenuModule*/

alternativeMenuModule.factory('alternativeMenuFactory', ['$http', function ($http) {
	return {
		getAllAlternativeMenu: function (alternativeMenuId, isActive) {
			return $http.get('/api/AlternativeMenu/GetAllAlternativeMenu', 
				{ params: { alternativeMenu: alternativeMenuId, isActive: isActive } });
		}
	};
} ]);