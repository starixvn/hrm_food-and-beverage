'use strict';
/*global alternativeMenuModule*/

alternativeMenuModule.controller('AlternativeMenuController',
['$scope', 'alternativeMenuFactory', 'ErrorFactory',
function ($scope, alternativeMenuFactory, ErrorFactory) {

	/*
	** Get all alternative menu.
	*/
	var getAllAlternativeMenu = function (alternativeMenuId, isActive) {
		alternativeMenuFactory.getAllAlternativeMenu(alternativeMenuId, isActive)
		.success(function (alternativeMenus) {
			$scope.alternativeMenus = alternativeMenus;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	getAllAlternativeMenu(null, null);
} ]);