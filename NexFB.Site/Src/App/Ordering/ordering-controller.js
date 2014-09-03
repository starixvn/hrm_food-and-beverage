'use strict';
/*global orderingModule*/

orderingModule.controller("OrderingController",
['$scope', '$route', 'orderingFactory', 'notificationFactory', 'ErrorFactory',
	function ($scope, $route, orderingFactory, notificationFactory, ErrorFactory) {
		orderingFactory.getOrderForAddNew()
		.success(function (data) {
			$scope.order = data;
		})
		.error(function () {
			// TODO
		});

		orderingFactory.getLocation()
		.success(function (data) { 
			$scope.locations = data;
		})
		.error(function () { 
			// TODO
		});
	}
]);