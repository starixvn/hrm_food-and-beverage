'use strict';
/*exported mealRegistrationModule*/

var mealRegistrationModule = angular.module("mealRegistrationModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/Registration/Index',
		{ templateUrl: baseUrl + 'src/app/employeeRegistration/templates/index.html', controller: 'EmployeeRegistrationMealController' });
} ]);