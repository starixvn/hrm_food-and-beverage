'use strict';
/*exported setMenuManagementModule*/

var setMenuManagementModule = angular.module("setMenuManagementModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/SetMenuManagement/Index',
		{ templateUrl: '/src/app/SetMenuManagement/templates/index.html', controller: 'SetMenuManagementController' });
} ]);