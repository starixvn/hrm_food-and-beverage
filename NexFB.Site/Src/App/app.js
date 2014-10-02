'use strict';
/*exported appModule*/

var appModule = angular.module("appModule",
['mealRegistrationModule', 'userControlModule', 'orderingModule', 'foodItemManagementModule',
'foodMenuManagementModule', 'mealPlanningManagementModule',
'ui.bootstrap', 'pascalprecht.translate', 'multi-select'])
.config(['$locationProvider', '$translateProvider', '$routeProvider',
function ($locationProvider, $translateProvider, $routeProvider) {
	$translateProvider.useLoader('resourceFactory', {});
	$routeProvider.when('/Home/AccessDenied', { templateUrl: '/Src/Components/Extensions/Common/templates/AccessDenied.html' });
	$translateProvider.preferredLanguage('en');
	$locationProvider.html5Mode(true);
} ]);
