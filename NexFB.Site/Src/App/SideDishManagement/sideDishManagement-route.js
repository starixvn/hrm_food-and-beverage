'use strict';
/*exported sideDishManagementModule*/

var sideDishManagementModule = angular.module("sideDishManagementModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/SideDishManagement/Index',
		{ templateUrl: '/src/app/SideDishManagement/templates/index.html', controller: 'SideDishManagementController' });
} ]);