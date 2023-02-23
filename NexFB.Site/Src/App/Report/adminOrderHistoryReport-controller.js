'use strict';
/*global reportModule*/

reportModule.controller('AdminOrderHistoryReportController', adminOrderHistoryReportController);
adminOrderHistoryReportController = ['$scope', 'reportFactory', 'ErrorFactory', 'reportUtilities', 'foodMenuFactory', 'commonFactory',
    'foodMenuUtilities', 'notificationFactory', 'groupControlFactory', '$modal', 'baseUrl'];
function adminOrderHistoryReportController($scope, reportFactory, ErrorFactory, reportUtilities, foodMenuFactory, commonFactory,
    foodMenuUtilities, notificationFactory, groupControlFactory, $modal, baseUrl) {
    $scope.searchEmployee = '';

    $scope.toDate = new Date();

    $scope.openMealList = false;

    $scope.orderStatus = {
        New: 1,
        Accepted: 2,
        Reduced: 3,
        Delivered: 4,
        Edited: 5,
        Rejected: 6
    };

    $scope.company = {
        Nexcel: '1',
        SMS: '2',
        HM: '3',
        Luminet: '4'
    };

    $scope.menuType = {
        All: 0,
        SideDish: 2,
        HasMainDish: 3,
        HasSideDish: 4
    };

    $scope.joinStatus = $scope.orderStatus.New + ',' + $scope.orderStatus.Accepted + ',' + $scope.orderStatus.Reduced + ',';
    $scope.filter = {
        fromDate: new Date($scope.toDate.getFullYear(), $scope.toDate.getMonth(), 1),
        toDate: new Date(),
        userId: null,
        employeeMenu: 0,
        orderPlatform: 0,
        mealId: '',
        statusId: $scope.joinStatus + $scope.orderStatus.Delivered + ',' + $scope.orderStatus.Edited + ',' + $scope.orderStatus.Rejected,
        companyId: $scope.company.Nexcel + ',' + $scope.company.SMS + ',' + $scope.company.HM + ',' + $scope.company.Luminet,
        isPagingCallback: false,
        startIndex: 0,
        itemsPerPage: 50
    };

    $scope.selectedOrderId = 0;

    var setDefaultCheck = function (items) {
        angular.forEach(items, function (item) {
            item.ticked = true;
        });
    };

    // Get all employees.
    $scope.getEmployee = function () {
        var companyId = $scope.company.Nexcel + ',' + $scope.company.SMS + ',' + $scope.company.HM + ',' + $scope.company.Luminet;

        return groupControlFactory.getEmployees($scope.searchEmployee, companyId, null, true);
    };

    $scope.unSelectEmployee = function () {
        if ($scope.searchEmployee.trim() === '') {
            if ($scope.filter && $scope.filter.employee) {
                $scope.filter.employee = null;
            }
        }
    };

    $scope.numberFormat = reportUtilities.getNumberFormat();
    $scope.dateFormat = reportUtilities.getDateFormatEmployeeOrderHistoryReport();
    $scope.timeFormat = reportUtilities.getTimeFormatAdminOrderHistoryReport();

    $scope.calendar = {
        startDateOpened: false,
        endDateOpened: false
    };

    // Open calendar.
    $scope.openPopupStartDate = function ($event) {
        commonFactory.openPopupStartDate($event, $scope);
    };

    $scope.openPopupEndDate = function ($event) {
        commonFactory.openPopupEndDate($event, $scope);
    };

    $scope.gridOption = {
        itemsPerPage: 50,
        currentPage: 0,
        startIndex: 0
    };

    // Reset grid option.
    var resetGridOption = function () {
        $scope.gridOption.itemsPerPage = 50;
        $scope.gridOption.currentPage = 0;
        $scope.gridOption.startIndex = 0;
    };

    // Reset paging option.
    var resetPagingOption = function (filter) {
        filter.startIndex = 0;
        filter.itemsPerPage = 50;
        filter.isPagingCallback = false;
    };

    // Get all meal timesheet.
    foodMenuFactory.getAllMealTimesheet().success(function (meals) {
        $scope.mealsOption = meals;
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    // Get Order Plaform.
    reportFactory.getOrderPlatformEnum().success(function (orderPlatformOptions) {
        $scope.orderPlatformOptions = orderPlatformOptions;
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    // Get Order Status.
    reportFactory.getOrderStatusEnum().success(function (orderStatuses) {
        $scope.orderStatusesOptions = orderStatuses;
        setDefaultCheck($scope.orderStatusesOptions);
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    // Get companies
    reportFactory.getCompanyEnum().success(function (companies) {
        $scope.companiesOptions = companies;
        setDefaultCheck($scope.companiesOptions);
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    // Get Menu Type.
    reportFactory.getMenuTypeEnum().success(function (menuTypes) {
        $scope.menuTypeOptions = menuTypes;
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    // Get report.
    $scope.getAdminOrderHistoryReport = function (filter) {
        commonFactory.blockUI();
        reportFactory.getAdminOrderHistoryReport(filter).success(function (report) {
            $scope.report = report;
            $scope.$broadcast("renderAdminOrderHistoryReport");

            commonFactory.unblockUI();
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);

            commonFactory.unblockUI();
        });
    };

    reportFactory.getCompanyOfCurrentUser().success(function () {
        $scope.filter.employeeMenu = $scope.menuType.All;

        $scope.getAdminOrderHistoryReport($scope.filter);
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    var getIdFromOptions = function (Options) {
        var statusId = '';

        angular.forEach(Options, function (value) {
            if (value.ticked) {
                var id = value.Value.toString();
                statusId = statusId !== '' ? statusId + ',' + id : id;
            }
        });

        return statusId;
    };

    // Filter report.
    $scope.query = function (filter) {
        resetGridOption();
        resetPagingOption(filter);

        filter.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
        filter.statusId = getIdFromOptions($scope.orderStatusesOptions);
        filter.companyId = getIdFromOptions($scope.companiesOptions);
        filter.userId = filter.employee ? filter.employee.UserId : '';

        var fromDate = new Date(filter.fromDate),
            toDate = new Date(filter.toDate);

        if (fromDate > toDate) {
            notificationFactory.error("DateRangeInvalid");
        } else {
            $scope.getAdminOrderHistoryReport(filter);
        }
    };

    // Show order detail.
    $scope.showOrderDetail = function (orderId) {
        if ($scope.locations.length > 0) {
            $scope.modalOrderDetailInstance = $modal.open({
                templateUrl: baseUrl + 'src/app/Report/Templates/adminOrderDetail.html',
                controller: "EditOrderController",
                backdrop: 'static',
                resolve: {
                    orderId: function () {
                        return orderId;
                    },
                    locations: function () {
                        return angular.copy($scope.locations);
                    },
                    startTime: function () {
                        return new Date();
                    }
                }
            });

            $scope.modalOrderDetailInstance.result.then(function () {
            }, function () {
            })['finally'](function () {
                $scope.modalOrderDetailInstance = undefined;
            });
        }
    };

    $scope.showEditOrderDetail = function (orderId) {
        if ($scope.locations.length > 0) {
            $scope.modalOrderDetailInstance = $modal.open({
                templateUrl: baseUrl + 'src/app/Report/Templates/adminOrderDetailEdit.html',
                controller: "EditOrderController",
                backdrop: 'static',
                resolve: {
                    orderId: function () {
                        return orderId;
                    },
                    locations: function () {
                        return angular.copy($scope.locations);
                    },
                    startTime: function () {
                        return new Date();
                    }
                }
            });

            $scope.modalOrderDetailInstance.result.then(function () {
                $scope.query($scope.filter);
            }, function () {
            })['finally'](function () {
                $scope.modalOrderDetailInstance = undefined;
            });
        }
    };

    // Get all locations.
    reportFactory.getLocation()
        .success(function (data) {
            $scope.locations = data;
        })
        .error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });

    // Export excel.
    $scope.exportExcel = function (filter) {
        filter.mealId = foodMenuUtilities.getMealId($scope.mealsOption);
        filter.statusId = getIdFromOptions($scope.orderStatusesOptions);
        filter.companyId = getIdFromOptions($scope.companiesOptions);
        filter.userId = filter.employee ? filter.employee.UserId : '';

        $scope.criteria = {
            fromDate: filter.fromDate.toDateString(),
            toDate: filter.toDate.toDateString(),
            userId: filter.userId,
            myMenu: filter.employeeMenu,
            orderPlatform: filter.orderPlatform,
            mealId: filter.mealId,
            statusId: filter.statusId,
            companyId: filter.companyId,
        };

        $scope.$broadcast('exportAdminOrderHistoryReport');
    };

    $scope.showMealList = function (isShow) {
        $scope.openMealList = isShow;
    }

    $scope.changeMeal = function (event) {
        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/components/extensions/common/templates/ModalConfirmContent.html',
            controller: "ModalConfirmController",
            backdrop: 'static',
            resolve: {
                action: function () {
                    return 'update meal for';
                },
                name: function () {
                    return 'Order (Ref : ' + $scope.selectedOrderId + ')';
                }
            },
            windowClass: "modal-confirm-center"
        });

        $scope.modalInstance.result.then(function () {
            var params = {
                OrderId: $scope.selectedOrderId,
                MealId: event.currentTarget.id
            }

            reportFactory.updateMealOrder(params).success(function (response) {
                if (response.IsSuccessful) {
                    notificationFactory.success("DataChangeSuccessfully");
                    $scope.query($scope.filter);
                } else {
                    notificationFactory.error("", response.Message);
                }
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            $scope.showMealList(false);
        })['finally'](function () {
            $scope.modalInstance = '';
        });
    }

    $scope.globalClickFunc = function () {
        var currentElement = $(document.activeElement)[0];
        if ($scope.openMealList == true && !currentElement.matches('.meal-select') && !currentElement.matches('.btn-expand-meal-list')) {
            $scope.showMealList(false);
        }
    };
}