'use strict';
/*exported alternativeMenuModule*/

var alternativeMenuModule = angular.module("alternativeMenuModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/AlternativeMenuManagement/Index',
		{ templateUrl: baseUrl + 'src/app/AlternativeMenuManagement/templates/index.html', controller: 'AlternativeMenuController' });
} ]);