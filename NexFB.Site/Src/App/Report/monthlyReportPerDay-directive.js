'use strict';
/*global reportModule, moment*/

reportModule.directive('renderMonthlyMainDishPerDay', function () {
    var generateNumberMealsRow = function (numberMealsMonth) {
        var numberMealsRow = [],
            numberMeals = numberMealsMonth.NumberMeals;

        for (var i = 0; i < numberMeals.length; i++) {
            var row = { Name: "" + (i + 1), Value: numberMeals[i] };
            numberMealsRow.push(row);
        }

        numberMealsRow.push({ Name: "Quantity", Value: numberMealsMonth.TotalQuantity });
        numberMealsRow.push({ Name: "Amount", Value: numberMealsMonth.Amount, CssClass: "bold" });

        return numberMealsRow;
    };

    var generateDataBody = function (report, scope) {
        var rows = [];

        if (report && report.TypeDays) {
            var currentSubRow = [],
                rowIndex = scope.gridOption.startIndex + 1;

            for (var i = 0; i < report.TypeDays.length; i++) {
                var typeDay = report.TypeDays[i],
                    currentRow = [],
                    mealItems = typeDay.MealItems;

                if (report.IsGroupByEmployee) {
                    currentRow = {
                        Columns: [
                            { Name: "Meal", Value: typeDay.TypeDay, MergeColNumber: 4, CellAlign: "left" },
                            { Name: "1", Value: "", MergeColNumber: 33 }
                        ],
                        SubRows: [],
                        CssClass: 'parent-row-background'
                    };
                }
                else {
                    currentRow = {
                        Columns: [
                            { Name: "Meal", Value: typeDay.TypeDay, MergeColNumber: 2, CellAlign: "left" },
                            { Name: "1", Value: "", MergeColNumber: 33 }
                        ],
                        SubRows: [],
                        CssClass: 'parent-row-background'
                    };
                }

                for (var j = 0; j < mealItems.length; j++) {
                    var mealItem = mealItems[j];

                    if (report.IsGroupByEmployee) {
                        currentSubRow = {
                            Columns: [
                                { Name: "Meal", Value: rowIndex, CssClass: "remove-padding-left" },
                                { Name: "NickName", Value: mealItem.Employee.NickName },
                                { Name: "EmployeeType", Value: mealItem.EmployeeType.EmployeeTypeName },
                                { Name: "Company", Value: mealItem.Company.Name + ' > ' + mealItem.Team.Name }
                            ]
                        };
                    }
                    else {
                        currentSubRow = {
                            Columns: [
                                { Name: "Meal", Value: rowIndex, CssClass: "remove-padding-left" },
                                { Name: "Company", Value: mealItem.Company.Name + ' > ' + mealItem.Team.Name }
                            ]
                        };
                    }

                    var numberMealsRow = generateNumberMealsRow(mealItem.NumberMealsMonth);
                    currentSubRow.Columns = currentSubRow.Columns.concat(numberMealsRow);
                    currentRow.SubRows.push(currentSubRow);
                    rowIndex++;
                }

                rows.push(currentRow);
            }
        }

        return rows;
    };

    var generateGroupHeader = function (month, isGroupByEmployee, monthFormat) {
        var groupHeader = [];

        if (isGroupByEmployee) {
            groupHeader = [
                {
                    Columns: [
                        { HeaderText: "#", Name: "Meal", MergeRowNumber: 2 },
                        { HeaderText: "Nick", Name: "NickName", MergeRowNumber: 2 },
                        { HeaderText: "Employee Type", Name: "EmployeeType", MergeRowNumber: 2 },
                        { HeaderText: "Team", Name: "Company", MergeRowNumber: 2 },
                        { HeaderText: "" + moment(month).format(monthFormat), Name: "1", MergeColNumber: 31 },
                        { HeaderText: "Total", Name: "Quantity", MergeColNumber: 2 }
                    ]
                }
            ];
        }
        else {
            groupHeader = [
                {
                    Columns: [
                        { HeaderText: "#", Name: "Meal", MergeRowNumber: 2 },
                        { HeaderText: "Team", Name: "Company", MergeRowNumber: 2 },
                        { HeaderText: "" + moment(month).format(monthFormat), Name: "1", MergeColNumber: 31 },
                        { HeaderText: "Total", Name: "Quantity", MergeColNumber: 2 }
                    ]
                }
            ];
        }

        return groupHeader;
    };

    var generateDataHeader = function (weekdays, isGroupByEmployee, numberFormat) {
        var columns = [];

        if (isGroupByEmployee) {
            columns = [
                { HeaderText: "#", Width: 60, Name: "Meal", HeaderAlign: "center", CellAlign: "center", ShowCollapseExpand: true },
                { HeaderText: "Nick", Width: 100, Name: "NickName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Employee Type", Width: 150, Name: "EmployeeType", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: "Team", Width: 150, Name: "Company", HeaderAlign: "center", CellAlign: "left" }
            ];
        }
        else {
            columns = [
                { HeaderText: "#", Width: 60, Name: "Meal", HeaderAlign: "center", CellAlign: "center", ShowCollapseExpand: true },
                { HeaderText: "Team", Width: 150, Name: "Company", HeaderAlign: "center", CellAlign: "left" }
            ];
        }

        var numberMealsLength = weekdays ? weekdays.NumberMeals.length : 31;

        for (var i = 0; i < numberMealsLength; i++) {
            var currentDate = weekdays.NumberMeals[i],
                weekendClass = currentDate === 7 || currentDate === 1 ? "red" : "",
                mealHeader = { HeaderText: "" + (i + 1), Width: 30, Name: "" + (i + 1), HeaderAlign: "center", CellAlign: "right", CssClass: weekendClass };
            columns.push(mealHeader);
        }

        columns.push({ HeaderText: "Qty", Width: 80, Name: "Quantity", HeaderAlign: "center", CellAlign: "right" });
        columns.push({ HeaderText: "Amount", Width: 80, Name: "Amount", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat });

        return columns;
    };

    var generateDataFooter = function (weekdays, isGroupByEmployee) {
        var columns = [],
            footerRow = [];

        if (weekdays) {
            if (isGroupByEmployee) {
                columns = [
                    { Value: "Total", Name: "Meal", MergeColNumber: 4 }
                ];
            }
            else {
                columns = [
                    { Value: "Total", Name: "Meal", MergeColNumber: 2 }
                ];
            }

            var numberMealsLength = weekdays.NumberMeals.length;

            for (var i = 0; i < numberMealsLength; i++) {
                var mealFooter = { Value: weekdays.NumberMeals[i], Name: "" + (i + 1), CellAlign: 'right' };
                columns.push(mealFooter);
            }

            columns.push({ Value: weekdays.TotalQuantity, Name: "Quantity" });
            columns.push({ Value: weekdays.Amount, Name: "Amount" });

            footerRow = [
                {
                    Columns: columns
                }
            ];
        }

        return footerRow;
    };

    return {
        link: function (scope, iElement) {
            scope.$on('renderMonthlyMainDishPerDay', function () {
                function ReRenderAfterPaged(pageIndex, itemsPerPage) {
                    scope.filter.isPagingCallback = true;
                    scope.filter.startIndex = pageIndex >= 0 ? (pageIndex * itemsPerPage) : 0;
                    scope.filter.itemsPerPage = itemsPerPage;

                    scope.gridOption.itemsPerPage = itemsPerPage;
                    scope.gridOption.currentPage = pageIndex;
                    scope.gridOption.startIndex = scope.filter.startIndex;

                    scope.getMonthlyMainDishReportPerDay.call(this, scope.filter);
                    return false;
                }

                var id = iElement.attr('id'),
                    report = scope.report,
                    bodyData = generateDataBody(report, scope),
                    groupHeaderData = generateGroupHeader(report.Month, report.IsGroupByEmployee, scope.monthFormat),
                    headerData = generateDataHeader(report.WeekDays, report.IsGroupByEmployee, scope.numberFormat),
                    footerData = generateDataFooter(report.Total, report.IsGroupByEmployee);

                var gridOptions = {
                    customizeHeader: true,
                    groupHeaderRows: groupHeaderData,
                    columns: headerData,
                    bodyRows: bodyData,
                    showTree: true,
                    showTitle: true,
                    titleInfo:
                    {
                        text: "Monthly Main Dish Report - Per Day",
                        showExcelButton: true,
                        exportExcelFunction: function () {
                            scope.exportExcel.call(this, scope.filter);
                        }
                    },
                    numberOfFrozenColumn: report.IsGroupByEmployee ? 4 : 2,
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
                    },
                    showHighlightRow: false
                };

                $('#' + id).fanexGrid(gridOptions);
            });
        }
    };
});

reportModule.directive('exportMonthlyMainDishPerDayReport', ['baseUrl', function (baseUrl) {
    return {
        link: function (scope) {
            scope.$on('exportMonthlyMainDishPerDayReport', function () {
                var data = $.param(scope.criteria),
                    url = baseUrl + 'ExportReport/ExportMonthlyMainDishPerDayReport',
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