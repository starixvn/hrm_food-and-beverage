'use strict';
/*global reportModule, moment*/

reportModule.directive('renderMonthlyReport', function () {
    var generateNumberMealsRow = function (numberMealsMonth) {
        var numberMealsRow = [],
            numberMeals = numberMealsMonth.NumberMeals;

        for (var i = 0; i < numberMeals.length; i++) {
            var row = { Name: "" + (i + 1), Value: numberMeals[i] };
            numberMealsRow.push(row);
        }

        numberMealsRow.push({ Name: "Quantity", Value: numberMealsMonth.TotalQuantity });
        numberMealsRow.push({ Name: "Amount", Value: numberMealsMonth.Amount });

        return numberMealsRow;
    };

    var generateDataBody = function (report, scope) {
        var rows = [];

        if (report && report.Meals) {
            var currentSubRow = [],
                rowIndex = scope.gridOption.startIndex + 1;

            for (var i = 0; i < report.Meals.length; i++) {
                var meal = report.Meals[i],
                    currentRow = [],
                    mealItems = meal.MealItems;

                if (report.GroupBy == 0) {
                    currentRow = {
                        Columns: [
                            { Name: "Meal", Value: meal.Meal.MealEngName, MergeColNumber: 5, CellAlign: "left" },
                            { Name: "1", Value: "", MergeColNumber: 33 }
                        ],
                        SubRows: [],
                        CssClass: 'parent-row-background'
                    };
                }
                else {
                    currentRow = {
                        Columns: [
                            { Name: "Meal", Value: meal.Meal.MealEngName, MergeColNumber: 2, CellAlign: "left" },
                            { Name: "1", Value: "", MergeColNumber: 33, TdClass: "meal-background" }
                        ],
                        SubRows: [],
                        CssClass: 'parent-row-background'
                    };
                }
                for (var j = 0; j < mealItems.length; j++) {
                    var mealItem = mealItems[j];

                    if (report.GroupBy == 0) {
                        currentSubRow = {
                            Columns: [
                                { Name: "Meal", Value: rowIndex, CssClass: "remove-padding-left" },
                                { Name: "NickName", Value: mealItem.Employee.NickName },
                                { Name: "EmployeeType", Value: mealItem.EmployeeType.EmployeeTypeName },
                                { Name: "Department", Value: mealItem.Department.Name },
                                { Name: "Team", Value: mealItem.Team.Name }
                            ]
                        };
                    }
                    else {
                        currentSubRow = {
                            Columns: [
                                { Name: "Meal", Value: rowIndex, CssClass: "remove-padding-left" },
                                {
                                    Name: report.GroupBy == 1 ? "Department" : "Cost Center",
                                    Value: report.GroupBy == 1 ? mealItem.Department.Name + ' > ' + mealItem.Team.Name : mealItem.CostCenter
                                }
                            ]
                        };
                    }

                    var numberMealsRow = generateNumberMealsRow(mealItem.NumberMealsMonth);
                    currentSubRow.Columns = currentSubRow.Columns.concat(numberMealsRow);
                    currentRow.SubRows.push(currentSubRow);
                    rowIndex++;
                }
                rows.push(currentRow);

                if (report.GroupBy === 2) {
                    var subTotalRow = generateSubTotalRow(meal, report.GroupBy);
                    rows.push(subTotalRow);
                }
            }
        }
        return rows;
    };

    var generateSubTotalRow = function (meal, groupBy) {
        var currentRow = [];
        var mergeColumn = groupBy === 0 ? 5 : 2;

        var columns = [{ Name: "Meal", Value: "Total", MergeColNumber: mergeColumn, CellAlign: "center" }];

        if (meal.TotalQuantityPerDays) {
            for (var i = 0; i < meal.TotalQuantityPerDays.length; i++) {
                var value = {
                    Name: "" + (i + 1),
                    Value: meal.TotalQuantityPerDays[i],
                    MergeColNumber: 1,
                    CellAlign: "right"
                };
                columns.push(value);
            }
        }
        columns.push({ Name: "Quantity", Value: meal.TotalQuantityOfMeal, MergeColNumber: 1, CellAlign: "right" })
        columns.push({ Name: "Amount", Value: meal.TotalAmountOfMeal, MergeColNumber: 1, CellAlign: "right" })

        currentRow = {
            Columns: columns,
            CssClass: 'total-price'
        };

        return currentRow;
    }

    var generateGroupHeader = function (month, groupBy, monthFormat) {
        var groupHeader = [];

        if (groupBy == 0) {
            groupHeader = [
                {
                    Columns: [
                        { HeaderText: "#", Name: "Meal", MergeRowNumber: 2 },
                        { HeaderText: "Nick", Name: "NickName", MergeRowNumber: 2 },
                        { HeaderText: "Employee Type", Name: "EmployeeType", MergeRowNumber: 2 },
                        { HeaderText: "Department", Name: "Department", MergeRowNumber: 2 },
                        { HeaderText: "Team", Name: "Team", MergeRowNumber: 2 },
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
                        { HeaderText: groupBy == 1 ? "Team" : "Cost Center", Name: groupBy == 1 ? "Department" : "Cost Center", MergeRowNumber: 2 },
                        { HeaderText: "" + moment(month).format(monthFormat), Name: "1", MergeColNumber: 31 },
                        { HeaderText: "Total", Name: "Quantity", MergeColNumber: 2 }
                    ]
                }
            ];
        }

        return groupHeader;
    };

    var generateDataHeader = function (weekdays, groupBy, numberFormat) {
        var columns = [];

        if (groupBy == 0) {
            columns = [
                { HeaderText: "#", Width: 60, Name: "Meal", HeaderAlign: "center", CellAlign: "center", ShowCollapseExpand: true },
                { HeaderText: "Nick", Width: 90, Name: "NickName", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: "Employee Type", Width: 150, Name: "EmployeeType", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: "Department", Width: 100, Name: "Department", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: "Team", Width: 150, Name: "Team", HeaderAlign: "center", CellAlign: "left" }
            ];
        }
        else {
            columns = [
                { HeaderText: "#", Width: 60, Name: "Meal", HeaderAlign: "center", CellAlign: "center", ShowCollapseExpand: true },
                {
                    HeaderText: groupBy == 1 ? "Team" : "Cost Center",
                    Width: 150, Name: groupBy == 1 ? "Department" : "Cost Center",
                    HeaderAlign: "center",
                    CellAlign: "left"
                }
            ];
        }

        var numberMealsLength = weekdays ? weekdays.NumberMeals.length : 31;

        for (var i = 0; i < numberMealsLength; i++) {
            var currentDate = weekdays.NumberMeals[i],
                weekendClass = currentDate === 7 || currentDate === 1 ? "red" : "",
                mealHeader = { HeaderText: "" + (i + 1), Width: 45, Name: "" + (i + 1), HeaderAlign: "center", CellAlign: "right", CssClass: weekendClass };
            columns.push(mealHeader);
        }

        columns.push({ HeaderText: "Qty", Width: 60, Name: "Quantity", HeaderAlign: "center", CellAlign: "right" });
        columns.push({ HeaderText: "Amount", Width: 80, Name: "Amount", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat });

        return columns;
    };

    var generateDataFooter = function (weekdays, groupBy) {
        var columns = [],
            footerRow = [];

        if (weekdays) {
            if (groupBy == 0) {
                columns = [
                    { Value: "Total", Name: "Meal", MergeColNumber: 5 }
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
            scope.$on('renderMonthlyReport', function () {
                function ReRenderAfterPaged(pageIndex, itemsPerPage) {
                    scope.filter.isPagingCallback = true;
                    scope.filter.startIndex = pageIndex >= 0 ? (pageIndex * itemsPerPage) : 0;
                    scope.filter.itemsPerPage = itemsPerPage;

                    scope.gridOption.itemsPerPage = itemsPerPage;
                    scope.gridOption.currentPage = pageIndex;
                    scope.gridOption.startIndex = scope.filter.startIndex;

                    scope.getMonthlyReportLocal.call(this, scope.filter);
                    return false;
                }

                var id = iElement.attr('id'),
                    report = scope.localReport,
                    bodyData = generateDataBody(report, scope),
                    groupHeaderData = generateGroupHeader(report.Month, report.GroupBy, scope.monthFormat),
                    headerData = generateDataHeader(report.WeekDays, report.GroupBy, scope.numberFormat),
                    footerData = generateDataFooter(report.Total, report.GroupBy);

                var gridOptions = {
                    customizeHeader: true,
                    groupHeaderRows: groupHeaderData,
                    columns: headerData,
                    bodyRows: bodyData,
                    showTree: true,
                    showTitle: true,
                    titleInfo:
                    {
                        text: "Monthly Main Dish Report - Per Meal",
                        showExcelButton: true,
                        exportExcelFunction: function () {
                            scope.exportExcel.call(this, scope.filter);
                        }
                    },
                    numberOfFrozenColumn: report.GroupBy == 0 ? 5 : 2,
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

reportModule.directive('exportMonthlyMainDishLocalReport', ['baseUrl', function (baseUrl) {
    return {
        link: function (scope) {
            scope.$on('exportMonthlyMainDishLocalReport', function () {
                var data = $.param(scope.criteria),
                    url = baseUrl + 'ExportReport/ExportMonthlyMainDishReport',
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

reportModule.directive('resetScroll', ['$timeout', function (timer) {
    return {
        restrict: 'A',
        link: function (scope, iElement) {
            iElement.click(function () {
                var resetScroll = function () {
                    scope.$watch(
                        function () { return $('.x-scrollbar').length; },
                        function (newValue) {
                            if (newValue) {
                                $(".x-scrollbar").scrollLeft(1, 0);
                                $('.y-scrollbar').scrollTop(1, 1);
                            }
                        }
                    );
                };
                timer(resetScroll, 500);
            });
        }
    };
}]);