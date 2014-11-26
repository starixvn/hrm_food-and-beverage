'use strict';
/*global reportModule*/


reportModule.directive('renderMonthlyReport', function () {

	var generateNumberMealsRow = function (numberMealsMonth) {
		var numberMealsRow = [],
			numberMeals = numberMealsMonth.NumberMeals;

		for (var i = 0; i < numberMeals.length; i++) {
			var row = { Name: "" + (i + 1), Value: numberMeals[i] };
			numberMealsRow.push(row);
		}

		numberMealsRow.push({ Name: "Quantity", Value: numberMealsMonth.TotalQuantity });
		numberMealsRow.push({ Name: "Amount", Value: numberMealsMonth.Amount });

		return numberMealsRow;
	};

	var generateDataBody = function (report) {
		var rows = [];

		if (report && report.Meals) {
			var currentSubRow = [];

			for (var i = 0; i < report.Meals.length; i++) {
				var meal = report.Meals[i],
					currentRow = [],
					mealItems = meal.MealItems;

				if (report.IsGroupByEmployee) {
					currentRow = {
						Columns: [{ Name: "Meal", Value: meal.Meal.MealEngName, MergeColNumber: 37, CellAlign: "left"}],
						SubRows: []
					};
				}
				else {
					currentRow = {
						Columns: [{ Name: "Meal", Value: meal.Meal.MealEngName, MergeColNumber: 35, CellAlign: "left"}],
						SubRows: []
					};
				}

				for (var j = 0; j < mealItems.length; j++) {
					var mealItem = mealItems[j];

					if (report.IsGroupByEmployee) {
						currentSubRow = {
							Columns: [
							{ Name: "Meal", Value: j + 1 },
							{ Name: "NickName", Value: mealItem.Employee.NickName },
							{ Name: "EmployeeType", Value: mealItem.EmployeeType.EmployeeTypeName },
							{ Name: "Department", Value: mealItem.Department.Name + ' > ' + mealItem.Team.Name }
						]
						};
					}
					else {
						currentSubRow = {
							Columns: [
							{ Name: "Meal", Value: j + 1 },
							{ Name: "Department", Value: mealItem.Department.Name + ' > ' + mealItem.Team.Name }
						]
						};
					}

					var numberMealsRow = generateNumberMealsRow(mealItem.NumberMealsMonth);
					currentSubRow.Columns = currentSubRow.Columns.concat(numberMealsRow);
					currentRow.SubRows.push(currentSubRow);
				}

				rows.push(currentRow);
			}
		}

		return rows;
	};

	var generateGroupHeader = function (month, isGroupByEmployee) {
		var groupHeader = [];

		if (isGroupByEmployee) {
			groupHeader = [
				{
					Columns: [
						{ HeaderText: "#", Name: "Meal", MergeRowNumber: 2 },
						{ HeaderText: "Nick", Name: "NickName", MergeRowNumber: 2 },
						{ HeaderText: "Employee Type", Name: "EmployeeType", MergeRowNumber: 2 },
						{ HeaderText: "Department/Section", Name: "Department", MergeRowNumber: 2 },
						{ HeaderText: "" + month, Name: "1", MergeColNumber: 31 },
						{ HeaderText: "Total", Name: "Quantity", MergeColNumber: 2 }
					]
				}
			];
		}
		else {
			groupHeader = [
				{
					Columns: [
						{ HeaderText: "#", Name: "Meal", MergeRowNumber: 2 },
						{ HeaderText: "Department/Section", Name: "Department", MergeRowNumber: 2 },
						{ HeaderText: "" + month, Name: "1", MergeColNumber: 31 },
						{ HeaderText: "Total", Name: "Quantity", MergeColNumber: 2 }
					]
				}
			];
		}

		return groupHeader;
	};

	var generateDataHeader = function (weekdays, isGroupByEmployee) {
		var columns = [];

		if (isGroupByEmployee) {
			columns = [
				{ HeaderText: "#", Width: 40, Name: "Meal", HeaderAlign: "center", CellAlign: "center" },
				{ HeaderText: "Nick", Width: 80, Name: "NickName", HeaderAlign: "center", CellAlign: "center" },
				{ HeaderText: "Employee Type", Width: 150, Name: "EmployeeType", HeaderAlign: "center", CellAlign: "left" },
				{ HeaderText: "Department/Section", Width: 150, Name: "Department", HeaderAlign: "center", CellAlign: "left" }
			];
		}
		else {
			columns = [
				{ HeaderText: "#", Width: 40, Name: "Meal", HeaderAlign: "center", CellAlign: "center" },
				{ HeaderText: "Department/Section", Width: 100, Name: "Department", HeaderAlign: "center", CellAlign: "left" }
			];
		}

		var numberMealsLength = weekdays.NumberMeals.length;

		for (var i = 0; i < numberMealsLength; i++) {
			var mealHeader = { HeaderText: "" + (i + 1), Width: 30, Name: "" + (i + 1), HeaderAlign: "center", CellAlign: "right" };
			columns.push(mealHeader);
		}

		columns.push({ HeaderText: "Qty", Width: 80, Name: "Quantity", HeaderAlign: "center", CellAlign: "right" });
		columns.push({ HeaderText: "Amount", Width: 80, Name: "Amount", HeaderAlign: "center", CellAlign: "right" });

		return columns;
	};

	var generateDataFooter = function (weekdays, isGroupByEmployee) {
		var columns = [];

		if (isGroupByEmployee) {
			columns = [
				{ Value: "Total", Name: "Meal", MergeColNumber: 4 }
			];
		}
		else {
			columns = [
				{ Value: "Total", Name: "Meal", MergeColNumber: 2 }
			];
		}

		var numberMealsLength = weekdays.NumberMeals.length;

		for (var i = 0; i < numberMealsLength; i++) {
			var mealFooter = { Value: weekdays.NumberMeals[i], Name: "" + (i + 1), CellAlign: 'right' };
			columns.push(mealFooter);
		}

		columns.push({ Value: weekdays.TotalQuantity, Name: "Quantity" });
		columns.push({ Value: weekdays.Amount, Name: "Amount" });

		var footerRow = [
			{
				Columns: columns
			}
		];

		return footerRow;
	};

	return {
		link: function (scope, iElement) {
			scope.$on('renderMonthlyReport', function () {
				var id = iElement.attr('id'),
					report = scope.localReport,
					bodyData = generateDataBody(report),
					groupHeaderData = generateGroupHeader(report.Month, report.IsGroupByEmployee),
					headerData = generateDataHeader(report.WeekDays, report.IsGroupByEmployee),
					footerData = generateDataFooter(report.Total, report.IsGroupByEmployee);

				var gridOptions = {
					customizeHeader: true,
					groupHeaderRows: groupHeaderData,
					columns: headerData,
					bodyRows: bodyData,
					showTree: true,
					showTitle: true,
					titleInfo:
						{
							text: "Monthly Report"
						},
					numberOfFrozenColumn: report.IsGroupByEmployee ? 4 : 2,
					showFooter: true,
					footerRows: footerData
				};

				$('#' + id).fanexGrid(gridOptions);
			});
		}
	};
});