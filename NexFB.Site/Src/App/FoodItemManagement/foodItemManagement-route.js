'use strict';
/*exported foodItemManagementModule*/

var foodItemManagementModule = angular.module("foodItemManagementModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/FoodItemManagement/Index',
		{ templateUrl: '/src/app/FoodItemManagement/templates/index.html', controller: 'FoodItemManagementController' });
} ]);