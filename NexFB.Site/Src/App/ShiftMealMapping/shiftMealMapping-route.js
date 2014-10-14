'use strict';
/*exported shiftMealMappingModule*/

var shiftMealMappingModule = angular.module("shiftMealMappingModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/ShiftMealMapping/Index',
		{ templateUrl: '/src/app/ShiftMealMapping/templates/index.html', controller: 'ShiftMealMappingController' });
} ]);