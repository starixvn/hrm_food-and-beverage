'use strict';
/*exported setMenuManagementModule*/

var setMenuManagementModule = angular.module("setMenuManagementModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/SetMenuManagement/Index',
		{ templateUrl: baseUrl + 'src/app/SetMenuManagement/templates/index.html', controller: 'SetMenuManagementController' });
} ]);