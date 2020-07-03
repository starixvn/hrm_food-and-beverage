'use strict';
/*global reportModule, moment, numeral*/

reportModule.directive('renderMealForecastReport', function () {
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
		var linkToDetail = function (currentCell) {
			var departmentId = scope.filter.departmentId;
			if (!departmentId) {
				scope.filter.departmentId = scope.originalDepartmentId;
			}

			var date = currentCell.Value,
			cell = '<a class="meal-forecast-detail" href="' + scope.baseUrl + 'MealForecastReport/Detail?date=' + date +
				'&departmentId=' + scope.filter.departmentId +
				'&employeeTypeId=' + scope.filter.employeeTypeId +
				'&dietType=' + scope.filter.vegetarianType + '">' + date + '</a>';
			return cell;
		};
		var rows = [], i = 0;
		var mealForecast = null, currentRow = [];

		if (report && report.MealForecasts) {
			var numberMealForecasts = report.MealForecasts.length,
                rowIndex = scope.gridOption.startIndex + 1;

			if (scope.filter.vegetarianType === '0') {
				for (i = 0; i < numberMealForecasts; i++) {
					mealForecast = report.MealForecasts[i];
					currentRow = {
						Columns: [
                        { Name: "Count", Value: rowIndex },
                        { Name: "Date", Value: moment(mealForecast.OnDate).format(scope.dateFormat), CustomCellRender: linkToDetail },
                        { Name: "Vegetarian", Value: "Non-Vegetarian" },
                        { Name: "NonRegistration", Value: mealForecast.NonRegistrationNonVegetarian },
                        { Name: "Registration", Value: mealForecast.RegistrationNonVegetarian },
                        { Name: "ActualOrder", Value: mealForecast.ActualOrderNonVegetarian < 0 ? '-' : mealForecast.ActualOrderNonVegetarian },
                        { Name: "Efficiency", Value: formatEfficiency(mealForecast.EfficiencyNonVegetarian),
							TdClass: getCssClass(mealForecast.EfficiencyNonVegetarian)
                        }
                    ]
					};

					rows.push(currentRow);
					rowIndex++;
				}
			}
			else if (scope.filter.vegetarianType === '1') {
				for (i = 0; i < numberMealForecasts; i++) {
					mealForecast = report.MealForecasts[i];
					currentRow = {
						Columns: [
                        { Name: "Count", Value: rowIndex },
                        { Name: "Date", Value: moment(mealForecast.OnDate).format(scope.dateFormat), CustomCellRender: linkToDetail },
                        { Name: "Vegetarian", Value: "Vegetarian" },
                        { Name: "NonRegistration", Value: mealForecast.NonRegistrationVegetarian },
                        { Name: "Registration", Value: mealForecast.RegistrationVegetarian },
                        { Name: "ActualOrder", Value: mealForecast.ActualOrderVegetarian < 0 ? '-' : mealForecast.ActualOrderVegetarian },
                        { Name: "Efficiency", Value: formatEfficiency(mealForecast.EfficiencyVegetarian),
							TdClass: getCssClass(mealForecast.EfficiencyVegetarian)
                        }
                    ]
					};

					rows.push(currentRow);
					rowIndex++;
				}
			}
			else {
				for (i = 0; i < numberMealForecasts; i++) {
					mealForecast = report.MealForecasts[i];
					var currentRowNonVegetarian = [],
                        currentRowVegetarian = [];

					currentRowNonVegetarian = {
						Columns: [
                            { Name: "Count", Value: rowIndex, MergeRowNumber: 2 },
                            { Name: "Date", Value: moment(mealForecast.OnDate).format(scope.dateFormat), CustomCellRender: linkToDetail,
								MergeRowNumber: 2
                            },
                            { Name: "Vegetarian", Value: "Non-Vegetarian" },
                            { Name: "NonRegistration", Value: mealForecast.NonRegistrationNonVegetarian },
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
                            { Name: "NonRegistration", Value: mealForecast.NonRegistrationVegetarian },
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

	var generateDataHeader = function (numberFormat) {
		var columns = [
            { HeaderText: "#", Width: 65, Name: "Count", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Date", Width: 210, Name: "Date", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Diet Type", Width: 200, Name: "Vegetarian", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Non-Registration", Width: 180, Name: "NonRegistration", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat },
            { HeaderText: "Registration (1)", Width: 180, Name: "Registration", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat },
            { HeaderText: "Actual orders (2)", Width: 180, Name: "ActualOrder", HeaderAlign: "center", CellAlign: "right", FormatNumber: numberFormat },
            { HeaderText: "Order rate (2)/(1)", Width: 180, Name: "Efficiency", HeaderAlign: "center", CellAlign: "right" }
        ];

		return columns;
	};

	var generateDataFooter = function (report, scope) {
        var footerRows = [],
            totalNonVegetarian = {
				Columns: [
                    { Value: "Total Non-Vegetarian", Name: "Count", MergeColNumber: 3, CellAlign: "right" },
                    { Value: numeral(report.TotalNonVegetarian.NonRegistration).format(scope.numberFormat), Name: "NonRegistration"  },
                    { Value: numeral(report.TotalNonVegetarian.Registration).format(scope.numberFormat), Name: "Registration" },
                    { Value: report.TotalNonVegetarian.ActualOrder < 0 ? '-' : numeral(report.TotalNonVegetarian.ActualOrder).format(scope.numberFormat),
						Name: "ActualOrder"
                    },
                    {
                        Value: formatEfficiency(report.TotalNonVegetarian.Efficiency), Name: "Efficiency",
						TdClass: getCssClass(report.TotalNonVegetarian.Efficiency)
                    }
                ]
            },
            totalVegetarian = {
				Columns: [
                    { Value: "Total Vegetarian", Name: "Count", MergeColNumber: 3, CellAlign: "right" },
                    { Value: numeral(report.TotalVegetarian.NonRegistration).format(scope.numberFormat), Name: "NonRegistration" },
                    { Value: numeral(report.TotalVegetarian.Registration).format(scope.numberFormat), Name: "Registration" }, 
                    { Value: report.TotalVegetarian.ActualOrder < 0 ? '-' : numeral(report.TotalVegetarian.ActualOrder).format(scope.numberFormat),
						Name: "ActualOrder"
                    },
                    {
                        Value: formatEfficiency(report.TotalVegetarian.Efficiency), Name: "Efficiency",
                        TdClass: getCssClass(report.TotalVegetarian.Efficiency)
                    }
                ]
            },
            totalMeal = {
				Columns: [
                    { Value: "Total Meal", Name: "Count", MergeColNumber: 3, CellAlign: "right" },
                    { Value: numeral(report.TotalMeal.NonRegistration).format(scope.numberFormat), Name: "NonRegistration" },
                    { Value: numeral(report.TotalMeal.Registration).format(scope.numberFormat), Name: "Registration" },
                    { Value: report.TotalMeal.ActualOrder < 0 ? '-' : numeral(report.TotalMeal.ActualOrder).format(scope.numberFormat), Name: "ActualOrder" },
                    {
                        Value: formatEfficiency(report.TotalMeal.Efficiency), Name: "Efficiency", 
                        TdClass: getCssClass(report.TotalMeal.Efficiency)
                    }
                ]
            };

		if (scope.filter.vegetarianType === '0') {
			footerRows.push(totalNonVegetarian);
		}
		else if (scope.filter.vegetarianType === '1') {
			footerRows.push(totalVegetarian);
		}
		else {
            footerRows.push(totalNonVegetarian, totalVegetarian, totalMeal);
		}

		return footerRows;
	};

	return {
		link: function (scope, iElement) {
			scope.$on("renderMealForecastReport", function () {
				function ReRenderAfterPaged(pageIndex, itemsPerPage) {
					scope.filter.isPagingCallback = true;
					scope.filter.startIndex = pageIndex >= 0 ? (pageIndex * itemsPerPage) : 0;
					scope.filter.itemsPerPage = itemsPerPage;

					scope.gridOption.itemsPerPage = itemsPerPage;
					scope.gridOption.currentPage = pageIndex;
					scope.gridOption.startIndex = scope.filter.startIndex;

					scope.setHistory();
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
								text: "Meal Forecast Report",
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

reportModule.directive('exportMealForecastReport', ['baseUrl', function (baseUrl) {
    return {
        link: function (scope) {
            scope.$on('exportMealForecastReport', function () {
                var data = $.param(scope.criteria),
                    url = baseUrl + 'ExportReport/ExportMealForecastReport',
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