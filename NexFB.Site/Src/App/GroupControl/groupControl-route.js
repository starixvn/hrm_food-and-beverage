'use strict';
/*exported groupControlModule*/

var groupControlModule = angular.module("groupControlModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/ManageGroup/Index', { 
        templateUrl: baseUrl + 'src/app/GroupControl/templates/index.html',
        controller: 'ManageGroupController' });
}]);