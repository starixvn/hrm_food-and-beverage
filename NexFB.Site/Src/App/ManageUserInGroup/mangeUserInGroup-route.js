'use strict';
/*exported manageUserInGroupModule*/

var manageUserInGroupModule = angular.module("manageUserInGroupModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/ManageGroup/User', 
        {   
            templateUrl: '/src/app/ManageUserInGroup/Templates/index.html',
            controller: 'ManageUserInGroupController'
		});
} ]);