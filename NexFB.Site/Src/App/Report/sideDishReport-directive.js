'use strict';
/*global reportModule*/

reportModule.directive('sideDishReport', function () {
    var self = this;

    self.columnHeaders = [
		{ HeaderText: "#", Width: 60, Name: "No", HeaderAlign: "center", CellAlign: "center", ShowCollapseExpand: true },
		{ HeaderText: "Date", Width: 90, Name: "WorkDate", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "dd-MMM-yy" },
		{ HeaderText: "Shift", Width: 150, Name: "Shift", HeaderAlign: "center", CellAlign: "center" },
		{ HeaderText: "Full Name", Width: 150, Name: "FullName", HeaderAlign: "center", CellAlign: "left" },
        { HeaderText: "Nick", Width: 150, Name: "NickName", HeaderAlign: "center", CellAlign: "left" },
        { HeaderText: "Food item (Vie)", Width: 150, Name: "FoodVNMName", HeaderAlign: "center", CellAlign: "left" },
        { HeaderText: "Food item (Eng)", Width: 150, Name: "FoodEngName", HeaderAlign: "center", CellAlign: "left" },
        { HeaderText: "Price", Width: 150, Name: "Price", HeaderAlign: "center", CellAlign: "right" },
        { HeaderText: "Quantity", Width: 150, Name: "Qty", HeaderAlign: "center", CellAlign: "right" },
        { HeaderText: "Amount", Width: 150, Name: "Amount", HeaderAlign: "center", CellAlign: "right", FormatNumber: "0,0" }
	];

    self.generateFooterRows = function (data) {
        var footerRows = [
            {
                Columns: [
                    { Value: "", Name: "No", MergeColNumber: 7, CellAlign: "left" },
                    { Value: "Total", Name: "Price", CellAlign: "center" },
                    { Value: data.TotalQuantity, Name: "Quantity", CellAlign: "right" },
                    { Value: data.TotalAmount, Name: "Amount", CellAlign: "right" }
                ]
            }
        ];

        return footerRows;
    };

    self.generateBodyRows = function () {
//        var rows = [];
//        for (var index = 0; index < numberParentRows; index++) {
//            var currentRow = {
//                Columns: [
//                    { Name: "No", Value: 'Group - ' + index, MergeColNumber: 3 },
//                    { Name: "Number2", Value: 'Group - ' + index, MergeColNumber: 7 }
//                ],
//                SubRows: []
//            }

//            for (var j = 0; j < 5; j++) {
//                var currentSubRow = {
//                    Columns: [
//                    { Name: "No", Value: 'Group - ' + index + ' - ' + j, MergeColNumber: 3 },
//                    { Name: "Number2", Value: 'Group - ' + index + ' - ' + j, MergeColNumber: 7 }
//                ]
//                };
//                currentRow.SubRows.push(currentSubRow);
//            }

//            rows.push(currentRow);
//        }

//        return rows;
    };

    return {
        link: function (scope, element) {
            scope.$on('renderSideDishReport', function () {
                var footerRows = self.generateFooterRows(scope.reportData);

                var gridOptions = {
                    columns: self.columnHeaders,
                    bodyRows: bodyData,
                    showTree: true,
                    showTitle: true,
                    titleInfo: {
                        text: "Side Dish Report",
                        showExcelButton: true,
                        exportExcelFunction: function () {
                            scope.exportExcel.call(this, scope.filter);
                        }
                    },
                    showFooter: true,
                    footerRows: footerRows,
                    gridExpandHeight: 250,
                    showPager: true,
                    pagerOption: {
                        itemsPerPage: 50,
                        currentPage: 0,
                        itemsPerPageArray: [50, 100, 200, 500],
                        showDetail: true,
                        afterPagedFunction: function () {

                        }
                    }
                };

                $(element).fanexGrid(gridOptions);
            });
        }
    };
});