'use strict';
/*exported orderingModule*/

var orderingModule = angular.module("orderingModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/Ordering/Index',
		{ templateUrl: '/src/app/ordering/templates/index.html', controller: 'OrderingController' });
} ]);