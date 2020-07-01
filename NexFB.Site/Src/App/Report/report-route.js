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
    $routeProvider.when('/SideDishReport/Index',
		{ templateUrl: baseUrl + 'src/app/report/templates/sideDishReport.html', controller: 'SideDishReportController' });
    $routeProvider.when('/MonthlyReportPerDay/Index',
		{ templateUrl: baseUrl + 'src/app/report/templates/monthlyReportPerDay.html', controller: 'MonthlyMainDishReportPerDayController' });
    $routeProvider.when('/MealForecastReport/Index',
		{ templateUrl: baseUrl + 'src/app/report/templates/mealForecastReport.html', controller: 'MealForecastReportController' });
    $routeProvider.when('/SideDishSummaryReport/Index',
		{ templateUrl: baseUrl + 'src/app/report/templates/sideDishSummaryReport.html', controller: 'SideDishSummaryReportController' });
    $routeProvider.when('/ViewMenuReport/Index',
		{ templateUrl: baseUrl + 'src/app/report/templates/viewMenuReport.html', controller: 'ViewMenuReportController' });
    $routeProvider.when('/MealForecastReport/Detail',
		{ templateUrl: baseUrl + 'src/app/report/templates/mealForecastDetailReport.html', controller: 'MealForecastDetailReportController' });
    $routeProvider.when('/EmployeeRegistration/Detail',
        { templateUrl: baseUrl + 'src/app/report/templates/employeeRegistrationDetailReport.html', controller: 'EmployeeRegistrationDetailReport' });
    $routeProvider.when('/EmployeeNonRegistration/Detail',
        { templateUrl: baseUrl + 'src/app/report/templates/employeeNonRegistrationDetailReport.html', controller: 'EmployeeNonRegistrationDetailReport' });
    $routeProvider.when('/AdminOrderHistoryReport/Index',
		{ templateUrl: baseUrl + 'src/app/report/templates/adminOrderHistoryReport.html', controller: 'AdminOrderHistoryReportController' });
    $routeProvider.when('/MonthlyLunchAndDinnerReport/Index',
		{ templateUrl: baseUrl + 'src/app/report/templates/monthlyLunchAndDinnerReport.html', controller: 'MonthlyLunchAndDinnerReportController' });
    $routeProvider.when('/MonthlyBreakfastAndSupperReport/Index',
		{ templateUrl: baseUrl + 'src/app/report/templates/monthlyBreakfastAndSupperReport.html', controller: 'MonthlyBreakfastAndSuperController' });
}]);