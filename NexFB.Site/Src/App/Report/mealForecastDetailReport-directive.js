'use strict';
/*global reportModule, moment, numeral*/

reportModule.directive('renderMealForecastDetailReport', function () {
    var formatEfficiency = function (efficiency) {
        if (efficiency < 0) {
            return '-';
        }

        return efficiency + '%';
    };

    var getCssClass = function (efficiency) {
        var cssClass = '';

        if (efficiency < 0) {
            cssClass = 'td-efficiency-grey';
        }
        else if (efficiency < 100) {
            cssClass = 'td-efficiency-red';
        }
        else if (efficiency === 100) {
            cssClass = 'td-efficiency-white';
        }
        else {
            cssClass = 'td-efficiency-blue';
        }

        return cssClass;
    };

    var generateDataBody = function (report, scope) {
        var rows = [], i = 0;
        var mealForecastDetail = null, currentRow = [];

        var createMeal = function (currentCell) {
            var meals = currentCell.Value,
			cell = '';

            if (meals.length > 1) {
                for (var i = 0; i < meals.length; i++) {
                    var meal = meals[i],
						mealHtml = '<ul class="list-meal-forecast-detail"><li>' +
						meal.MealName + '</li></ul>';

                    cell += mealHtml;
                }
            }
            else {
                cell += meals[0].MealName;
            }

            return cell;
        };

        if (report !== null && report.MealForecastDetails.length > 0) {
            var numberMealForecastDetails = report.MealForecastDetails.length,
                rowIndex = scope.gridOption.startIndex + 1;

            for (i = 0; i < numberMealForecastDetails; i++) {
                mealForecastDetail = report.MealForecastDetails[i];
                currentRow = {
                    Columns: [
                        { Name: "Count", Value: rowIndex },
                        { Name: "MenuVie", Value: mealForecastDetail.MenuVie },
                        { Name: "MenuEng", Value: mealForecastDetail.MenuEng },
                        { Name: "Meal", Value: mealForecastDetail.Meals, CustomCellRender: createMeal },
                        { Name: "Vegetarian", Value: mealForecastDetail.IsVegetarian === false ? 'Non-Vegetarian' : 'Vegetarian' },
                        { Name: "Registration", Value: mealForecastDetail.Registration },
                        { Name: "ActualOrder", Value: mealForecastDetail.ActualOrder < 0 ? '-' : mealForecastDetail.ActualOrder },
                        { Name: "Efficiency", Value: formatEfficiency(mealForecastDetail.OrderRate),
                            TdClass: getCssClass(mealForecastDetail.OrderRate)
                        }
                    ]
                };
                rows.push(currentRow);
                rowIndex++;
            }
        }

        return rows;
    };

    var generateDataHeader = function (numberFormat) {
        var columns = [
            { HeaderText: "#", Width: 65, Name: "Count", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Menu (Vie)", Width: 160, Name: "MenuVie", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Menu (Eng)", Width: 160, Name: "MenuEng", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Meal", Width: 160, Name: "Meal", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Diet Type", Width: 160, Name: "Vegetarian", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Registration (1)", Width: 160, Name: "Registration", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat },
            { HeaderText: "Actual orders (2)", Width: 160, Name: "ActualOrder", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat },
            { HeaderText: "Order rate (2)/(1)", Width: 160, Name: "Efficiency", HeaderAlign: "center", CellAlign: "right" }
        ];

        return columns;
    };

    var generateDataFooter = function (report, scope) {
        if (report !== null && report.MealForecastDetails.length > 0) {
            var footerRows = [],
            totalNonVegetarian = {
                Columns: [
                    { Value: "Total Non-Vegetarian", Name: "Count", MergeColNumber: 5, CellAlign: "right" },
                    { Value: numeral(report.TotalNonVegetarian.Registration).format(scope.numberFormat), Name: "Registration" },
                    { Value: report.TotalNonVegetarian.ActualOrder < 0 ? '-' : numeral(report.TotalNonVegetarian.ActualOrder).format(scope.numberFormat),
                        Name: "ActualOrder"
                    },
                    { Value: formatEfficiency(report.TotalNonVegetarian.Efficiency), Name: "Efficiency",
                        TdClass: getCssClass(report.TotalNonVegetarian.Efficiency)
                    }
                ]
            },
            totalVegetarian = {
                Columns: [
                    { Value: "Total Vegetarian", Name: "Count", MergeColNumber: 5, CellAlign: "right" },
                    { Value: numeral(report.TotalVegetarian.Registration).format(scope.numberFormat), Name: "Registration" },
                    { Value: report.TotalVegetarian.ActualOrder < 0 ? '-' : numeral(report.TotalVegetarian.ActualOrder).format(scope.numberFormat),
                        Name: "ActualOrder"
                    },
                    { Value: formatEfficiency(report.TotalVegetarian.Efficiency), Name: "Efficiency",
                        TdClass: getCssClass(report.TotalVegetarian.Efficiency)
                    }
                ]
            },
            totalMeal = {
                Columns: [
                    { Value: "Total Meal", Name: "Count", MergeColNumber: 5, CellAlign: "right" },
                    { Value: numeral(report.TotalMeal.Registration).format(scope.numberFormat), Name: "Registration" },
                    { Value: report.TotalMeal.ActualOrder < 0 ? '-' : numeral(report.TotalMeal.ActualOrder).format(scope.numberFormat), Name: "ActualOrder" },
                    { Value: formatEfficiency(report.TotalMeal.Efficiency), Name: "Efficiency", TdClass: getCssClass(report.TotalMeal.Efficiency) }
                ]
            };

            if (scope.filter.dietType === '0') {
                footerRows.push(totalNonVegetarian);
            }
            else if (scope.filter.dietType === '1') {
                footerRows.push(totalVegetarian);
            }
            else {
                footerRows.push(totalNonVegetarian, totalVegetarian, totalMeal);
            }

            return footerRows;
        }
    };

    return {
        link: function (scope, iElement) {
            scope.$on("renderMealForecastDetailReport", function () {
                function ReRenderAfterPaged(pageIndex, itemsPerPage) {
                    scope.filter.isPagingCallback = true;
                    scope.filter.startIndex = pageIndex >= 0 ? (pageIndex * itemsPerPage) : 0;
                    scope.filter.itemsPerPage = itemsPerPage;

                    scope.gridOption.itemsPerPage = itemsPerPage;
                    scope.gridOption.currentPage = pageIndex;
                    scope.gridOption.startIndex = scope.filter.startIndex;

                    scope.getMealForecastReport.call(this, scope.filter);
                    return false;
                }

                var id = iElement.attr('id'),
                    report = scope.report,
                    bodyData = generateDataBody(report, scope),
                    headerData = generateDataHeader(scope.numberFormat),
                    footerData = generateDataFooter(report, scope);

                var gridOptions = {
                    columns: headerData,
                    bodyRows: bodyData,
                    showTitle: true,
                    titleInfo:
                            {
                                text: "Meal forecast report for " + moment(scope.filter.date).format(scope.dateFormat),
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

reportModule.directive('exportMealForecastDetailReport', ['baseUrl', function (baseUrl) {
    return {
        link: function (scope) {
            scope.$on('exportMealForecastDetailReport', function () {
                var data = $.param(scope.criteria),
                    url = baseUrl + 'ExportReport/ExportMealForecastDetailReport',
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