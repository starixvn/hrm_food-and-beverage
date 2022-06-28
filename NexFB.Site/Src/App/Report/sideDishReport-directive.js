'use strict';
/*global reportModule*/

reportModule.directive('sideDishReport', function () {
    var columnHeaders = [
        { HeaderText: "#", Width: 60, Name: "No", HeaderAlign: "center", CellAlign: "center", ShowCollapseExpand: true },
        { HeaderText: "Date", Width: 90, Name: "WorkDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "DD-MMM-YYYY" },
        { HeaderText: "Shift", Width: 80, Name: "Shift", HeaderAlign: "center", CellAlign: "center" },
        { HeaderText: "Full Name", Width: 170, Name: "FullName", HeaderAlign: "center", CellAlign: "left" },
        { HeaderText: "Nick", Width: 110, Name: "NickName", HeaderAlign: "center", CellAlign: "left" },
        { HeaderText: "Section", Width: 150, Name: "Section", HeaderAlign: "center", CellAlign: "left" },
        { HeaderText: "Side Dish (Vie)", Width: 170, Name: "FoodVNMName", HeaderAlign: "center", CellAlign: "left" },
        { HeaderText: "Side Dish (Eng)", Width: 170, Name: "FoodEngName", HeaderAlign: "center", CellAlign: "left" },
        { HeaderText: "Price", Width: 110, Name: "Price", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0" },
        { HeaderText: "Quantity", Width: 80, Name: "Qty", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0" },
        { HeaderText: "Amount", Width: 120, Name: "Amount", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0" }
    ];

    var generateFooterRows = function (data) {
        var footerRows = [
            {
                Columns: [
                    { Value: "", Name: "No", MergeColNumber: 8, CellAlign: "left" },
                    { Value: "Total", Name: "Price", CellAlign: "center" },
                    { Value: data.TotalQuantity.toString(), Name: "Qty", CellAlign: "right", FormatNumber: "0,0" },
                    { Value: data.TotalAmount.toString(), Name: "Amount", CellAlign: "right", FormatNumber: "0,0" }
                ]
            }
        ];

        return footerRows;
    };

    var generateBodyRows = function (data, pageIndex, itemsPerPage) {
        var rows = [];

        if (data) {
            var counter = (pageIndex * itemsPerPage) + 1;

            angular.forEach(data.SideDishItems, function (item) {
                //var currentRow = {};

                //var result = $.grep(rows, function (e) { return e.Columns[0].Value === item.DepartmentTeamName; });

                //if (result.length > 0) {
                //    currentRow = result[0];
                //} else {
                //    currentRow = {
                //        Columns: [
                //            //{ Name: "No", Value: item.DepartmentTeamName, MergeColNumber: 11, CellAlign: "left" }
                //        ],
                //        SubRows: []
                //    };

                //    rows.push(currentRow);
                //}

                //var currentSubRow = {
                //    Columns: [
                //        { Name: "No", Value: counter },
                //        { Name: "WorkDate", Value: item.WorkDate },
                //        { Name: "Shift", Value: item.Shift },
                //        { Name: "FullName", Value: item.FullName },
                //        { Name: "NickName", Value: item.NickName },
                //        { Name: "Section", Value: item.Team },
                //        { Name: "FoodVNMName", Value: item.FoodVNMName },
                //        { Name: "FoodEngName", Value: item.FoodEngName },
                //        { Name: "Price", Value: item.Price },
                //        { Name: "Qty", Value: item.Qty },
                //        { Name: "Amount", Value: item.Amount }
                //    ]
                //};

                //currentRow.SubRows.push(currentSubRow);

                //new render way: without section row
                var currentRow = {
                    Columns: [
                        { Name: "No", Value: counter },
                        { Name: "WorkDate", Value: item.WorkDate },
                        { Name: "Shift", Value: item.Shift },
                        { Name: "FullName", Value: item.FullName },
                        { Name: "NickName", Value: item.NickName },
                        { Name: "Section", Value: item.Team },
                        { Name: "FoodVNMName", Value: item.FoodVNMName },
                        { Name: "FoodEngName", Value: item.FoodEngName },
                        { Name: "Price", Value: item.Price },
                        { Name: "Qty", Value: item.Qty },
                        { Name: "Amount", Value: item.Amount }
                    ]
                }
                rows.push(currentRow);

                counter++;
            });
        }

        return rows;
    };

    return {
        link: function (scope, element) {
            scope.$on('renderSideDishReport', function () {
                var footerRows = generateFooterRows(scope.reportData),
                    bodyRows = generateBodyRows(scope.reportData, scope.criteria.pageIndex, scope.criteria.itemsPerPage);

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
                    gridExpandHeight: 250,
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