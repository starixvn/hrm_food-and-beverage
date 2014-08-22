'use strict';
/*exported registrationMealModule*/

var registrationMealModule = angular.module("registrationMealModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/RegistrationMeal/Index',
		{ templateUrl: '/src/app/employeeRegistration/templates/index.tpl.html', controller: 'EmployeeRegistrationMealController' });
} ]);