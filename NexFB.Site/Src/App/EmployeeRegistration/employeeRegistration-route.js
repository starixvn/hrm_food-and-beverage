'use strict';
/*exported mealRegistrationModule*/

var mealRegistrationModule = angular.module("mealRegistrationModule", ['ngRoute'])
.config(['$routeProvider', '$provide', function ($routeProvider, $provide) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when(baseUrl + 'Registration/Index',
		{ templateUrl: baseUrl + 'src/app/employeeRegistration/templates/index.html', controller: 'EmployeeRegistrationMealController' });
} ]);