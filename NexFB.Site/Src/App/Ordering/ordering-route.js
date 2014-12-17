'use strict';
/*exported orderingModule*/

var orderingModule = angular.module("orderingModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/Ordering/Index',
		{ templateUrl: baseUrl + 'src/app/ordering/templates/index.html', controller: 'OrderingController' });
	$routeProvider.when(baseUrl,
		{ templateUrl: baseUrl + 'src/app/ordering/templates/index.html', controller: 'OrderingController' });
} ]);