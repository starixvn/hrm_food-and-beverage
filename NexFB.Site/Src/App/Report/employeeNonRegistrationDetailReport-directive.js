'use strict';
/*global reportModule, moment, numeral*/

reportModule.directive('renderEmployeeNonRegistrationDetailReport', function () {
    var generateDataBody = function (report, scope) {
        var rows = [], i = 0;
        var employeeNonRegistration = null, currentRow = [];

        if (report !== null && report.EmployeeNonRegistrations.length > 0) {
            var numberNonRegistrations = report.EmployeeNonRegistrations.length,
                rowIndex = scope.gridOption.startIndex + 1;

            for (i = 0; i < numberNonRegistrations; i++) {
                employeeNonRegistration = report.EmployeeNonRegistrations[i];
                currentRow = {
                    Columns: [
                        { Name: "Count", Value: rowIndex },
                        { Name: "Code", Value: employeeNonRegistration.Employee.Code },
                        { Name: "Department", Value: employeeNonRegistration.DepartmentName },
                        { Name: "Shift", Value: employeeNonRegistration.ShiftName },
                        { Name: "NickName", Value: employeeNonRegistration.Employee.NickName },
                        { Name: "FullName", Value: employeeNonRegistration.Employee.FullName },
                        { Name: "Meal", Value: employeeNonRegistration.Meal.MealName },
                        { Name: "Vegetarian", Value: employeeNonRegistration.IsVegetarian === false ? 
                            'Non-Vegetarian' : (employeeNonRegistration.IsVegetarian === true ? 'Vegetarian' : '-') }
                    ]
                };
                rows.push(currentRow);
                rowIndex++;
            }
        }

        return rows;
    };

    var generateDataHeader = function () {
        var columns = [
            { HeaderText: "#", Width: 65, Name: "Count", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Code", Width: 160, Name: "Code", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Department", Width: 160, Name: "Department", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Shift", Width: 160, Name: "Shift", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Nick Name", Width: 200, Name: "NickName", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Full Name", Width: 200, Name: "FullName", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Meal", Width: 200, Name: "Meal", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Diet Type", Width: 200, Name: "Vegetarian", HeaderAlign: "center", CellAlign: "center" }
        ];

        return columns;
    };

    var generateDataFooter = function (report, scope) {
        if (report !== null && report.EmployeeNonRegistrations.length > 0) {
            var footerRows = [],
            totalMeal = {
                Columns: [
                    { Value: "Total Meal", Name: "Count", MergeColNumber: 7, CellAlign: "right" },
                    { Value: numeral(report.TotalMeal.NonRegistration).format(scope.numberFormat), Name: "Vegetarian" }
                ]
            };

            footerRows.push(totalMeal);

            return footerRows;
        }
    };

    return {
        link: function (scope, iElement) {
            scope.$on("renderEmployeeNonRegistrationDetailReport", function () {
                function ReRenderAfterPaged(pageIndex, itemsPerPage) {
                    scope.filter.isPagingCallback = true;
                    scope.filter.startIndex = pageIndex >= 0 ? (pageIndex * itemsPerPage) : 0;
                    scope.filter.itemsPerPage = itemsPerPage;

                    scope.gridOption.itemsPerPage = itemsPerPage;
                    scope.gridOption.currentPage = pageIndex;
                    scope.gridOption.startIndex = scope.filter.startIndex;

                    scope.getEmployeeNonRegistrationDetailReport.call(this, scope.filter);
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
                                text: "Meal NonRegistration detail report for " + moment(scope.filter.date).format(scope.dateFormat),
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

reportModule.directive('exportEmployeeNonRegistrationDetailReport', ['baseUrl', function (baseUrl) {
    return {
        link: function (scope) {
            scope.$on('exportEmployeeNonRegistrationDetailReport', function () {
                var data = $.param(scope.criteria),
                    url = baseUrl + 'ExportReport/ExportEmployeeNonRegistrationDetailReport',
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