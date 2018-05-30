'use strict';
/*global reportModule*/

reportModule.controller('ViewMenuReportController',
['$scope', 'reportFactory', 'commonFactory', 'ErrorFactory', 'foodMenuFactory', 'foodMenuUtilities', 'baseUrl', 'notificationFactory',
function ($scope, reportFactory, commonFactory, ErrorFactory, foodMenuFactory, foodMenuUtilities, baseUrl, notificationFactory) {
    var self = this,
        fromDate = moment().startOf("isoWeek").toDate(),
        toDate = moment().endOf("isoWeek").toDate();

    // $scope region.
    $scope.meals = [];

    $scope.criteria = {
        mealIds: '',
        fromDate: fromDate,
        toDate: toDate,
        isVegetarian: '-1'
    };

    $scope.calendar = {
        startDateOpened: false,
        endDateOpened: false
    };

    $scope.dateOpened = false;

    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.dateOpened = true;
    };

    /*
	** Open calendar.
	*/
    $scope.openPopupStartDate = function ($event) {
        commonFactory.openPopupStartDate($event, $scope);
    };

    $scope.openPopupEndDate = function ($event) {
        commonFactory.openPopupEndDate($event, $scope);
    };

    $scope.search = function () {
        self.getReportData();
    };

    /*
	** Query data.
	*/

    $scope.exportExcel = function () {
        var criteria = self.getCriteria();

        var data = $.param(criteria),
					url = baseUrl + 'ExportReport/ExportViewMenuReport',
					downloadUrl = url + "?" + data,
					iframe = $('body iframe');

        if (iframe.length === 0) {
            iframe = $('<iframe width="0" height="0" class="display-none"></iframe>');
            iframe.appendTo('body');
        }

        iframe.attr('src', downloadUrl);
    };

    // self region.
    self.getReportData = function () {
        var criteria = self.getCriteria();
        if (criteria.fromDate < criteria.toDate) {
            reportFactory.getViewMenuReport(criteria).success(function (reportData) {
                $scope.reportData = reportData;
                $scope.$broadcast('renderViewMenuReport');
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });
        } else {
            notificationFactory.error("DateRangeInvalid");
        }
    };

    self.getMeals = function () {
        foodMenuFactory.getAllMealTimesheet().success(function (meals) {
            $scope.meals = meals;
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    self.loadData = function () {
        self.getMeals();

        self.getReportData();
    };

    self.getCriteria = function () {
        $scope.criteria.mealIds = foodMenuUtilities.getMealId($scope.meals);

        return {
            fromDate: $scope.criteria.fromDate.toString("MM/dd/yyyy"),
            toDate: $scope.criteria.toDate.toString("MM/dd/yyyy"),
            mealId: $scope.criteria.mealIds,
            isVegetarian: $scope.criteria.isVegetarian === '-1' ? '' : ($scope.criteria.isVegetarian === '1' ? true : false)
        };
    };

    self.loadData();
}]);