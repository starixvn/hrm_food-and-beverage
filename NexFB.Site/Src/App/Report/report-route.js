'use strict';
/*exported reportModule*/

var reportModule = angular.module("reportModule", ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
	var baseUrl = $("base").first().attr("href");
	$routeProvider.when('/MonthlyReportLocal/Index',
		{ templateUrl: baseUrl + 'src/app/report/templates/monthlyReportLocal.html', controller: 'MonthlyMainDishReportLocalController' });
	$routeProvider.when('/DailyMainDishReport/Index',
		{ templateUrl: baseUrl + 'src/app/report/templates/dailyMainDishReport.html', controller: 'DailyMainDishReportController' });
	$routeProvider.when('/EmployeeOrderHistory/Index',
		{ templateUrl: baseUrl + 'src/app/report/templates/employeeOrderHistory.html', controller: 'EmployeeOrderHistoryReportController' });
} ]);
