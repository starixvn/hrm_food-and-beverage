'use strict';
/*exported orderingModule*/

var orderingModule = angular.module("orderingModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/Ordering/Create',
		{ templateUrl: '/src/app/ordering/templates/create.html', controller: 'OrderingController' });
} ]);