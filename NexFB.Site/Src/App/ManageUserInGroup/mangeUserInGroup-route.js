'use strict';
/*exported manageUserInGroupModule*/

var manageUserInGroupModule = angular.module("manageUserInGroupModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/ManageGroup/User', 
        {   
            templateUrl: baseUrl + 'src/app/ManageUserInGroup/Templates/index.html',
            controller: 'ManageUserInGroupController'
		});
} ]);