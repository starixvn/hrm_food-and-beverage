'use strict';
/*global reportModule*/

reportModule.directive('sideDishReport', function () {
	var columnHeaders = [
			{ HeaderText: "#", Width: 60, Name: "No", HeaderAlign: "center", CellAlign: "center", ShowCollapseExpand: true },
			{ HeaderText: "Date", Width: 90, Name: "WorkDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "DD-MMM-YY" },
			{ HeaderText: "Shift", Width: 80, Name: "Shift", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Full Name", Width: 200, Name: "FullName", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "Nick", Width: 110, Name: "NickName", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "Food item (Vie)", Width: 170, Name: "FoodVNMName", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "Food item (Eng)", Width: 170, Name: "FoodEngName", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "Price", Width: 100, Name: "Price", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0" },
			{ HeaderText: "Quantity", Width: 80, Name: "Qty", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0" },
			{ HeaderText: "Amount", Width: 100, Name: "Amount", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0" }
	];

	var generateFooterRows = function (data) {
		var footerRows = [
			{
				Columns: [
					{ Value: "", Name: "No", MergeColNumber: 7, CellAlign: "left" },
					{ Value: "Total", Name: "Price", CellAlign: "center" },
					{ Value: data.TotalQuantity.toString(), Name: "Qty", CellAlign: "right", FormatNumber: "0,0" },
					{ Value: data.TotalAmount.toString(), Name: "Amount", CellAlign: "right", FormatNumber: "0,0" }
				]
			}
		];

		return footerRows;
	};

	var generateBodyRows = function (data) {
		var rows = [];

		if (data) {
			angular.forEach(data.DepartmentTeamModels, function (item) {
				var currentRow = {
					Columns: [
						{ Name: "No", Value: item.DepartmentTeamName, MergeColNumber: 10, CellAlign: "left" }
					],
					SubRows: []
				};

				angular.forEach(item.OrderModels, function (order) {
					var currentSubRow = {
						Columns: [
							{ Name: "No", Value: order.No },
							{ Name: "WorkDate", Value: order.WorkDate },
							{ Name: "Shift", Value: order.Shift },
							{ Name: "FullName", Value: order.FullName },
							{ Name: "NickName", Value: order.NickName },
							{ Name: "FoodVNMName", Value: order.FoodVNMName },
							{ Name: "FoodEngName", Value: order.FoodEngName },
							{ Name: "Price", Value: order.Price },
							{ Name: "Qty", Value: order.Qty },
							{ Name: "Amount", Value: order.Amount }
						]
					};

					currentRow.SubRows.push(currentSubRow);
				});

				rows.push(currentRow);
			});
		}

		return rows;
	};

	return {
		link: function (scope, element) {
			scope.$on('renderSideDishReport', function () {
				var footerRows = generateFooterRows(scope.reportData),
					bodyRows = generateBodyRows(scope.reportData);

				var ReRenderAfterPaged = function (pageIndex, itemsPerPage) {
					scope.criteria.itemsPerPage = itemsPerPage;
					scope.criteria.pageIndex = pageIndex;
					scope.getReportData.call(this);

					return false;
				};

				var gridOptions = {
					columns: columnHeaders,
					bodyRows: bodyRows,
					showTree: true,
					showTitle: true,
					titleInfo: {
						text: "Side Dish Report",
						showExcelButton: true,
						exportExcelFunction: function () {
							scope.exportExcel.call(this);
						}
					},
					showFooter: true,
					footerRows: footerRows,
					gridExpandHeight: 150,
					showPager: true,
					customPager: true,
					pagerOption: {
						startPerPage: 50,
						totalItem: scope.reportData.TotalItem,
						itemsPerPageArray: [50, 100, 200, 500],
						itemsPerPage: scope.criteria.itemsPerPage,
						currentPage: scope.criteria.pageIndex,
						pagerSelectCallBack: ReRenderAfterPaged
					}
				};

				$(element).fanexGrid(gridOptions);
			});
		}
	};
});