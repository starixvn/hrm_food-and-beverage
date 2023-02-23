'use strict';

reportModule.controller('MonthlyBreakfastAndSuperController', monthlyBreakfastAndSuperController);
monthlyBreakfastAndSuperController.$inject = ['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'commonFactory', 'baseUrl'];
function monthlyBreakfastAndSuperController($scope, reportFactory, ErrorFactory, reportUtilities, commonFactory, baseUrl) {
    $scope.filter = {
        reportDate: new Date(),
        companyId: '',
        startIndex: 0,
        isExportReport: false,
        isPagingCallback: false
    };

    $scope.originalCompanyId = '';

    $scope.format = 'MMM yyyy';
    $scope.popUpMode = 'month';
    $scope.dateOptions = {
        minMode: 'month',
        maxMode: 'month',
        formatMonth: 'MMM'
    };

    $scope.gridOption = {
        itemsPerPage: 50,
        currentPage: 0,
        startIndex: 0
    };

    $scope.numberFormat = reportUtilities.getNumberFormat();
    $scope.monthFormat = reportUtilities.getMonthFormat();

    // Open pop-up calendar.
    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.opened = true;
    };

    var getCompanyFilter = function (companies) {
        var companyId = '';
        if (companies) {
            if (companies.length === 1) {
                companyId = companies[0].CompanyId;
            } else {
                angular.forEach(companies, function (company) {
                    companyId = companyId ? companyId + ',' + company.CompanyId : company.CompanyId;
                });
            }
        }

        $scope.filter.companyId = companyId;
        $scope.originalCompanyId = companyId;
    };

    // Get all company.
    reportFactory.getCompanyByUserRole().success(function (companies) {
        $scope.companyOptions = companies;
        getCompanyFilter(companies);
        $scope.getMonthlyBreakfastAndSupperReport($scope.filter);
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    // Get Day Type.
    reportFactory.getDayTypeEnum().success(function (dayTypes) {
        $scope.dayTypeOptions = dayTypes;
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    // Get monthly breakfast and supper report.
    $scope.getMonthlyBreakfastAndSupperReport = function (filter) {
        reportFactory.getMonthlyBreakfastAndSupperReport(filter).success(function (report) {
            $scope.report = report;
            commonFactory.tablePagination($scope, report, $scope.entrySelected);
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    // Query data.
    $scope.query = function (filter) {
        if (!filter.companyId) {
            filter.companyId = $scope.originalCompanyId;
        }

        $scope.getMonthlyBreakfastAndSupperReport(filter);
    };

    // Export Excel
    $scope.exportExcel = function (filter) {
        var criteria = {
            companyId: filter.companyId,
            reportDate: filter.reportDate.toDateString()
        }
        var data = $.param(criteria),
            url = baseUrl + 'ExportReport/ExportMonthlyBreakfastAndSupperAmountReport',
            downloadUrl = url + "?" + data,
            iframe = $('body iframe');

        if (iframe.length === 0) {
            iframe = $('<iframe width="0" height="0" class="display-none"></iframe>');
            iframe.appendTo('body');
        }

        iframe.attr('src', downloadUrl);
    };
}