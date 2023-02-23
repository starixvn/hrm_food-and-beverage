'use strict';
/*global reportModule,moment,numeral*/

reportModule.directive('renderAdminOrderHistoryReport', function () {

    var generateDataBody = function (report, scope) {
        var createOrderDetail = function (currentCell) {
            var orderId = currentCell.Value;
            var details = "<div class='text-center admin-order-action'>";
            details += "<a class='order-detail-edit glyphicon glyphicon-edit' href='javascript:;' order-detail-number ='" + orderId + "'></a>";
            details += "<a class='order-detail glyphicon glyphicon-list-alt' href='javascript:;' order-detail-number ='" + orderId + "'></a>";
            details += "</div>";
            return details;
        };

        var checkHasSideDish = function (menus) {
            for (var i = 0; i < menus.length; i++) {
                var menu = menus[i];

                if (menu.IsSideDish) {
                    return true;
                }
            }

            return false;
        };

        var getAmount = function (currentCell) {
            var menus = currentCell.Value,
                amount = 0;

            if (menus && menus.length > 0) {
                var hasSideDish = checkHasSideDish(menus);

                if (hasSideDish) {
                    for (var i = 0; i < menus.length; i++) {
                        var menu = menus[i],
                            value = !menu.IsMainMenu ? menu.TotalAmount : 0;

                        amount += value;
                    }
                }
                else {
                    return 'N/A';
                }
            }
            else {
                return 'N/A';
            }

            return numeral(amount).format(scope.numberFormat);
        };

        var getMainDish = function (currentCell) {
            var menus = currentCell.Value,
                totalMainDish = 0;

            for (var i = 0; i < menus.length; i++) {
                var menu = menus[i];

                if (menu.IsMainMenu) {
                    totalMainDish += menu.QuantityOrder;
                }
            }

            return totalMainDish;
        };

        var getSideDish = function (currentCell) {
            var menus = currentCell.Value,
                totalSideDish = 0;

            for (var i = 0; i < menus.length; i++) {
                var menu = menus[i];

                if (menu.IsSideDish) {
                    totalSideDish += menu.QuantityOrder;
                }
            }

            return totalSideDish;
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

        var selectMeal = function (currentCell) {
            var result = "<div class='meal-list'>";
            result += "<a href='javascript:;' class='btn-expand-meal-list' order-id='"
                + currentCell.Value.Order.OrderId + "'>" + currentCell.Value.Meal.MealEngName + "</a>";
            result += "</div>"

            return result;
        }

        var formatOrderTime = function (time) {
            var date = new Date(time);
            return date.toLocaleString();
        }

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
                        { Name: "Time", Value: formatOrderTime(moment(employeeOrder.OrderTime)) },
                        { Name: "Meal", Value: employeeOrder, CustomCellRender: selectMeal },
                        { Name: "Order", Value: employeeOrder.Order.OrderNo },
                        { Name: "Ref", Value: employeeOrder.Order.OrderId },
                        { Name: "OrderPlatform", Value: employeeOrder.OrderFrom },
                        { Name: "Status", Value: employeeOrder.Status.Name, CssClass: cssClass },
                        { Name: "OrderedBy", Value: employeeOrder.OrderBy.NickName },
                        { Name: "FullName", Value: employeeOrder.OrderBy.FullName },
                        { Name: "Section", Value: employeeOrder.OrderBy.Team },
                        { Name: "Company", Value: employeeOrder.OrderBy.CompanyName },
                        { Name: "MainDish", Value: employeeOrder.Menus, CustomCellRender: getMainDish },
                        { Name: "SideDish", Value: employeeOrder.Menus, CustomCellRender: getSideDish }
                    ]
                };

                var totalAmount = '';

                if (employeeOrder.Status.StatusId === scope.orderStatus.Rejected) {
                    totalAmount = { Name: "Amount", Value: 'N/A' };
                }
                else {
                    totalAmount = { Name: "Amount", Value: employeeOrder.Menus, CustomCellRender: getAmount };
                }

                currentRow.Columns.push(totalAmount);

                currentRow.Columns.push({ Name: "OrderDetail", Value: employeeOrder.Order.OrderId, CustomCellRender: createOrderDetail }); //createOrderDetail

                rows.push(currentRow);
                rowIndex++;
            }
        }

        return rows;
    };

    var generateDataHeader = function (numberFormat) {
        var columns = [
            { HeaderText: "#", Width: 70, Name: "Count", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Time", Width: 80, Name: "Time", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Meal", Width: 90, Name: "Meal", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Order #", Width: 90, Name: "Order", HeaderAlign: "center", CellAlign: "center", FormatNumber: numberFormat },
            { HeaderText: "Ref", Width: 90, Name: "Ref", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Order Platform", Width: 150, Name: "OrderPlatform", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Status", Width: 100, Name: "Status", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Ordered By", Width: 120, Name: "OrderedBy", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Full Name", Width: 120, Name: "FullName", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Section", Width: 120, Name: "Section", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Company", Width: 80, Name: "Company", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Main Dish", Width: 80, Name: "MainDish", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Side Dish", Width: 80, Name: "SideDish", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Amount", Width: 100, Name: "Amount", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Order Detail", Width: 100, Name: "OrderDetail", HeaderAlign: "center", CellAlign: "center" }
        ];

        return columns;
    };

    var generateDataFooter = function (report, numberFormat) {
        var footerRow = [];
        var totalAmount = report.TotalAmount;
        var totalMainDish = report.TotalMainDish;
        var totalSideDish = report.TotalSideDish;

        footerRow = [
            {
                Columns: [
                    { Name: "Count", Value: "Total", MergeColNumber: 11, CellAlign: "right" },
                    { Name: "MainDish", Value: totalMainDish, CellAlign: "right" },
                    { Name: "SideDish", Value: totalSideDish, CellAlign: "right" },
                    { Name: "Amount", Value: numeral(totalAmount).format(numberFormat), CellAlign: "right" }
                ]
            }
        ];

        return footerRow;
    };

    return {
        link: function (scope, iElement) {
            scope.$on('renderAdminOrderHistoryReport', function () {

                function ReRenderAfterPaged(pageIndex, itemsPerPage) {
                    scope.filter.isPagingCallback = true;
                    scope.filter.startIndex = pageIndex >= 0 ? (pageIndex * itemsPerPage) : 0;
                    scope.filter.itemsPerPage = itemsPerPage;

                    scope.gridOption.itemsPerPage = itemsPerPage;
                    scope.gridOption.currentPage = pageIndex;
                    scope.gridOption.startIndex = scope.filter.startIndex;

                    scope.getAdminOrderHistoryReport.call(this, scope.filter);
                    return false;
                }

                var id = iElement.attr('id'),
                    report = scope.report,
                    bodyData = generateDataBody(report, scope),
                    headerData = generateDataHeader(scope.numberFormat),
                    footerData = generateDataFooter(report, scope.numberFormat);

                var gridOptions = {
                    columns: headerData,
                    bodyRows: bodyData,
                    showTree: true,
                    showTitle: true,
                    titleInfo: {
                        text: "Admin Order History Report - " + moment(scope.filter.fromDate).format(scope.dateFormat)
                            + " to " + moment(scope.filter.toDate).format(scope.dateFormat),
                        showExcelButton: true,
                        exportExcelFunction: function () {
                            scope.exportExcel.call(this, scope.filter);
                        }
                    },
                    showFooter: true,
                    footerRows: footerData,
                    gridExpandHeight: 300,
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

                iElement.find('a.order-detail').on('click', function () {
                    var orderId = $(this).attr('order-detail-number');
                    scope.showOrderDetail(orderId);
                });

                iElement.find('a.order-detail-edit').on('click', function () {
                    var orderId = $(this).attr('order-detail-number');
                    scope.showEditOrderDetail(orderId);
                });

                iElement.find('a.btn-expand-meal-list').on('click', function () {
                    var mealSelection = $(".meal-list-item")[0];
                    var position = $(this).position();

                    mealSelection.style.left = (position.left - 16) + "px";
                    mealSelection.style.top = (position.top + 250) + "px";

                    scope.selectedOrderId = $(this).attr('order-id');;
                    scope.showMealList(true);
                });
            });
        }
    };
});

reportModule.directive('exportAdminOrderHistoryReport', ['baseUrl', function (baseUrl) {
    return {
        link: function (scope) {
            scope.$on('exportAdminOrderHistoryReport', function () {
                var data = $.param(scope.criteria),
                    url = baseUrl + 'ExportReport/ExportAdminOrderHistoryReport',
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
}]);

reportModule.directive('onDocumentClick', ['$document',
    function ($document) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {

                var onClick = function () {
                    scope.$apply(function () {
                        scope.$eval(attrs.onDocumentClick);
                    });
                };

                $document.on('click', onClick);

                scope.$on('$destroy', function () {
                    $document.off('click', onClick);
                });
            }
        };
    }
]);