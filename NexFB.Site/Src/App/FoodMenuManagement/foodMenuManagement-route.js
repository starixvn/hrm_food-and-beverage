'use strict';
/*exported foodMenuManagementModule*/

var foodMenuManagementModule = angular.module("foodMenuManagementModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/FoodMenuManagement/Index',
		{ templateUrl: '/src/app/FoodMenuManagement/templates/index.html', controller: 'FoodMenuManagementController' });
} ]);