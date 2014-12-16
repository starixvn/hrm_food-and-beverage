'use strict';
/*exported mealPlanningManagementModule*/

var mealPlanningManagementModule = angular.module("mealPlanningManagementModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/MealPlanningManagement/Index',
		{ templateUrl: baseUrl + 'src/app/MealPlanningManagement/templates/index.html', controller: 'MealPlanningManagementController' });
} ]);