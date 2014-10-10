'use strict';
/*exported alternativeMenuModule*/

var alternativeMenuModule = angular.module("alternativeMenuModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/AlternativeMenuManagement/Index',
		{ templateUrl: '/src/app/AlternativeMenuManagement/templates/index.html', controller: 'AlternativeMenuController' });
} ]);