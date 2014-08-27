'use strict';
/*exported userControlModule*/

var userControlModule = angular.module("userControlModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/ManageRole/Index',
		{ templateUrl: '/src/app/UserControl/templates/index.html', controller: 'ManageRoleController' });
} ]);