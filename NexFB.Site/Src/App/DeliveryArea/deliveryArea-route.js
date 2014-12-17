'use strict';
/*exported deliveryAreaModule*/

var deliveryAreaModule = angular.module("deliveryAreaModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/DeliveryArea/Index',
		{ templateUrl: '/src/app/DeliveryArea/templates/index.html', controller: 'DeliveryAreaController' });
} ]);