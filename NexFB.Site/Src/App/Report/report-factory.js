'use strict';
/*global reportModule*/

reportModule.factory('reportFactory', ['$http', function ($http) {
	return {
		getMonthlyMainDishReportByLocal: function (reportDate, departmentId, employeeTypeId, teamId, mealId, groupBy) {
			return $http.get('/api/Report/GetMonthlyMainDishReportLocal',
				{ params: { reportDate: reportDate,
					departmentId: departmentId,
					employeeTypeId: employeeTypeId,
					teamId: teamId,
					mealId: mealId,
					groupBy: groupBy
				}
				});
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