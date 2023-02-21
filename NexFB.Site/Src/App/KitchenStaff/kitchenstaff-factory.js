'use strict';
/*global kitchenStaffModule*/

kitchenStaffModule.factory("kitchenStaffFactory",
    ['$http', 'baseUrl', function ($http, baseUrl) {
        return {
            getNewOrderByStaff: function (userId, mealId, onDate) {
                return $http.get(baseUrl + 'api/KitchenStaff/GetNewOrderByStaff',
                    {
                        params: {
                            userId: userId,
                            mealId: mealId,
                            onDate: onDate
                        }
                    }
                );
            },
            getStaffOrder: function (onDate) {
                return $http.get(baseUrl + 'api/KitchenStaff/GetStaffOrder',
                    {
                        params: {
                            onDate: onDate
                        }
                    });
            },
            getEmployees: function (keyWord, companyId, teamId, isShowDisable) {
                return $http.get(baseUrl + 'api/Employee/GetEmployees',
                    {
                        params: {
                            keyWord: keyWord,
                            companyId: companyId,
                            teamId: teamId,
                            isShowDisable: isShowDisable
                        }
                    }
                );
            },
            createOrder: function (order) {
                return $http.post(baseUrl + 'api/KitchenStaff/InsertStaffOrder', order);
            },
            editOrder: function (order) {
                return $http.post(baseUrl + 'api/KitchenStaff/UpdateStaffOrder', order);
            },
            deleteOrder: function (orderId) {
                return $http['delete'](baseUrl + 'api/KitchenStaff', { params: { orderId: orderId } });
            },
            getAllMealTimesheet: function () {
                return $http.get(baseUrl + 'api/Common/GetAllMealTimesheet');
            },
            getOrderStatusEnum: function () {
                return $http.get(baseUrl + 'api/report/GetOrderStatusEnum');
            }
        };
    }]
);

kitchenStaffModule.factory('staffOrderingUtilities', function () {
    var utilities = {};

    utilities.getStatusValue = function (orderStatusesOptions) {
        var statusValue = '';

        angular.forEach(orderStatusesOptions, function (status) {
            if (status.ticked) {
                statusValue = status.Value;
            }
        });

        return statusValue;
    };

    utilities.getOrderLocation = function (typeOrder, scope) {
        var locationId = '';

        if (typeOrder === 0) {
            angular.forEach(scope.newOrderLocations, function (value) {
                if (value.ticked) {
                    locationId = value.LocationId;
                }
            });
        }

        return locationId;
    };

    utilities.sortOrderIdByDesc = function (order, orders) {
        if (orders.length === 0) {
            orders.push(order);
        }
        else {
            var smallest = true;

            for (var i = 0; i < orders.length; i++) {
                if (order.OrderId > orders[i].OrderId) {
                    orders.splice(i, 0, order);
                    i = orders.length;
                    smallest = false;
                }
            }

            if (smallest) {
                orders.push(order);
            }
        }

        return orders;
    };

    return utilities;
});