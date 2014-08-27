'use strict';
/*exported appModule*/

var appModule = angular.module("appModule", ['registrationMealModule', 'userControlModule', 'ui.bootstrap', 'pascalprecht.translate'])
.config(['$locationProvider', '$translateProvider',
function ($locationProvider, $translateProvider) {
	$translateProvider.useLoader('resourceFactory', {});
	$translateProvider.preferredLanguage('en');
	$locationProvider.html5Mode(true);
} ]); 