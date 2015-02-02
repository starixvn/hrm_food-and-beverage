'use strict';
/*global reportModule*/

reportModule.directive('renderSideDishSummaryReport', ['baseUrl', function (baseUrl) {
	var generateDataHeader = function () {
		var columns = [
			{ HeaderText: "#", Width: 60, Name: "Count", HeaderAlign: "center", CellAlign: "center", ShowCollapseExpand: true },
			{ HeaderText: "Full Name", Width: 250, Name: "FullName", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "Nick", Width: 250, Name: "NickName", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "Employee Type", Width: 250, Name: "EmployeeType", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "Quantity", Width: 190, Name: "Qty", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0" },
			{ HeaderText: "Amount", Width: 190, Name: "Amount", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0" }
		];

		return columns;
	};

	var generateDataFooter = function (report, scope) {
		var linkToDetail = '<a class="side-dish-detail" href="' + baseUrl + 'SideDishReport/Index?'
			+ '&departmentId=&teamId=&employeeTypeId=&userId='
			+ '&fromDate=' + scope.filter.fromDate.toString("MM dd yyyy")
			+ '&toDate=' + scope.filter.toDate.toString("MM dd yyyy")
			+ '">' + report.TotalQuantity + '</a>';

		var footerRows = [
			{
				Columns: [
					{ Value: "Total", Name: "Count", MergeColNumber: 4, CellAlign: "right" },
					{ Value: linkToDetail, Name: "Qty", CellAlign: "right", FormatNumber: "0,0" },
					{ Value: report.TotalAmount, Name: "Amount", CellAlign: "right", FormatNumber: "0,0" }
				]
			}
		];

		return footerRows;
	};

	var generateDataBody = function (report, scope) {
		var rows = [];

		if (report && report.SideDishSummarys) {
			var rowIndex = 1;

			angular.forEach(report.SideDishSummarys, function (item) {
				var currentRow = {};
				var result = $.grep(rows, function (e) { return e.Columns[0].Value === item.DepartmentTeamName; });

				if (result.length > 0) {
					currentRow = result[0];
				}
				else {
					currentRow = {
						Columns: [
							{ Name: "Count", Value: item.DepartmentTeamName, MergeColNumber: 6, CellAlign: "left" }
						],
						SubRows: []
					};

					rows.push(currentRow);
					rowIndex = 1;
				}

				var linkToDetail = '<a class="side-dish-detail" href="' + baseUrl + 'SideDishReport/Index?'
					+ '&departmentId=' + scope.filter.departmentId
					+ '&teamId=' + scope.filter.teamId
					+ '&employeeTypeId=' + scope.filter.employeeTypeId
					+ '&userId=' + item.SysId
					+ '&fromDate=' + scope.filter.fromDate.toString("MM dd yyyy")
					+ '&toDate=' + scope.filter.toDate.toString("MM dd yyyy")
					+ '">' + item.Qty + '</a>';

				var currentSubRow = {
					Columns: [
						{ Name: "Count", Value: rowIndex },
						{ Name: "FullName", Value: item.FullName },
						{ Name: "NickName", Value: item.NickName },
						{ Name: "EmployeeType", Value: item.EmpType },
						{ Name: "Qty", Value: linkToDetail },
						{ Name: "Amount", Value: item.Amount }
					]
				};

				currentRow.SubRows.push(currentSubRow);
				rowIndex++;
			});
		}

		return rows;
	};

	return {
		link: function (scope, iElement) {
			scope.$on('renderSideDishSummaryReport', function () {
				function ReRenderAfterPaged(pageIndex, itemsPerPage) {
					scope.filter.pageIndex = pageIndex;
					scope.filter.itemsPerPage = itemsPerPage;

					scope.getSideDishSummaryReport.call(this, scope.filter);
					return false;
				}

				var id = iElement.attr('id'),
					report = scope.report,
					headerData = generateDataHeader(),
					footerData = generateDataFooter(report, scope),
					bodyData = generateDataBody(report, scope);

				var gridOptions = {
					columns: headerData,
					bodyRows: bodyData,
					showTree: true,
					showTitle: true,
					titleInfo: {
						text: 'Side Dish Summary Report',
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
						itemsPerPage: scope.filter.itemsPerPage,
						currentPage: scope.filter.pageIndex,
						pagerSelectCallBack: ReRenderAfterPaged
					}
				};

				$('#' + id).fanexGrid(gridOptions);
			});
		}
	};
} ]);

reportModule.directive('exportSideDishSummaryReport', ['baseUrl', function (baseUrl) {
	return {
		link: function (scope) {
			scope.$on('exportSideDishSummaryReport', function () {
				var data = $.param(scope.criteria),
                    url = baseUrl + 'ExportReport/ExportSideDishSummaryReport',
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