'use strict';
/*exported appModule*/

var appModule = angular.module("appModule",
['mealRegistrationModule', 'userControlModule', 'orderingModule', 'ui.bootstrap', 'pascalprecht.translate',
'multi-select'])
.config(['$locationProvider', '$translateProvider',
function ($locationProvider, $translateProvider) {
	$translateProvider.useLoader('resourceFactory', {});
	$translateProvider.preferredLanguage('en');
	$locationProvider.html5Mode(true);
} ]); 