'use strict';
/*global reportModule*/

reportModule.directive('viewMenuReport', function () {
	var columnHeaders = [
			{ HeaderText: "#", Width: 60, Name: "No", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Food Item (Eng)", Width: 570, Name: "FoodEngName", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "Food Item (Vie)", Width: 563, Name: "FoodVNMName", HeaderAlign: "center", CellAlign: "left" }
	];

	var generateBodyRows = function (data) {
		var rows = [];

		if (data) {
			var counter = 1;

			angular.forEach(data.ViewMenuItems, function (item) {
				var currentRow = {};

				var result = $.grep(rows, function (e) { return e.Columns[0].Key === item.MenuName; });

				if (result.length > 0) {
					currentRow = result[0];
				} else {
					var iconClass = "pull-left ";
					iconClass += item.IsVegetarian === true ? "icon-veg" : "icon-none-veg";
					var menuClass = item.IsVegetarian === true ? "menu-vegetarian" : "menu-non-vegetarian";

					currentRow = {
						Columns: [
							{
								Name: "No",
								Value: '<div class="' + iconClass + '"></div>' + item.MenuName,
								MergeColNumber: 3,
								CellAlign: "left",
								Key: item.MenuName
							}
						],
						SubRows: [],
						CssClass: menuClass
					};

					rows.push(currentRow);
				}

				var currentSubRow = {
					Columns: [
						{ Name: "No", Value: counter },
						{ Name: "FoodEngName", Value: item.FoodEngName },
						{ Name: "FoodVNMName", Value: item.FoodVNMName }
					]
				};

				currentRow.SubRows.push(currentSubRow);
				counter++;
			});
		}

		return rows;
	};

	return {
		link: function (scope, element) {
			scope.$on('renderViewMenuReport', function () {
				var bodyRows = generateBodyRows(scope.reportData);
				var title = "Menu for " + scope.criteria.date.toString("dd MMM, yyyy");

				var gridOptions = {
					columns: columnHeaders,
					bodyRows: bodyRows,
					showTree: true,
					showTitle: true,
					titleInfo: {
						text: title,
						showExcelButton: true,
						exportExcelFunction: function () {
							scope.exportExcel.call(this);
						}
					},
					gridExpandHeight: 250
				};

				$(element).fanexGrid(gridOptions);
			});
		}
	};
});