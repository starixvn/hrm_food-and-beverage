'use strict';
/*global reportModule, moment*/

reportModule.directive('renderDailyReport', function () {
	var generateDataBody = function (report, scope) {
		var rows = [],
			rowIndex = 0;

		if (report && report.MainDishes) {
			var currentSubRow = [],
				numberMainDishes = report.MainDishes.length;
			rowIndex = scope.gridOption.startIndex + 1;

			for (var i = 0; i < numberMainDishes; i++) {
				var mainDish = report.MainDishes[i],
					currentRow = [],
					employeeDishes = mainDish.EmployeeDishes,
					numberEmployeeDishes = employeeDishes.length;

				currentRow = {
					Columns: [
						{ Name: "Count", Value: mainDish.Department.Name + ' - ' + mainDish.Team.Name, MergeColNumber: 9, CellAlign: "left" }
					],
					SubRows: []
				};

				for (var j = 0; j < numberEmployeeDishes; j++) {
					var employeeDish = employeeDishes[j],
						shiftName = employeeDish.Shift ? employeeDish.Shift.ShiftCode : '';

					currentSubRow = {
						Columns: [
							{ Name: "Count", Value: rowIndex, CssClass: "remove-padding-left" },
							{ Name: "Date", Value: "" + moment(employeeDish.WorkDate).format(scope.dateFormat) },
							{ Name: "Shift", Value: shiftName },
							{ Name: "FullName", Value: employeeDish.Employee.FullName },
							{ Name: "NickName", Value: employeeDish.Employee.NickName },
							{ Name: "Breakfast", Value: employeeDish.Breakfast },
							{ Name: "Lunch", Value: employeeDish.Lunch },
							{ Name: "Dinner", Value: employeeDish.Dinner },
							{ Name: "Supper", Value: employeeDish.Supper }
						]
					};
					currentRow.SubRows.push(currentSubRow);
					rowIndex++;
				}

				rows.push(currentRow);
			}
		}

		return rows;
	};

	var generateDataHeader = function (numberFormat) {
		var columns = [
			{ HeaderText: "#", Width: 60, Name: "Count", HeaderAlign: "center", CellAlign: "center", ShowCollapseExpand: true },
			{ HeaderText: "Date", Width: 100, Name: "Date", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Shift", Width: 100, Name: "Shift", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Full Name", Width: 150, Name: "FullName", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Nick", Width: 100, Name: "NickName", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Breakfast", Width: 100, Name: "Breakfast", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat },
			{ HeaderText: "Lunch", Width: 100, Name: "Lunch", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat },
			{ HeaderText: "Dinner", Width: 100, Name: "Dinner", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat },
			{ HeaderText: "Supper", Width: 100, Name: "Supper", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat }
		];

		return columns;
	};

	var generateDataFooter = function (report) {
		var footerRow = [
			{
				Columns: [
					{ Value: "Sub Total", Name: "Count", MergeColNumber: 5, CellAlign: "right" },
					{ Value: report.SubTotal ? report.SubTotal.Breakfast : 0, Name: "Breakfast" },
					{ Value: report.SubTotal ? report.SubTotal.LUnch : 0, Name: "Lunch" },
					{ Value: report.SubTotal ? report.SubTotal.Dinner : 0, Name: "Dinner" },
					{ Value: report.SubTotal ? report.SubTotal.Supper : 0, Name: "Supper" }
				]
			},
			{
				Columns: [
					{ Value: "Grand Total", Name: "Count", MergeColNumber: 5, CellAlign: "right" },
					{ Value: report.GrandTotal ? report.GrandTotal.Breakfast : 0, Name: "Breakfast" },
					{ Value: report.GrandTotal ? report.GrandTotal.Lunch : 0, Name: "Lunch" },
					{ Value: report.GrandTotal ? report.GrandTotal.Dinner : 0, Name: "Dinner" },
					{ Value: report.GrandTotal ? report.GrandTotal.Supper : 0, Name: "Supper" }
				]
			},
			{
				Columns: [
					{ Value: "Total Amount", Name: "Count", MergeColNumber: 5, CellAlign: "right" },
					{ Value: report.TotalAmount, Name: "Breakfast", MergeColNumber: 4, CellAlign: 'center' }
				]
			}
		];

		return footerRow;
	};

	return {
		link: function (scope, iElement) {
			scope.$on("renderDailyReport", function () {
				function ReRenderAfterPaged(pageIndex, itemsPerPage) {
					scope.filter.isPagingCallback = true;
					scope.filter.startIndex = pageIndex >= 0 ? (pageIndex * itemsPerPage) : 0;
					scope.filter.itemsPerPage = itemsPerPage;

					scope.gridOption.itemsPerPage = itemsPerPage;
					scope.gridOption.currentPage = pageIndex;
					scope.gridOption.startIndex = scope.filter.startIndex;

					scope.getDailyMainDishReport.call(this, scope.filter);
					return false;
				}

				var id = iElement.attr('id'),
					report = scope.report,
					bodyData = generateDataBody(report, scope),
					headerData = generateDataHeader(scope.numberFormat),
					footerData = generateDataFooter(report);

				var gridOptions = {
					columns: headerData,
					bodyRows: bodyData,
					showTree: true,
					showTitle: true,
					titleInfo:
							{
								text: "Daily Main Dish Report - Price Per Meal",
								showExcelButton: true,
								exportExcelFunction: function () {
									scope.exportExcel.call(this, scope.filter);
								}
							},
					showFooter: true,
					footerRows: footerData,
					gridExpandHeight: 250,		
					showPager: true,
					customPager: true,
					pagerOption: {
						startPerPage: 50,
						totalItem: report.TotalItem,
						itemsPerPageArray: [50, 100, 200, 500],
						itemsPerPage: scope.gridOption.itemsPerPage,
						currentPage: scope.gridOption.currentPage,
						pagerSelectCallBack: ReRenderAfterPaged
					}
				};

				$('#' + id).fanexGrid(gridOptions);
			});
		}
	};
});

reportModule.directive('exportDailyMainDishReport', function () {
	return {
		link: function (scope) {
			scope.$on('exportDailyMainDishReport', function () {
				var data = $.param(scope.criteria),
					url = '/ExportReport/ExportDailyMainDishReport',
					downloadUrl = url + "?" + data,
					iframe = $('body iframe');

				if (iframe.length === 0) {
					iframe = $('<iframe width="0" height="0" class="display-none"></iframe>');
					iframe.appendTo('body');
				}

				iframe.attr('src', downloadUrl);
			});
		}
	};
});