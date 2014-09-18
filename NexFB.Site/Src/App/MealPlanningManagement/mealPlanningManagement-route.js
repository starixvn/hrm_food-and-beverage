'use strict';
/*exported mealPlanningManagementModule*/

var mealPlanningManagementModule = angular.module("mealPlanningManagementModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/MealPlanningManagement/Index',
		{ templateUrl: '/src/app/MealPlanningManagement/templates/index.html', controller: 'MealPlanningManagementController' });
} ]);