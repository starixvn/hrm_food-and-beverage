'use strict';
/*global reportModule*/

reportModule.controller('ViewMenuReportController',
['$scope', 'reportFactory', 'ErrorFactory', 'foodMenuFactory', 'foodMenuUtilities', 'baseUrl',
function ($scope, reportFactory, ErrorFactory, foodMenuFactory, foodMenuUtilities, baseUrl) {
	var self = this,
		date = new Date(),
		year = date.getFullYear(),
		month = date.getMonth(),
		day = date.getDate();

	// $scope region.
	$scope.meals = [];

	$scope.criteria = {
		mealIds: '',
		date: new Date(year, month, day),
		isVegetarian: '-1'
	};

	$scope.dateOpened = false;

	$scope.open = function ($event) {
		$event.preventDefault();
		$event.stopPropagation();

		$scope.dateOpened = true;
	};

	$scope.search = function () {
		self.getReportData();
	};

	$scope.next = function () {
		var date = $scope.criteria.date;
		date.setDate(date.getDate() + 1);
		$scope.criteria.date = new Date(date);

		self.getReportData();
	};

	$scope.previous = function () {
		var date = $scope.criteria.date;
		date.setDate(date.getDate() - 1);
		$scope.criteria.date = new Date(date);

		self.getReportData();
	};

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

		reportFactory.getViewMenuReport(criteria).success(function (reportData) {
			$scope.reportData = reportData;
			$scope.$broadcast('renderViewMenuReport');
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
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
			onDate: $scope.criteria.date.toString("MM/dd/yyyy"),
			mealId: $scope.criteria.mealIds,
			isVegetarian: $scope.criteria.isVegetarian === '-1' ? '' : ($scope.criteria.isVegetarian === '1' ? true : false)
		};
	};

	self.loadData();
} ]);
