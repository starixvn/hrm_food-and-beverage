'use strict';
/*exported foodItemManagementModule*/

var foodItemManagementModule = angular.module("foodItemManagementModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/FoodItemManagement/Index',
		{ templateUrl: baseUrl + 'src/app/FoodItemManagement/templates/index.html', controller: 'FoodItemManagementController' });
} ]);