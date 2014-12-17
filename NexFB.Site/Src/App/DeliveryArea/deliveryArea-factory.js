'use strict';
/*global deliveryAreaModule*/

deliveryAreaModule.factory('deliveryAreaFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		getAllDeliveryArea: function (deliveryAreaId, isActive, query) {
			return $http.get(baseUrl + 'api/DeliveryArea/GetAllDeliveryArea',
				{ params: { deliveryArea: deliveryAreaId,
					isActive: isActive,
					query: query
				}});
		}
	};
} ]);