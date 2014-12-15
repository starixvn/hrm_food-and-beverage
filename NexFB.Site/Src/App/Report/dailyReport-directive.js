'use strict';
/*global reportModule*/

reportModule.directive('renderDailyReport', function () {
//	var generateDataBody = function (report, scope) {
//		var rows = [];

//		if (report && report.Meals) {
//			var currentSubRow = [],
//				rowIndex = scope.gridOption.startIndex + 1;
//		}

//		return rows;
//	};

//	return {
//		link: function (scope, iElement) {
//			var id = iElement.attr('id'),
//				report = scope.localReport,
//				bodyData = generateDataBody(report, scope),
//				headerData = generateDataHeader(report.WeekDays, report.IsGroupByEmployee, scope.numberFormat),
//				footerData = generateDataFooter(report.Total, report.IsGroupByEmployee);

//			var gridOptions = {
//				columns: headerData,
//				bodyRows: bodyData,
//				showTree: true,
//				showTitle: true,
//				titleInfo:
//						{
//							text: "Daily Main Dish Report - Price Per Meal",
//							showExcelButton: true,
//							exportExcelFunction: function () {
//								scope.exportExcel.call(this, scope.filter);
//							}
//						},
//				showFooter: true,
//				footerRows: footerData,
//				gridExpandHeight: 250,
//				showPager: true,
//				customPager: true,
//				pagerOption: {
//					startPerPage: 50,
//					totalItem: report.TotalItem,
//					itemsPerPageArray: [50, 100, 200, 500],
//					itemsPerPage: scope.gridOption.itemsPerPage,
//					currentPage: scope.gridOption.currentPage,
//					pagerSelectCallBack: ReRenderAfterPaged
//				}
//			};

//			$('#' + id).fanexGrid(gridOptions);
//		}
//	};
});