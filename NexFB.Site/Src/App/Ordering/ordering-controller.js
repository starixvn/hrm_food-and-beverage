'use strict';
/*global orderingModule*/

orderingModule.controller("OrderingController",
['$scope', '$route', 'orderingFactory',
function ($scope, $route, orderingFactory) {
	orderingFactory.getAddNewOrder()
	.success(function (data) {
		$scope.newOrder = data;
	})
	.error(function () {
		// TODO
	});

	orderingFactory.getSavedOrder()
	.success(function (data) {
		$scope.savedOrder = data;
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
} ]);