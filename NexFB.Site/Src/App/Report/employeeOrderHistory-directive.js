'use strict';
/*global reportModule,moment,numeral*/

reportModule.directive('renderEmployeeOrderHistoryReport', function () {

	var generateDataBody = function (report, scope) {
		var createMyMenu = function (currentCell) {
			var menus = currentCell.Value,
			cell = '';

			for (var i = 0; i < menus.length; i++) {
				var menu = menus[i],
				menuHtml = '<ul class="list-order list-unstyled list-employee-menu"><li>' +
					menu.QuantityOrder + ' x "' + menu.NameEN + '"</li></ul>';

				cell += menuHtml;
			}

			return cell;
		};

		var createMyAmount = function (currentCell) {
			var menus = currentCell.Value,
				amount = 0;

			if (menus && menus.length > 0) {
				for (var i = 0; i < menus.length; i++) {
					var menu = menus[i],
						value = !menu.IsMainMenu ? menu.TotalAmount : 0;

					if (menu.IsMainMenu && menus.length === 1) {
						return 'N/A';
					}

					amount += value;
				}
			}
			else {
				return '';
			}

			return numeral(amount).format(scope.numberFormat);
		};

		var getCssClass = function (statusId, orderStatus) {
			var cssClass = '';

			switch (statusId) {
				case orderStatus.New:
					cssClass = 'lbl-new';
					break;
				case orderStatus.Accepted:
					cssClass = 'lbl-received';
					break;
				case orderStatus.Delivered:
					cssClass = 'lbl-delivered';
					break;
				case orderStatus.Reduced:
					cssClass = 'lbl-reduced';
					break;
				case orderStatus.Edited:
					cssClass = 'lbl-edited';
					break;
				case orderStatus.Rejected:
					cssClass = 'lbl-rejected';
					break;
			}

			return cssClass;
		};

		var rows = [];

		if (report && report.EmployeeOrders) {
			var numberEmployeeOrders = report.EmployeeOrders.length,
				rowIndex = scope.gridOption.startIndex + 1;

			for (var i = 0; i < numberEmployeeOrders; i++) {
				var employeeOrder = report.EmployeeOrders[i],
					currentRow = [],
					cssClass = getCssClass(employeeOrder.Status.StatusId, scope.orderStatus);

				currentRow = {
					Columns: [
						{ Name: "Count", Value: rowIndex },
						{ Name: "Date", Value: moment(employeeOrder.OnDate).format(scope.dateFormat) },
						{ Name: "Meal", Value: employeeOrder.Meal.MealEngName },
						{ Name: "Order", Value: employeeOrder.Order.OrderNo },
						{ Name: "Status", Value: employeeOrder.Status.Name, CssClass: cssClass },
						{ Name: "OrderedBy", Value: employeeOrder.OrderBy.NickName },
						{ Name: "OrderPlatform", Value: employeeOrder.OrderFrom },
						{ Name: "MyMenu", Value: employeeOrder.Menus, CustomCellRender: createMyMenu }
					]
				};

				var totalAmount = '';

				if (employeeOrder.Status.StatusId === scope.orderStatus.Rejected) {
					totalAmount = { Name: "MyAmount", Value: 'N/A' };
				}
				else {
					totalAmount = { Name: "MyAmount", Value: employeeOrder.Menus, CustomCellRender: createMyAmount };
				}

				currentRow.Columns.push(totalAmount);
				rows.push(currentRow);
				rowIndex++;
			}
		}

		return rows;
	};

	var generateDataHeader = function (numberFormat) {
		var columns = [
			{ HeaderText: "#", Width: 80, Name: "Count", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Date", Width: 100, Name: "Date", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Meal", Width: 90, Name: "Meal", HeaderAlign: "center", CellAlign: "center" },
			{ HeaderText: "Order", Width: 90, Name: "Order", HeaderAlign: "center", CellAlign: "center", FormatNumber: numberFormat },
			{ HeaderText: "Status", Width: 100, Name: "Status", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "Ordered By", Width: 120, Name: "OrderedBy", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "Order Platform", Width: 150, Name: "OrderPlatform", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "My Menu", Width: 335, Name: "MyMenu", HeaderAlign: "center", CellAlign: "left" },
			{ HeaderText: "My Amount", Width: 112, Name: "MyAmount", HeaderAlign: "center", CellAlign: "right" }
		];

		return columns;
	};

	var generateDataFooter = function (totalAmount, numberFormat) {
		var footerRow = [];

		footerRow = [
				{
					Columns: [
						{ Value: "Total", Name: "Date", MergeColNumber: 8, CellAlign: "right" },
						{ Value: numeral(totalAmount).format(numberFormat), Name: "MyAmount" }
					]
				}
			];

		return footerRow;
	};

	return {
		link: function (scope, iElement) {
			scope.$on('renderEmployeeOrderHistoryReport', function () {

				function ReRenderAfterPaged(pageIndex, itemsPerPage) {
					scope.filter.isPagingCallback = true;
					scope.filter.startIndex = pageIndex >= 0 ? (pageIndex * itemsPerPage) : 0;
					scope.filter.itemsPerPage = itemsPerPage;

					scope.gridOption.itemsPerPage = itemsPerPage;
					scope.gridOption.currentPage = pageIndex;
					scope.gridOption.startIndex = scope.filter.startIndex;

					scope.getEmployeeOrderHistoryReport.call(this, scope.filter);
					return false;
				}

				var id = iElement.attr('id'),
					report = scope.report,
					bodyData = generateDataBody(report, scope),
					headerData = generateDataHeader(scope.numberFormat),
					footerData = generateDataFooter(report.TotalAmount, scope.numberFormat);

				var gridOptions = {
					columns: headerData,
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

reportModule.directive('exportEmployeeOrderHistoryReport', ['baseUrl', function (baseUrl) {
	return {
		link: function (scope) {
			scope.$on('exportEmployeeOrderHistoryReport', function () {
				var data = $.param(scope.criteria),
					url = baseUrl + 'ExportReport/ExportEmployeeOrderHistoryReport',
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