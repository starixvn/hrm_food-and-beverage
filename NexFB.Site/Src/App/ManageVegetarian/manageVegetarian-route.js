'use strict';
/*exported manageVegetarianModule*/

var manageVegetarianModule = angular.module("manageVegetarianModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/ManageVegetarian/Index',
		{ templateUrl: baseUrl + 'src/app/manageVegetarian/templates/index.html', controller: 'ManageVegetarianController' });
} ]);