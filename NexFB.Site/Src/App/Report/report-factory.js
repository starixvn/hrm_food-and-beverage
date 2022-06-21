'use strict';
/*global reportModule*/

reportModule.factory('reportFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
    var date = new Date(),
        history = {
            fromDate: new Date(date.getFullYear(), date.getMonth(), 1),
            toDate: new Date(),
            departmentId: '',
            employeeTypeId: '',
            vegetarianType: '',
            isPagingCallback: false,
            startIndex: 0,
            itemsPerPage: 50,
            currentPage: 0,
            isCallBack: false
        };
    var historySideDishSummary = {
        fromDate: new Date(date.getFullYear(), date.getMonth(), 1),
        toDate: new Date(),
        departmentId: '',
        teamId: '',
        employeeTypeId: '',
        employee: {},
        pageIndex: 0,
        itemsPerPage: 50
    };

    return {
        getMonthlyMainDishReportByLocal: function (filter) {
            return $http.get(baseUrl + 'api/report/GetMonthlyMainDishReportLocal',
                {
                    params: {
                        reportDate: filter.reportDate.toDateString(),
                        departmentId: filter.departmentId,
                        employeeTypeId: filter.employeeTypeId,
                        teamId: filter.teamId,
                        mealId: filter.mealId,
                        groupBy: filter.groupBy,
                        startIndex: filter.startIndex,
                        itemsPerPage: filter.itemsPerPage,
                        isExportReport: filter.isExportReport,
                        isPagingCallback: filter.isPagingCallback
                    }
                });
        },
        getDailyMainDishReport: function (filter) {
            return $http.get(baseUrl + 'api/report/GetDailyMainDishReport', {
                params: {
                    fromDate: filter.fromDate.toDateString(),
                    toDate: filter.toDate.toDateString(),
                    orderCount: filter.orderCount,
                    employeeTypeId: filter.employeeTypeId,
                    departmentId: filter.departmentId,
                    teamId: filter.teamId,
                    mealId: filter.mealId,
                    userId: filter.userId,
                    query: filter.query,
                    startIndex: filter.startIndex,
                    itemsPerPage: filter.itemsPerPage,
                    isPagingCallback: filter.isPagingCallback
                }
            });
        },
        getDepartmentByUserRole: function () {
            return $http.get(baseUrl + 'api/report/GetDepartmentByUserRole');
        },
        getEmployeeOrderHistoryReport: function (filter) {
            return $http.get(baseUrl + 'api/report/GetEmployeeOrderHistoryReport',
                {
                    params: {
                        fromDate: filter.fromDate.toDateString(),
                        toDate: filter.toDate.toDateString(),
                        orderBy: filter.orderBy,
                        employeeMenu: filter.employeeMenu,
                        orderPlatform: filter.orderPlatform,
                        mealId: filter.mealId,
                        statusId: filter.statusId,
                        isPagingCallback: filter.isPagingCallback,
                        startIndex: filter.startIndex,
                        itemsPerPage: filter.itemsPerPage
                    }
                });
        },
        getAdminOrderHistoryReport: function (filter) {
            return $http.get(baseUrl + 'api/report/GetAdminOrderHistoryReport',
                {
                    params: {
                        fromDate: filter.fromDate.toDateString(),
                        toDate: filter.toDate.toDateString(),
                        userId: filter.userId > 0 ? filter.userId : filter.employee ? filter.employee.UserId : '',
                        employeeMenu: filter.employeeMenu,
                        orderPlatform: filter.orderPlatform,
                        mealId: filter.mealId,
                        statusId: filter.statusId,
                        companyId: filter.companyId,
                        isPagingCallback: filter.isPagingCallback,
                        startIndex: filter.startIndex,
                        itemsPerPage: filter.itemsPerPage
                    }
                });
        },
        getSideDishReport: function (filter) {
            return $http.get(baseUrl + 'api/report/GetSideDishReport', {
                params: {
                    fromDate: filter.fromDate.toDateString(),
                    toDate: filter.toDate.toDateString(),
                    departmentId: filter.departmentIds,
                    teamIds: filter.teamIds,
                    employeeTypeIds: filter.employeeTypeIds,
                    userId: filter.userId > 0 ? filter.userId : filter.employee ? filter.employee.UserId : '',
                    sideDishItemIds: filter.sideDishItemIds,
                    pageIndex: filter.pageIndex,
                    itemsPerPage: filter.itemsPerPage
                }
            });
        },
        getMealForecastReport: function (filter) {
            return $http.get(baseUrl + 'api/report/GetMealForecastReport',
                {
                    params: {
                        fromDate: filter.fromDate.toDateString(),
                        toDate: filter.toDate.toDateString(),
                        departmentId: filter.departmentId,
                        employeeTypeId: filter.employeeTypeId,
                        isVegetarian: filter.vegetarianType.length === 0 ? '' : filter.vegetarianType === '0' ? false : true,
                        isPagingCallback: filter.isPagingCallback,
                        startIndex: filter.startIndex,
                        itemsPerPage: filter.itemsPerPage
                    }
                });
        },
        getMealForecastDetailReport: function (filter) {
            return $http.get(baseUrl + 'api/report/GetMealForecastDetailReport',
                {
                    params: {
                        date: filter.date,
                        departmentId: filter.departmentId,
                        employeeTypeId: filter.employeeTypeId,
                        isVegetarian: filter.dietType.length === 0 ? '' : filter.dietType === '0' ? false : true,
                        isPagingCallback: filter.isPagingCallback,
                        startIndex: filter.startIndex,
                        itemsPerPage: filter.itemsPerPage
                    }
                });
        },
        getEmployeeRegistrationDetailReport: function (filter) {
            return $http.get(baseUrl + 'api/report/GetEmployeeRegistration',
                {
                    params: {
                        date: filter.date,
                        departmentId: filter.departmentId,
                        employeeTypeId: filter.employeeTypeId,
                        mealId: filter.mealId,
                        menuId: filter.menuId,
                        isVegetarian: filter.dietType.length === 0 ? '' : filter.dietType === '0' ? false : true,
                        isPagingCallback: filter.isPagingCallback,
                        startIndex: filter.startIndex,
                        itemsPerPage: filter.itemsPerPage
                    }
                });
        },
        getEmployeeNonRegistrationDetailReport: function (filter) {
            return $http.get(baseUrl + 'api/report/GetEmployeeNonRegistration',
                {
                    params: {
                        date: filter.date,
                        departmentId: filter.departmentId,
                        employeeTypeId: filter.employeeTypeId,
                        mealId: filter.mealId,
                        menuId: filter.menuId,
                        isVegetarian: filter.dietType.length === 0 ? '' : filter.dietType === '0' ? false : true,
                        isPagingCallback: filter.isPagingCallback,
                        startIndex: filter.startIndex,
                        itemsPerPage: filter.itemsPerPage
                    }
                });
        },
        getSideDishSummaryReport: function (filter) {
            return $http.get(baseUrl + 'api/report/GetSideDishSummaryReport', {
                params: {
                    fromDate: filter.fromDate.toDateString(),
                    toDate: filter.toDate.toDateString(),
                    departmentId: filter.departmentId,
                    teamId: filter.teamId,
                    employeeTypeId: filter.employeeTypeId,
                    userId: filter.employee ? filter.employee.UserId : '',
                    pageIndex: filter.pageIndex,
                    itemsPerPage: filter.itemsPerPage
                }
            });
        },
        getOrderedByEnum: function () {
            return $http.get(baseUrl + 'api/report/GetOrderedByEnum');
        },
        getOrderPlatformEnum: function () {
            return $http.get(baseUrl + 'api/report/GetOrderPlatformEnum');
        },
        getOrderStatusEnum: function () {
            return $http.get(baseUrl + 'api/report/GetOrderStatusEnum');
        },
        getCompanyEnum: function () {
            return $http.get(baseUrl + 'api/report/GetCompanyEnum');
        },
        setHistory: function (scope, isCallBack) {
            history.fromDate = scope.filter.fromDate;
            history.toDate = scope.filter.toDate;
            history.departmentId = scope.filter.departmentId;
            history.employeeTypeId = scope.filter.employeeTypeId;
            history.vegetarianType = scope.filter.vegetarianType;
            history.startIndex = scope.filter.startIndex;
            history.itemsPerPage = scope.filter.itemsPerPage;
            history.currentPage = scope.gridOption.currentPage;
            history.isCallBack = isCallBack;
        },
        getHistory: function () {
            return history;
        },
        getMenuTypeEnum: function () {
            return $http.get(baseUrl + 'api/report/GetMenuTypeEnum');
        },
        getMonthlyMainDishReportPerDay: function (filter) {
            return $http.get(baseUrl + 'api/report/GetMonthlyMainDishPerDayReport',
                {
                    params: {
                        reportDate: filter.reportDate.toDateString(),
                        departmentId: filter.departmentId,
                        employeeTypeId: filter.employeeTypeId,
                        teamId: filter.teamId,
                        workDay: filter.workDay,
                        groupBy: filter.groupBy,
                        startIndex: filter.startIndex,
                        itemsPerPage: filter.itemsPerPage,
                        isPagingCallback: filter.isPagingCallback
                    }
                });
        },
        getDayTypeEnum: function () {
            return $http.get(baseUrl + 'api/report/GetDayType');
        },
        getDepartmentOfCurrentUser: function () {
            return $http.get(baseUrl + 'api/common/GetDepartmentOfCurrentUser');
        },
        getLocation: function () {
            return $http.get(baseUrl + 'api/Common/GetAllDeliveryArea');
        },
        getAllSideDish: function (foodId, isActive, employeeTypeId) {
            return $http.get(baseUrl + 'api/report/GetAllSideDish', {
                params: {
                    foodId: foodId,
                    isActive: isActive,
                    employeeTypeId: employeeTypeId
                }
            });
        },
        getViewMenuReport: function (filter) {
            var data = {
                fromDate: filter.fromDate,
                toDate: filter.toDate,
                mealId: filter.mealId,
                isVegetarian: filter.isVegetarian
            };

            return $http.get(baseUrl + 'api/report/GetViewMenuReport', { params: data });
        },
        getSpecialUserById: function (userId) {
            return $http.get(baseUrl + 'api/report/GetEmployeeById',
                { params: { userId: userId } });
        },
        setHistorySideDishSummary: function (scope) {
            historySideDishSummary = {
                fromDate: scope.filter.fromDate,
                toDate: scope.filter.toDate,
                departmentId: scope.filter.departmentId,
                teamId: scope.filter.teamId,
                employeeTypeId: scope.filter.employeeTypeId,
                employee: scope.filter.employee,
                pageIndex: scope.filter.pageIndex,
                itemsPerPage: scope.filter.itemsPerPage
            };
        },
        getHistorySideDishSummary: function () {
            return historySideDishSummary;
        },
        getMonthlyLunchAndDinnerReport: function (filter) {
            var data = {
                departmentId: filter.departmentId,
                reportDate: filter.reportDate.toDateString()
            }
            return $http.get(baseUrl + 'api/report/GetLunchAndDinnerReport', { params: data })
        },
        getMonthlyBreakfastAndSupperReport: function (filter) {
            var data = {
                departmentId: filter.departmentId,
                reportDate: filter.reportDate.toDateString()
            }
            return $http.get(baseUrl + 'api/report/GetBreakfastAndSupperReport', { params: data })
        },
        updateMealOrder: function (params) {
            return $http.put(baseUrl + 'api/Ordering/ChangeMealOrder', params);
        }
    };
}]);

reportModule.factory('reportUtilities', function () {
    var utilities = {},
        numberFormat = '0,0',
        monthFormat = 'MMM-YYYY',
        dateFormatDailyReport = 'DD-MMM-YYYY',
        dateFormaEmployeeOrderHistoryReport = "DD MMM YYYY",
        timeFormat = "HH:mm";

    utilities.getTeamId = function (teamOptions) {
        var teamId = '';

        angular.forEach(teamOptions, function (value) {
            if (value.ticked) {
                var id = value.TeamId.toString();
                teamId = teamId !== '' ? teamId + ',' + id : id;
            }
        });

        return teamId;
    };

    utilities.getSideDishIds = function (sideDishItems) {
        var sideDishIds = '',
            selectedIds = [];

        angular.forEach(sideDishItems, function (value) {
            if (value.ticked) {
                selectedIds.push(value.FoodId.toString());
            }
        });

        sideDishIds = selectedIds.join(",");

        return sideDishIds;
    };

    utilities.getNumberFormat = function () {
        return numberFormat;
    };

    utilities.getMonthFormat = function () {
        return monthFormat;
    };

    utilities.getDateFormatDailyReport = function () {
        return dateFormatDailyReport;
    };

    utilities.getDateFormatEmployeeOrderHistoryReport = function () {
        return dateFormaEmployeeOrderHistoryReport;
    };

    utilities.getTimeFormatAdminOrderHistoryReport = function () {
        return timeFormat;
    };

    return utilities;
});