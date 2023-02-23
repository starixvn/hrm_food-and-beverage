'use strict';

reportModule.controller('MonthlyLunchAndDinnerReportController', monthlyLunchAndDinnerReportController);
monthlyLunchAndDinnerReportController.$inject = ['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'commonFactory', 'baseUrl'];

function monthlyLunchAndDinnerReportController($scope, reportFactory, ErrorFactory, reportUtilities, commonFactory, baseUrl) {
    $scope.filter = {
        reportDate: new Date(),
        companyId: ''
    };

    $scope.originalCompanyId = '';

    $scope.format = 'MMM yyyy';
    $scope.popUpMode = 'month';
    $scope.dateOptions = {
        minMode: 'month',
        maxMode: 'month',
        formatMonth: 'MMM'
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
            }
            else {
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
        $scope.getMonthlyLunchAndDinnerReport($scope.filter);
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    // Get Day Type.
    reportFactory.getDayTypeEnum().success(function (dayTypes) {
        $scope.dayTypeOptions = dayTypes;
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    // Get monthly lunch and dinner report.
    $scope.getMonthlyLunchAndDinnerReport = function (filter) {
        commonFactory.blockUI();

        reportFactory.getMonthlyLunchAndDinnerReport(filter).success(function (report) {
            $scope.report = report;

            commonFactory.unblockUI();
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);

            commonFactory.unblockUI();
        });
    };

    // Query data.
    $scope.query = function (filter) {
        if (!filter.companyId) {
            filter.companyId = $scope.originalCompanyId;
        }

        $scope.getMonthlyLunchAndDinnerReport(filter);
    };

    // Export Excel
    $scope.exportExcel = function (filter) {
        var criteria = {
            companyId: filter.companyId,
            reportDate: filter.reportDate.toDateString()
        };
        var data = $.param(criteria),
            url = baseUrl + 'ExportReport/ExportMonthlyLunchAndDinnerAmountReport',
            downloadUrl = url + "?" + data,
            iframe = $('body iframe');

        if (iframe.length === 0) {
            iframe = $('<iframe width="0" height="0" class="display-none"></iframe>');
            iframe.appendTo('body');
        }

        iframe.attr('src', downloadUrl);
    };
}