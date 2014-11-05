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
				} });
		}
	};
} ]);