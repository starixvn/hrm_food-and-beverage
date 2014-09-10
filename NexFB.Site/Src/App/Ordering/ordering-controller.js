'use strict';
/*global orderingModule*/

orderingModule.controller("OrderingController",
['$scope', '$route', '$timeout', 'orderingFactory',
function ($scope, $route, $timeout, orderingFactory) {

	//	$scope.counter = 0;
	//	$scope.onTimeout = function () {
	//		$scope.counter++;
	//		mytimeout = $timeout($scope.onTimeout, 1000);
	//		//chat.server.sendMessage('aaaa');
	//	}
	//	var mytimeout = $timeout($scope.onTimeout, 1000);

	$.connection.hub.start();
	var chat = $.connection.chat;
	//chat.hubs.start();
	$scope.messages = [];
	$scope.sendMessage = function () {
		//signalRSvc.sendRequest();
		chat.server.sendMessage($scope.newMessage);
		$scope.newMessage = "";
	};


	//	$scope.$parent.$on("newMessage", function (e, message) {
	//		$scope.messages.push({ message: message });
	//		$scope.$apply();
	//	});

	chat.client.newMessage = function (message) {
		$scope.messages.push({ message: message });
		$scope.$apply();
		console.log(message);
	};

	orderingFactory.getMyOrder()
		.success(function (data) {
			$scope.myOrder = data;
		})
		.error(function () {
			// TODO
		});

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

	orderingFactory.getMenu()
		.success(function (data) {
			$scope.menu = data;
		})
		.error(function () {
			// TODO
		});
} ]);