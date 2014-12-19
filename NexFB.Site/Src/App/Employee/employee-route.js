'use strict';
/*exported employeeModule*/

var employeeModule = angular.module("employeeModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/SpecialEmployee/Index',
			{ templateUrl: baseUrl + 'src/app/Employee/templates/index.html', controller: 'EmployeeController' });
} ]);