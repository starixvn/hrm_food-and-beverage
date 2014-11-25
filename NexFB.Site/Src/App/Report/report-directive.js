'use strict';
/*global reportModule*/


reportModule.directive('renderMonthlyReport', function () {
	//	var generateNumberMealsRow = function (mealItem) {
	//		var numberMealsRow = [],
	//			numberMeals = mealItem.NumberMeals;

	//		for (var i = 0; i < numberMeals.length; i++) {
	//			var row = { Name: i + 1, Value: numberMeals[i] };
	//			numberMealsRow.push(row);
	//		}

	//		numberMealsRow.push({ Name: "Quantity", Value: numberMeals.TotalQuantity });
	//		numberMealsRow.push({ Name: "Amount", Value: numberMeals.Amount });

	//		return numberMealsRow;
	//	};

	var generateDataBody = function (report) {
		var rows = [];

		if (report && report.Meals) {
			var currentSubRow = [];

			for (var i = 0; i < report.Meals.length; i++) {
				var meal = report.Meals[i],
				currentRow = {
					Columns: [{ Name: "Meal", Value: meal.Meal.MealEngName, MergeColNumber: 37}],
					SubRows: []
				},
				mealItems = meal.MealItems;

				for (var j = 0; j < mealItems.length; j++) {
					var mealItem = mealItems[j];

					currentSubRow = {
						Columns: [
							{ Name: "Meal", Value: j + 1 },
							{ Name: "NickName", Value: mealItem.Employee.NickName },
							{ Name: "EmployeeType", Value: mealItem.EmployeeType.EmployeeTypeName },
							{ Name: "Department", Value: mealItem.Department.Name + ' > ' + mealItem.Team.Name },
							{ Name: "Quantity", Value: mealItem.NumberMealsMonth.TotalQuantity },
							{ Name: "Amount", Value: mealItem.NumberMealsMonth.Amount }
						]
					};

					//currentSubRow.Columns.splice(4, 0, numberMealsRow);
					currentRow.SubRows.push(currentSubRow);
				}
			}
		}

		return rows;
	};

	var generateDataHeader = function () {
		var columns = [
			{ HeaderText: "#", Width: 60, Name: "Meal", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Nick", Width: 100, Name: "NickName", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Employee Type", Width: 100, Name: "EmployeeType", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "Department/Section", Width: 150, Name: "Department", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "Qty", Width: 80, Name: "Quantity", HeaderAlign: "center", CellAlign: "right" },
			{ HeaderText: "Amount", Width: 80, Name: "Amount", HeaderAlign: "center", CellAlign: "right" }
		];

		return columns;
	};

	return {
		link: function (scope, iElement) {
			scope.$on('renderMonthlyReport', function () {
				var id = iElement.attr('id'),
				report = scope.localReport,
				bodyData = generateDataBody(report),
				headerData = generateDataHeader();

				var gridOptions = {
					columns: headerData,
					bodyRows: bodyData
				};

				$('#' + id).fanexGrid(gridOptions);
			});
		}
	};
});