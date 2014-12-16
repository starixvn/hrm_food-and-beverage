'use strict';
/*global reportModule*/

reportModule.factory('reportFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		getMonthlyMainDishReportByLocal: function (filter) {
			return $http.get(baseUrl + 'api/report/Report/GetMonthlyMainDishReportLocal',
				{ params: { reportDate: filter.reportDate.toDateString(),
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
			return $http.get('/api/report/Report/GetDailyMainDishReport',
				{ params: { fromDate: filter.fromDate.toDateString(),
						toDate: filter.toDate.toDateString(),
						orderCount: filter.orderCount,
						departmentId: filter.departmentId,
						teamId: filter.teamId,
						mealId: filter.mealId,
						userId: filter.userId,
						query: filter.query
				}});
		},
		getDepartmentByUserRole: function () {
			return $http.get('/api/report/GetDepartmentByUserRole');
		}
	};
} ]);

reportModule.factory('reportUtilities', function () {

	var utilities = {},
		numberFormat = '0,0',
		monthFormat = 'MMM-YYYY',
		dateFormatDailyReport = 'd-MMM-YYYY';

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

	utilities.getNumberFormat = function () {
		return numberFormat;
	};

	utilities.getMonthFormat = function () {
		return monthFormat;
	};

	utilities.getDateFormatDailyReport = function () {
		return dateFormatDailyReport;
	};

	return utilities;
});