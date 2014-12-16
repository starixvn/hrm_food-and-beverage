'use strict';
/*exported shiftMealMappingModule*/

var shiftMealMappingModule = angular.module("shiftMealMappingModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/ShiftMealMapping/Index',
		{ templateUrl: baseUrl + 'src/app/ShiftMealMapping/templates/index.html', controller: 'ShiftMealMappingController' });
} ]);