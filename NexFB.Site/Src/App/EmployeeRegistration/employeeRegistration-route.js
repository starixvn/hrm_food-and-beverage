'use strict';
/*exported mealRegistrationModule*/

var mealRegistrationModule = angular.module("mealRegistrationModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/Registration/Index',
		{ templateUrl: '/src/app/employeeRegistration/templates/index.html', controller: 'EmployeeRegistrationMealController' });
} ]);