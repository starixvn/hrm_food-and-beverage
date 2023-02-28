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
					employeeDishes = mainDish.EmployeeDishes,
					numberEmployeeDishes = employeeDishes.length;

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
							{ Name: "UnitName", Value: employeeDish.UnitName },
							{ Name: "DepartmentName", Value: employeeDish.DepartmentName },
							{ Name: "SectionName", Value: employeeDish.SectionName },
							{ Name: "TeamName", Value: employeeDish.TeamName },
							{ Name: "Breakfast", Value: employeeDish.Breakfast },
							{ Name: "Lunch", Value: employeeDish.Lunch },
							{ Name: "Dinner", Value: employeeDish.Dinner },
							{ Name: "Supper", Value: employeeDish.Supper }
						]
					};
					rows.push(currentSubRow);
					rowIndex++;
				}
			}
		}

		return rows;
	};

	var generateDataHeader = function (numberFormat) {
		var columns = [
			{ HeaderText: "#", Width: 60, Name: "Count", HeaderAlign: "center", CellAlign: "center", ShowCollapseExpand: true },
			{ HeaderText: "Date", Width: 120, Name: "Date", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Shift", Width: 100, Name: "Shift", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Full Name", Width: 240, Name: "FullName", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "Nick", Width: 140, Name: "NickName", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "Unit", Width: 100, Name: "UnitName", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Department", Width: 100, Name: "DepartmentName", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Section", Width: 100, Name: "SectionName", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Team", Width: 100, Name: "TeamName", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Breakfast", Width: 130, Name: "Breakfast", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat },
			{ HeaderText: "Lunch", Width: 130, Name: "Lunch", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat },
			{ HeaderText: "Dinner", Width: 130, Name: "Dinner", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat },
			{ HeaderText: "Supper", Width: 130, Name: "Supper", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat }
		];

		return columns;
	};

	var generateDataFooter = function (report) {
		var footerRow = [
			{
				Columns: [
					{ Value: "Total", Name: "Count", MergeColNumber: 9, CellAlign: "right" },
					{ Value: report.Total ? report.Total.Breakfast : 0, Name: "Breakfast" },
					{ Value: report.Total ? report.Total.Lunch : 0, Name: "Lunch" },
					{ Value: report.Total ? report.Total.Dinner : 0, Name: "Dinner" },
					{ Value: report.Total ? report.Total.Supper : 0, Name: "Supper" }
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
								text: "Daily Main Dish Report",
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

reportModule.directive('exportDailyMainDishReport', ['baseUrl', function (baseUrl) {
	return {
		link: function (scope) {
			scope.$on('exportDailyMainDishReport', function () {
				var data = $.param(scope.criteria),
					url = baseUrl + 'ExportReport/ExportDailyMainDishReport',
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
} ]);
