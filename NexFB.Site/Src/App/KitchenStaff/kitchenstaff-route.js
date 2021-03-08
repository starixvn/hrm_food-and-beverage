'use strict';
/*exported kitchenStaffModule*/

var kitchenStaffModule = angular.module("kitchenStaffModule", ['ngRoute'])
    .config(['$routeProvider', function ($routeProvider) {
        var baseUrl = $("base").first().attr("href");
        $routeProvider.when('/KitchenStaff/Order',
            { templateUrl: baseUrl + 'src/app/kitchenStaff/templates/index.html', controller: 'KitchenStaffController' });
    }]);