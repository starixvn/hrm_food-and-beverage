'use strict';
/*exported userControlModule*/

var userControlModule = angular.module("userControlModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/ManageRole/Index',
		{ templateUrl: baseUrl + 'src/app/UserControl/templates/RolePermission/index.html', controller: 'ManageRoleController' });
	$routeProvider.when('/ManageUser/Index',
		{ templateUrl: baseUrl + 'src/app/UserControl/templates/UserPermission/index.html', controller: 'ManageUserController' });
} ]);