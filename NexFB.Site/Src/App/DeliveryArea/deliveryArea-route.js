'use strict';
/*exported deliveryAreaModule*/

var deliveryAreaModule = angular.module("deliveryAreaModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/DeliveryArea/Index',
		{ templateUrl: baseUrl + 'src/app/DeliveryArea/templates/index.html', controller: 'DeliveryAreaController' });
} ]);