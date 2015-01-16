'use strict';
/*global reportModule, moment*/

reportModule.directive('renderMealForecastReportDetail', function () {
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

        if (report && report.MealForecastDetails) {
            var numberMealForecastDetails = report.MealForecastDetails.length,
                rowIndex = scope.gridOption.startIndex + 1;

            if (scope.filter.dietType === '0') {
                for (i = 0; i < numberMealForecastDetails; i++) {
                    mealForecastDetail = report.MealForecastDetails[i];
                    currentRow = {
                        Columns: [
                        { Name: "Count", Value: rowIndex },
                        { Name: "MenuVie", Value: mealForecastDetail.MenuVie },
                        { Name: "MenuEng", Value: mealForecastDetail.MenuEng },
                        { Name: "Meal", Value: mealForecastDetail.Meal },
                        { Name: "Vegetarian", Value: "Non-Vegetarian" },
                        { Name: "Registration", Value: mealForecastDetail.RegistrationNonVegetarian },
                        { Name: "ActualOrder", Value: mealForecastDetail.ActualOrderNonVegetarian < 0 ? '-' : mealForecastDetail.ActualOrderNonVegetarian },
                        { Name: "Efficiency", Value: formatEfficiency(mealForecastDetail.EfficiencyNonVegetarian),
                            TdClass: getCssClass(mealForecastDetail.EfficiencyNonVegetarian)
                        }
                    ]
                    };

                    rows.push(currentRow);
                    rowIndex++;
                }
            }
            else if (scope.filter.dietType === '1') {
                for (i = 0; i < numberMealForecastDetails; i++) {
                    mealForecastDetail = report.MealForecastDetails[i];
                    currentRow = {
                        Columns: [
                        { Name: "Count", Value: rowIndex },
                        { Name: "MenuVie", Value: mealForecastDetail.MenuVie },
                        { Name: "MenuEng", Value: mealForecastDetail.MenuEng },
                        { Name: "Meal", Value: mealForecastDetail.Meal },
                        { Name: "Vegetarian", Value: "Vegetarian" },
                        { Name: "Registration", Value: mealForecastDetail.RegistrationVegetarian },
                        { Name: "ActualOrder", Value: mealForecastDetail.ActualOrderVegetarian < 0 ? '-' : mealForecastDetail.ActualOrderVegetarian },
                        { Name: "Efficiency", Value: formatEfficiency(mealForecastDetail.EfficiencyVegetarian),
                            TdClass: getCssClass(mealForecastDetail.EfficiencyVegetarian)
                        }
                    ]
                    };

                    rows.push(currentRow);
                    rowIndex++;
                }
            }
            else {
                for (i = 0; i < numberMealForecastDetails; i++) {
                    mealForecastDetail = report.MealForecastDetails[i];
                    var currentRowNonVegetarian = [],
                        currentRowVegetarian = [];

                    currentRowNonVegetarian = {
                        Columns: [
                            { Name: "Count", Value: rowIndex, MergeRowNumber: 2 },
                            { Name: "Date", Value: '<a>' + moment(mealForecast.OnDate).format(scope.dateFormat) + '</a>', MergeRowNumber: 2 },
                            { Name: "Vegetarian", Value: "Non-Vegetarian" },
                            { Name: "Registration", Value: mealForecast.RegistrationNonVegetarian },
                            { Name: "ActualOrder", Value: mealForecast.ActualOrderNonVegetarian < 0 ? '-' : mealForecast.ActualOrderNonVegetarian },
                            { Name: "Efficiency", Value: formatEfficiency(mealForecast.EfficiencyNonVegetarian),
                                TdClass: getCssClass(mealForecast.EfficiencyNonVegetarian)
                            }
                        ]
                    };

                    currentRowVegetarian = {
                        Columns: [
                            { Name: "Vegetarian", Value: "Vegetarian" },
                            { Name: "Registration", Value: mealForecast.RegistrationVegetarian },
                            { Name: "ActualOrder", Value: mealForecast.ActualOrderVegetarian < 0 ? '-' : mealForecast.ActualOrderVegetarian },
                            { Name: "Efficiency", Value: formatEfficiency(mealForecast.EfficiencyVegetarian),
                                TdClass: getCssClass(mealForecast.EfficiencyVegetarian)
                            }
                        ]
                    };

                    rows.push(currentRowNonVegetarian);
                    rows.push(currentRowVegetarian);
                    rowIndex++;
                }
            }
        }

        return rows;
    };

    var generateDataHeader = function () {
        var columns = [
            { HeaderText: "#", Width: 60, Name: "Count", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Menu (Vie)", Width: 100, Name: "MenuVie", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Menu (Eng)", Width: 100, Name: "MenuEng", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Meal", Width: 100, Name: "Meal", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Diet Type", Width: 100, Name: "Vegetarian", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Registration (1)", Width: 150, Name: "Registration", HeaderAlign: "center", CellAlign: "right" },
            { Headertext: "Actual orders (2)", Width: 150, Name: "ActualOrders", HeaderAlign: "center", CellAlign: "right" },
            { HeaderText: "Order rate (2)/(1)", Width: 150, name: "Efficiency", HeaderAlign: "center", CellAlign: "right" }
        ];

        return columns;
    };

    var generateDataFooter = function (report) {
        var footerRow = [
            {
                Columns: [
                    { Value: "Total Non-Vegetarian", Name: "Count", MergeColNumber: 5, CellAlign: "right" },
                    { Value: 0, Name: "Registration" },
                    { Value: 0, Name: "ActualOrders" },
                    { Value: 0, Name: "Efficiency" }
                ]
            },
            {
                Columns: [
                    { Value: "Total Vegetarian", Name: "Count", MergeColNumber: 5, CellAlign: "right" },
                    { Value: 0, Name: "Registration" },
                    { Value: 0, Name: "ActualOrders" },
                    { Value: 0, Name: "Efficiency" }
                ]
            },
            {
                Columns: [
                    { Value: "Total Meal", Name: "Count", MergeColNumber: 5, CellAlign: "right" },
                    { Value: 0, Name: "Registration" },
                    { Value: 0, Name: "ActualOrders" },
                    { Value: 0, Name: "Efficiency" }
                ]
            }
        ];

        return footerRow;
    };

    return {
        link: function (scope, iElement) {
            scope.$on("renderMealForecastReportDetail", function () {
                var id = iElement.attr('id'),
                    report = scope.report,
                    bodyData = generateDataBody(report, scope),
                    headerData = generateDataHeader(),
                    footerData = generateDataFooter(report);

                var gridOptions = {
                    columns: headerData,
                    bodyRows: bodyData,
                    showTitle: true,
                    titleInfo:
                            {
                                text: "Meal forecast report for " + scope.filter.date,
                                showExcelButton: true,
                                exportExcelFunction: function () {
                                }
                            },
                    showFooter: true,
                    footerRows: footerData,
                    gridExpandHeight: 250
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
                    url = baseUrl + 'ExportReport/exportMealForecastDetailReport',
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