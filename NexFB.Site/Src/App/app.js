'use strict';
/*exported appModule*/

var appModule = angular.module("appModule", ['registrationMealModule', 'ui.bootstrap'])
.config(['$locationProvider',
function ($locationProvider) {
	$locationProvider.html5Mode(true);
} ]); 