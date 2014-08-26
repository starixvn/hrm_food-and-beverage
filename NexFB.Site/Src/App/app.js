'use strict';
/*exported appModule*/

var appModule = angular.module("appModule", ['registrationMealModule', 'userControlModule', 'ui.bootstrap', 'pascalprecht.translate'])
.config(['$locationProvider',
function ($locationProvider) {
	$locationProvider.html5Mode(true);
} ]); 