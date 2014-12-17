'use strict';
/*global deliveryAreaModule*/

deliveryAreaModule.controller('DeliveryAreaController',
['$scope', 'deliveryAreaFactory', 'ErrorFactory',
function ($scope, deliveryAreaFactory, ErrorFactory) {

	deliveryAreaFactory.getAllDeliveryArea(null, true, null).success(function (deliveryAreaList) {
		$scope.deliveryAreaList = deliveryAreaList;
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});
} ]);