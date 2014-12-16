'use strict';
/*exported foodMenuManagementModule*/

var foodMenuManagementModule = angular.module("foodMenuManagementModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/MenuManagement/Index',
		{ templateUrl: baseUrl + 'src/app/FoodMenuManagement/templates/index.html', controller: 'FoodMenuManagementController' });
} ]);