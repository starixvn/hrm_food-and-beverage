'use strict';
/*global reportModule*/

reportModule.factory('reportFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		getMonthlyMainDishReportByLocal: function (filter) {
			return $http.get(baseUrl + 'api/report/GetMonthlyMainDishReportLocal', {
				params: {
					reportDate: filter.reportDate.toDateString(),
					departmentId: filter.departmentId,
					employeeTypeId: filter.employeeTypeId,
					teamId: filter.teamId,
					mealId: filter.mealId,
					groupBy: filter.groupBy,
					startIndex: filter.startIndex,
					itemsPerPage: filter.itemsPerPage,
					isExportReport: filter.isExportReport,
					isPagingCallback: filter.isPagingCallback
				}
			});
		},
		getDailyMainDishReport: function (filter) {
			return $http.get('api/report/GetDailyMainDishReport', {
				params: {
					fromDate: filter.fromDate.toDateString(),
					toDate: filter.toDate.toDateString(),
					orderCount: filter.orderCount,
					departmentId: filter.departmentId,
					teamId: filter.teamId,
					mealId: filter.mealId,
					userId: filter.userId,
						query: filter.query,
						startIndex: filter.startIndex,
						itemsPerPage: filter.itemsPerPage,
						isPagingCallback: filter.isPagingCallback
				}});
		},
		getDepartmentByUserRole: function () {
			return $http.get(baseUrl + 'api/report/GetDepartmentByUserRole');
		},
		getEmployeeOrderHistoryReport: function (filter) {
			return $http.get(baseUrl + 'api/report/GetEmployeeOrderHistoryReport',
			{ params: { fromDate: filter.fromDate.toDateString(),
					toDate: filter.toDate.toDateString(),
					orderBy: filter.orderBy,
					employeeMenu: filter.employeeMenu,
					orderPlatform: filter.orderPlatform,
					mealId: filter.mealId,
					statusId: filter.statusId,
					isPagingCallback: filter.isPagingCallback,
					startIndex: filter.startIndex,
					itemsPerPage: filter.itemsPerPage
				}
			});
		},
		getSideDishReport: function (filter) {
			return $http.get('/api/report/Report/GetSideDishReport', {
				params: {
					fromDate: filter.fromDate.toDateString(),
					toDate: filter.toDate.toDateString(),
					departmentId: filter.departmentId === -1 ? null : filter.departmentId,
					teamIds: filter.teamIds,
					employeeTypeIds: filter.employeeTypeIds,
					sideDishItemIds: filter.sideDishItemIds,
					pageIndex: filter.pageIndex,
					itemsPerPage: filter.itemsPerPage
				}
			});
		},
		getOrderedByEnum: function () {
			return $http.get(baseUrl + 'api/report/GetOrderedByEnum');
		},
		getOrderPlatformEnum: function () {
			return $http.get(baseUrl + 'api/report/GetOrderPlatformEnum');
		},
		getOrderStatusEnum: function () {
			return $http.get(baseUrl + 'api/report/GetOrderStatusEnum');
		},
		getMenuTypeEnum: function () {
			return $http.get(baseUrl + 'api/report/GetMenuTypeEnum');
		},
		getMonthlyMainDishReportPerDay: function (filter) {
			return $http.get(baseUrl + 'api/report/GetMonthlyMainDishPerDayReport',
			{
				params: {
					reportDate: filter.reportDate.toDateString(),
					departmentId: filter.departmentId,
					employeeTypeId: filter.employeeTypeId,
					teamId: filter.teamId,
					workDay: filter.workDay,
					groupBy: filter.groupBy,
					startIndex: filter.startIndex,
					itemsPerPage: filter.itemsPerPage,
					isPagingCallback: filter.isPagingCallback
				}
			});
		},
		getDayTypeEnum: function () {
			return $http.get(baseUrl + 'api/report/GetDayType');
		},
		getDepartmentOfCurrentUser: function () {
			return $http.get(baseUrl + 'api/common/GetDepartmentOfCurrentUser');
		}
	};
} ]);

reportModule.factory('reportUtilities', function () {

	var utilities = {},
		numberFormat = '0,0',
		monthFormat = 'MMM-YYYY',
		dateFormatDailyReport = 'DD-MMM-YYYY',
		dateFormaEmployeeOrderHistoryReport = "DD MMM YYYY";

	utilities.getTeamId = function (teamOptions) {
		var teamId = '';

		angular.forEach(teamOptions, function (value) {
			if (value.ticked) {
				var id = value.TeamId.toString();
				teamId = teamId !== '' ? teamId + ',' + id : id;
			}
		});

		return teamId;
	};

	utilities.getSideDishIds = function (sideDishItems) {
		var sideDishIds = '',
			selectedIds = [];

		angular.forEach(sideDishItems, function (value) {
			if (value.ticked) {
				selectedIds.push(value.FoodId.toString());
			}
		});

		sideDishIds = selectedIds.join(",");

		return sideDishIds;
	};

	utilities.getNumberFormat = function () {
		return numberFormat;
	};

	utilities.getMonthFormat = function () {
		return monthFormat;
	};

	utilities.getDateFormatDailyReport = function () {
		return dateFormatDailyReport;
	};

	utilities.getDateFormatEmployeeOrderHistoryReport = function () {
		return dateFormaEmployeeOrderHistoryReport;
	};

	return utilities;
});