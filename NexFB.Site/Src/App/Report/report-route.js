'use strict';
/*exported reportModule*/

var reportModule = angular.module("reportModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	$routeProvider.when('/MonthlyReportLocal/Index',
		{ templateUrl: '/src/app/report/templates/monthlyReportLocal.html', controller: 'MonthlyMainDishReportLocalController' });
	$routeProvider.when('/DailyMainDishReport/Index',
		{ templateUrl: '/src/app/report/templates/dailyMainDishReport.html', controller: 'DailyMainDishReportController' });
} ]);
