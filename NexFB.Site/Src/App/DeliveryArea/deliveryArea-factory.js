'use strict';
/*global deliveryAreaModule*/

deliveryAreaModule.factory('deliveryAreaFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		getAllDeliveryArea: function (deliveryAreaId, isActive, query) {
			return $http.get(baseUrl + 'api/DeliveryArea/GetAllDeliveryArea',
				{ params: { deliveryArea: deliveryAreaId,
					isActive: isActive,
					query: query
				}
				});
		},
		insertDeliveryArea: function (deliveryArea) {
			return $http.post(baseUrl + 'api/DeliveryArea/InsertDeliveryArea', deliveryArea);
		},
		updateDeliveryArea: function (deliveryArea) {
			return $http.put(baseUrl + 'api/DeliveryArea/UpdateDeliveryArea', deliveryArea);
		},
		activateDeliveryArea: function (deliveryArea) {
			return $http.put(baseUrl + 'api/DeliveryArea/ActivateDeliveryArea', deliveryArea);
		},
		deleteDeliveryArea: function (locationId) {
			return $http["delete"](baseUrl + 'api/DeliveryArea/DeleteDeliveryArea',
				{ params: { locationId: locationId} });
		},
		getUserPermissionOnDeliveryArea: function () {
			return $http.get(baseUrl + 'api/DeliveryArea/GetUserPermissionOnDeliveryArea');
		},
		checkDeliveryAreaExistOrderMeal: function (deliveryAreaId, statusId) {
			return $http.get(baseUrl + 'api/DeliveryArea/CheckDeliveryAreaExistOrderMeal',
				{ params: { deliveryAreaId: deliveryAreaId, statusId: statusId} });
		}
	};
} ]);

deliveryAreaModule.factory('deliveryAreaUtilities', ['notificationFactory', function (notificationFactory) {
	var utilities = {};

	utilities.validateDeliveryArea = function (deliveryArea) {
		var isValid = true;

		if (!deliveryArea) {
			isValid = false;
		}
		else {
			if (!deliveryArea.FloorName) {
				isValid = false;
				notificationFactory.error("FloorRequire");
			}
			if (!deliveryArea.NameEN) {
				isValid = false;
				notificationFactory.error("DeliveryAreaEngRequire");
			}
			if (!deliveryArea.NameVN) {
				isValid = false;
				notificationFactory.error("DeliveryAreaVietRequire");
			}
		}

		return isValid;
	};

	return utilities;
} ]);