'use strict';
/*exported appModule*/

var appModule = angular.module("appModule",
['mealRegistrationModule', 'userControlModule', 'orderingModule', 'foodItemManagementModule',
'foodMenuManagementModule', 'mealPlanningManagementModule', 'sideDishManagementModule', 'alternativeMenuModule',
'shiftMealMappingModule', 'setMenuManagementModule', 'reportModule',
'ui.bootstrap', 'pascalprecht.translate', 'multi-select', 'taiPlaceholder'])
.config(['$locationProvider', '$translateProvider', '$routeProvider', 'datepickerConfig', 'datepickerPopupConfig',
function ($locationProvider, $translateProvider, $routeProvider, datepickerConfig, datepickerPopupConfig) {
	$translateProvider.useLoader('resourceFactory', {});
	$routeProvider.when('/Home/AccessDenied', { templateUrl: '/Src/Components/Extensions/Common/templates/AccessDenied.html' });
	$translateProvider.preferredLanguage('en');
	$locationProvider.html5Mode(true);
	datepickerConfig.showWeeks = false;
	datepickerPopupConfig.datepickerPopup = 'dd MMM, yyyy';
} ]);
