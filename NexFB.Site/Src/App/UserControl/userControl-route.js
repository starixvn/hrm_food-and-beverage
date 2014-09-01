'use strict';
/*exported userControlModule*/

var userControlModule = angular.module("userControlModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/ManageRole/Index',
		{ templateUrl: '/src/app/UserControl/templates/RolePermission/index.html', controller: 'ManageRoleController' });
	$routeProvider.when('/ManageUser/Index',
		{ templateUrl: '/src/app/UserControl/templates/UserPermission/index.html', controller: 'ManageUserController' });
} ]);