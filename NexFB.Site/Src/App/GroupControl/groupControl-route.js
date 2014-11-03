'use strict';
/*exported groupControlModule*/

var groupControlModule = angular.module("groupControlModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/ManageGroup/Index',
		{ templateUrl: '/src/app/GroupControl/templates/ManageGroup/index.html', 
		  controller: 'ManageGroupController' });
} ]);