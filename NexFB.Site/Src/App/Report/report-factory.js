'use strict';
/*global reportModule*/

reportModule.factory('reportFactory', ['$http', function ($http) {
	return {
		getMonthlyMainDishReportByLocal: function (filter) {
			return $http.get('/api/report/Report/GetMonthlyMainDishReportLocal',
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
		getDepartmentByUserRole: function () {
			return $http.get('/api/report/GetDepartmentByUserRole');
		}
	};
} ]);

reportModule.factory('reportUtilities', function () {

	var utilities = {},
		numberFormat = '0,0',
		monthFormat = 'MMM-YYYY';

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

	return utilities;
});