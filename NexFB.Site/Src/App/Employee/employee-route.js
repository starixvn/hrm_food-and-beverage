'use strict';
/*exported employeeModule*/

var employeeModule = angular.module("employeeModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/SpecialEmployee/Index',
		{ templateUrl: '/src/app/Employee/templates/index.html', controller: 'EmployeeController' });
} ]);