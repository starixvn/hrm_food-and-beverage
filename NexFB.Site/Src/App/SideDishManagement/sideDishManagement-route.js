'use strict';
/*exported sideDishManagementModule*/

var sideDishManagementModule = angular.module("sideDishManagementModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/SideDishManagement/Index',
		{ templateUrl: baseUrl + 'src/app/SideDishManagement/templates/index.html', controller: 'SideDishManagementController' });
} ]);