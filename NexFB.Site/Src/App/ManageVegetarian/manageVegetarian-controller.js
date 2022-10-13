'use strict';
/*global manageVegetarianModule*/

manageVegetarianModule.controller('ManageVegetarianController',
    ['$scope', 'mealRegistrationFactory', 'mealRegistrationUtilities', 'notificationFactory', '$route', '$window', 'ErrorFactory', 'baseUrl',
        function ($scope, mealRegistrationFactory, mealRegistrationUtilities, notificationFactory, $route, $window, ErrorFactory, baseUrl) {
            mealRegistrationUtilities.getTabHeading($scope);

            mealRegistrationFactory.getUserPermissionOnRegistration().success(function (permission) {
                $scope.policy = {
                    permission: permission
                };
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            mealRegistrationFactory.getCurrentUser()
                .success(function (data) {
                    $scope.currentUser = data;
                })
                .error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                });

            $scope.goHomePage = function () {
                $window.location.href = baseUrl + "Ordering/Index";
            };
        }]);

manageVegetarianModule.controller('MonthlyManageVegetarianController',
    ['$scope', 'manageVegetarianFactory', 'mealRegistrationFactory', '$modal', 'notificationFactory', 'ErrorFactory', 'mealRegistrationUtilities', 'baseUrl',
        function ($scope, manageVegetarianFactory, mealRegistrationFactory, $modal, notificationFactory, ErrorFactory, mealRegistrationUtilities, baseUrl) {
            /*
            ** Declare variable.
            */
            var currentDate = new Date();

            $scope.currentDate = currentDate;
            $scope.format = 'MMM yyyy';
            $scope.popUpMode = 'month';
            $scope.dateOptions = {
                minMode: 'month',
                maxMode: 'month',
                formatMonth: 'MMM'
            };

            /*
            ** Get setting of register meal.
            */
            mealRegistrationFactory.getRegisterMealSetting().success(function (setting) {
                $scope.registerSetting = setting;
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            /*
            ** Open pop-up calendar.
            */
            $scope.open = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();

                $scope.opened = true;
            };
            /*
            ** Get register meal by range date.
            */
            var getRegisterMealByRangeDate = function (fromDate, toDate, mealId) {
                mealRegistrationFactory.getRegisterMealByRangeDate(fromDate.toDateString(), toDate.toDateString(), mealId).success(function (mealDates) {
                    $scope.allWeekMeals = mealDates;
                }).error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                });
            };

            /*
            ** Get vegetarian management by range date.
            */
            var getVegetarianManagementByRangeDate = function (fromDate, toDate, mealId) {
                manageVegetarianFactory.getVegetarianManagementByRangeDate(fromDate.toDateString(), toDate.toDateString(), mealId).success(function (result) {
                    $scope.listVegetarianManagement = result;
                }).error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                });
            };

            $scope.isShowShortNameAndIcon = function (id, date) {
                var isShow = $scope.listVegetarianManagement.find(item => (item.WorkDate === date && item.MealId === id));
                return isShow && isShow.IsAllowVegetarian
            }

            $scope.$watch('tabMonthly', function (newValue) {
                if (newValue) {
                    var changedMonth = $scope.currentDate,
                        fromDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth(), 1),
                        toDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth() + 1, 0);

                    getRegisterMealByRangeDate(fromDate, toDate, null);
                    getVegetarianManagementByRangeDate(fromDate, toDate, null)
                }
            });

            /*
            ** Check current date is today or not.
            */
            $scope.checkCurrentDate = function (date) {
                var toDay = new Date(),
                    workDate = new Date(date.WorkDate);

                if (toDay.getYear() === workDate.getYear() && toDay.getMonth() === workDate.getMonth() &&
                    toDay.getDate() === workDate.getDate()) {
                    return true;
                }

                return false;
            };

            /*
            ** Check current date is weekend or not.
            */
            $scope.checkWeekend = function (date) {
                var workDate = new Date(date.WorkDate);

                if (workDate.getDay() === 0 || workDate.getDay() === 6) {
                    return true;
                }

                return false;
            };

            /*
            ** Get all meal timesheet.
            */
            mealRegistrationFactory.getAllMealTimesheet().success(function (mealTimesheet) {
                $scope.mealTimesheet = mealTimesheet;
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            /*
            ** Get next or previous month
            */
            $scope.changeMonth = function (numberMonth) {
                var currentDate = $scope.currentDate,
                    changedMonth = new Date(currentDate.setMonth(currentDate.getMonth() + numberMonth));
                $scope.currentDate = changedMonth;
            };

            /*
            ** Get register meal when current date change.
            */
            $scope.$watch('currentDate', function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    var changedMonth = $scope.currentDate,
                        fromDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth(), 1),
                        toDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth() + 1, 0);

                    getRegisterMealByRangeDate(fromDate, toDate, null);
                    getVegetarianManagementByRangeDate(fromDate, toDate, null)
                }
            });

            /*
            ** Check if a week has at least one enabled date.
            */
            $scope.hasDateEnableInWeek = function (week) {
                var hasEnableDate = mealRegistrationUtilities.hasDateEnableInWeek(week);
                return hasEnableDate;
            };

            /*
            ** Check if all same date in month has at least one enable date.
            */
            $scope.hasDateEnabled = function (date, weeks) {
                var hasEnableDate = mealRegistrationUtilities.hasDateEnabled(date, weeks);
                return hasEnableDate;
            };

            /*
            ** Check if month has at least one enabled date.
            */
            $scope.hasDateEnabledInMonth = function (weeks) {
                var hasEnableDate = false;

                angular.forEach(weeks, function (week) {
                    var hasEnableDateInWeek = mealRegistrationUtilities.hasDateEnableInWeek(week, $scope.mealTimesheet);

                    if (hasEnableDateInWeek) {
                        hasEnableDate = true;
                    }
                });

                return hasEnableDate;
            };

            /*
            ** If route change , close dialog.
            */
            $scope.$on('$routeChangeStart', function () {
                // if modal instance defined, dismiss dialog
                if ($scope.modalRegisterInstance) {
                    $scope.modalRegisterInstance.dismiss('cancel');
                }
            });

            /* 
            ** Register meal for a date.
            */
            $scope.registerMeal = function (date) {
                $scope.modalRegisterInstance = $modal.open({
                    templateUrl: baseUrl + 'src/app/EmployeeRegistration/Templates/registerMeal.html',
                    controller: "RegisterMealController",
                    backdrop: 'static',
                    resolve: {
                        meals: function () {
                            return angular.copy(date.Meals);
                        },
                        workDate: function () {
                            return date.WorkDate;
                        },
                        mealTimesheet: function () {
                            return $scope.mealTimesheet;
                        },
                        isSameMeal: function () {
                            return true;
                        },
                        mode: function () {
                            return 'month';
                        },
                        timePeriod: function () {
                            return new Date(date.WorkDate);
                        },
                        isSerialRegistration: function () {
                            return false;
                        },
                        registerSetting: function () {
                            return $scope.registerSetting;
                        },
                        listRegister: function () {
                            return $scope.empRegisterFood;
                        }
                    },
                    windowClass: "modal-confirm-center register-meal-dialog"
                });

                $scope.modalRegisterInstance.result.then(function (registrationInfo) {
                    var radioVegetarian = registrationInfo.meals;

                    var wd = new Date(date.WorkDate);
                    var isVegetarianDay = mealRegistrationUtilities.checkVegetarianDay(wd.getDate(), wd.getMonth() + 1, wd.getFullYear(), 7);
                    var getLunarDayWorkDate = mealRegistrationUtilities.getLunarDay(wd.getDate(), wd.getMonth() + 1, wd.getFullYear(), 7);

                    wd.setDate(wd.getDate() + 1);
                    var isBeforeVegetarianDay = mealRegistrationUtilities.checkVegetarianDay(wd.getDate(), wd.getMonth() + 1, wd.getFullYear(), 7);
                    var getLunarDayTheDayAfterWorkDate = mealRegistrationUtilities.getLunarDay(wd.getDate(), wd.getMonth() + 1, wd.getFullYear(), 7);

                    var noVegetarian = true;
                    radioVegetarian.forEach(x => {
                        if (x.IsVegetarian === 1 && x.MealId !== 4 && !isVegetarianDay && getLunarDayWorkDate.month !== 7) noVegetarian = false;
                        if (x.IsVegetarian === 1 && x.MealId === 4 && !isVegetarianDay && !isBeforeVegetarianDay && getLunarDayTheDayAfterWorkDate.month !== 7) noVegetarian = false;
                    });

                    if (!noVegetarian) {
                        notificationFactory.error("", "Can't register because it's not a vegetarian day or supper before vegetarian day");
                    } else {
                        var mealRegistration = {
                            meal: registrationInfo.mealsTemplate,
                            onDate: date.WorkDate.toString()
                        };

                        mealRegistrationFactory.registerMealByRangeDate(mealRegistration).success(function (response) {
                            if (response.IsSuccessful) {
                                date.Meals = registrationInfo.meals;
                                var changedMonth = $scope.currentDate,
                                    fromDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth(), 1),
                                    toDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth() + 1, 0);
                                getRegisterMealByRangeDate(fromDate, toDate, null);

                                notificationFactory.success("DataChangeSuccessfully");
                            } else {
                                notificationFactory.error("", response.Message);
                            }
                        }).error(function (response, status) {
                            if (status === 405) {
                                ErrorFactory.errorHandle(status);
                            } else {
                                notificationFactory.error("DataChangeFail");
                            }
                        });
                    }
                });
            };
        }]);