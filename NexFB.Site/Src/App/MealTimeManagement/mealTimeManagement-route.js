'use strict';
/*exported mealTimeManagementModule*/

var mealTimeManagementModule = angular.module("mealTimeManagementModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/MealTimeManagement/Index',
		{ templateUrl: baseUrl + 'src/app/MealTimeManagement/templates/index.html', controller: 'MealTimeManagementController' });
} ]);