'use strict';
/*global reportModule*/

reportModule.directive('renderEmployeeOrderHistoryReport', function () {
	var generateDataBody = function (report) {
		var rows = [];

		if (report && report.EmployeeOrders) {
			var numberEmployeeOrders = report.EmployeeOrders.length;

			//rowIndex = scope.gridOption.startIndex + 1;
			for (var i = 0; i < numberEmployeeOrders; i++) {
				var employeeOrder = report.EmployeeOrders[i],
					currentRow = [];

				currentRow = {
					Columns: [
						{ Name: "Date", Value: employeeOrder.OnDate, CellAlign: "center" },
						{ Name: "Meal", Value: employeeOrder.Meal.MealEngName, CellAlign: "center" },
						{ Name: "Order", Value: employeeOrder.Order.OrderNo, CellAlign: "center" },
						{ Name: "Status", Value: employeeOrder.Status.Name, CellAlign: "left" },
						{ Name: "OrderedBy", Value: employeeOrder.OrderBy.NickName, CellAlign: "left" },
						{ Name: "OrderPlarform", Value: employeeOrder.OrderForm, CellAlign: "left" },
						{ Name: "MyMenu", Value: employeeOrder.Menus }
					]
				};

				rows.push(currentRow);
			}
		}

		return rows;
	};

	return {
		link: function (scope, iElement) {
			scope.$on('renderEmployeeOrderHistoryReport', function () {
				var id = iElement.attr('id'),
					report = scope.report,
					bodyData = generateDataBody(report, scope);
//					headerData = generateDataHeader(scope.numberFormat),
//					footerData = generateDataFooter(report);

				var gridOptions = {
//					columns: headerData,
					bodyRows: bodyData,
					showTree: true,
					showTitle: true,
					titleInfo:
							{
								text: "My Order History",
								showExcelButton: true,
								exportExcelFunction: function () {
									scope.exportExcel.call(this, scope.filter);
								}
							},
//					showFooter: true,
//					footerRows: footerData,
					gridExpandHeight: 250
					//					showPager: true,
					//					customPager: true,
					//					pagerOption: {
					//						startPerPage: 50,
					//						totalItem: report.TotalItem,
					//						itemsPerPageArray: [50, 100, 200, 500],
					//						itemsPerPage: scope.gridOption.itemsPerPage,
					//						currentPage: scope.gridOption.currentPage,
					//						pagerSelectCallBack: ReRenderAfterPaged
					//					}
				};

				$('#' + id).fanexGrid(gridOptions);
			});
		}
	};
});