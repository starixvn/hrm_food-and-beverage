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
    ['$scope', 'mealRegistrationFactory', '$modal', 'notificationFactory', 'ErrorFactory', 'mealRegistrationUtilities', 'baseUrl',
        function ($scope, mealRegistrationFactory, $modal, notificationFactory, ErrorFactory, mealRegistrationUtilities, baseUrl) {
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

                mealRegistrationFactory.getEmployeeRegisterByRangeDate(fromDate.toDateString(), toDate.toDateString(), mealId).success(function (listRegister) {
                    $scope.empRegisterFood = listRegister;
                }).error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                });
            };



            /*
            ** Check show icon vegetarian or non-vegetarian
            */
            $scope.showIcon = function (id, date) {
                return "icon-allow-register-vegetarian";
            }

            /*
            ** Check show short name
            */
            $scope.showShortName = function (id, date, isVeg, meal) {
                return true;
            }
            $scope.test = function (id) {
                return true;
            }

            $scope.$watch('tabMonthly', function (newValue) {
                if (newValue) {
                    var changedMonth = $scope.currentDate,
                        fromDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth(), 1),
                        toDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth() + 1, 0);

                    getRegisterMealByRangeDate(fromDate, toDate, null);
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

            /* 
            ** Register meal for a week.
            */
            var updatedRegistrationMealToWeek = function (week, meals) {
                var hasDateEffectedVegetarian = false;

                angular.forEach(week.WeekMeals, function (date) {
                    var isEffected = mealRegistrationUtilities.updateMealForDate(date, meals, $scope.mealTimesheet, $scope.registerSetting);

                    if (isEffected) {
                        hasDateEffectedVegetarian = true;
                    }
                });

                return hasDateEffectedVegetarian;
            };

            $scope.registerMealWeek = function (week, index) {
                $scope.modalRegisterInstance = $modal.open({
                    templateUrl: baseUrl + 'src/app/EmployeeRegistration/Templates/registerMeal.html',
                    controller: "RegisterMealController",
                    backdrop: 'static',
                    resolve: {
                        meals: function () {
                            var date = mealRegistrationUtilities.getDateNotToday(week);

                            if (!date) {
                                date = mealRegistrationUtilities.getDateIsToday(week);
                            }

                            var copyMeal = angular.copy(date.Meals);
                            return mealRegistrationUtilities.resetDisableMeal(copyMeal);
                        },
                        workDate: function () {
                            return '';
                        },
                        mealTimesheet: function () {
                            return $scope.mealTimesheet;
                        },
                        isSameMeal: function () {
                            var sealedDate = mealRegistrationUtilities.getDateNotToday(week);
                            return mealRegistrationUtilities.checkSameMealInWeek(week, $scope.mealTimesheet, sealedDate);
                        },
                        mode: function () {
                            return 'month';
                        },
                        timePeriod: function () {
                            return 'Week ' + index;
                        },
                        isSerialRegistration: function () {
                            return true;
                        },
                        registerSetting: function () {
                            return $scope.registerSetting;
                        },
                        listRegister: function () {
                            return '';
                        }
                    },
                    windowClass: "modal-confirm-center register-meal-dialog"
                });

                $scope.modalRegisterInstance.result.then(function (registrationInfo) {
                    var radioVegetarian = registrationInfo.meals;
                    var noVegetarian = true;
                    radioVegetarian.forEach(x => {
                        if (x.IsVegetarian === 1)
                            noVegetarian = false;
                    });

                    if (!noVegetarian) {
                        notificationFactory.error("", "Can't register vegetarian for all days in week");
                    } else {
                        var mealRegistration = {
                            meal: registrationInfo.mealsTemplate,
                            onDate: mealRegistrationUtilities.getRegisterDateInWeek(week)
                        };
                        var changedMonth = $scope.currentDate,
                            fromDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth(), 1),
                            toDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth() + 1, 0);


                        mealRegistrationFactory.registerMealByRangeDate(mealRegistration).success(function (response) {
                            if (response.IsSuccessful) {
                                var isUpdateVegetarian = mealRegistrationUtilities.checkIsUpdateVegetarian(registrationInfo.meals),
                                    hasDateEffectedVegetarian = updatedRegistrationMealToWeek(week, registrationInfo.meals),
                                    hasNonRegisterVegetarianDate = mealRegistrationUtilities.checkNonRegisterVegetarianDateInWeek(week, $scope.registerSetting);

                                if (isUpdateVegetarian && hasNonRegisterVegetarianDate) {
                                    if (hasDateEffectedVegetarian) {
                                        notificationFactory.success("UpdateVegetarianInfo");
                                        getRegisterMealByRangeDate(fromDate, toDate, null);
                                    }
                                    else {
                                        notificationFactory.success("NonUpdateVegetarianWarning");
                                        getRegisterMealByRangeDate(fromDate, toDate, null);
                                    }
                                }
                                else {
                                    notificationFactory.success("DataChangeSuccessfully");
                                    getRegisterMealByRangeDate(fromDate, toDate, null);
                                }
                            }
                            else {
                                notificationFactory.error("", response.Message);
                            }
                        }).error(function (response, status) {
                            if (status === 405) {
                                ErrorFactory.errorHandle(status);
                            }
                            else {
                                notificationFactory.error("DataChangeFail");
                            }
                        });
                    }
                });
            };

            /* 
            ** Register meal for a month.
            */
            var updatedRegistrationMealToMonth = function (weeks, meals) {
                var hasDateEffectedVegetarian = false;

                angular.forEach(weeks, function (week) {
                    var effectedDayInWeek = updatedRegistrationMealToWeek(week, meals);

                    if (effectedDayInWeek) {
                        hasDateEffectedVegetarian = true;
                    }
                });

                return hasDateEffectedVegetarian;
            };

            $scope.registerMealMonth = function (allWeekMeals) {
                $scope.modalRegisterInstance = $modal.open({
                    templateUrl: baseUrl + 'src/app/EmployeeRegistration/Templates/registerMeal.html',
                    controller: "RegisterMealController",
                    backdrop: 'static',
                    resolve: {
                        meals: function () {
                            var date = mealRegistrationUtilities.getDateNotTodayInMonth(allWeekMeals);

                            if (!date) {
                                date = mealRegistrationUtilities.getDateIsTodayInMonth(allWeekMeals);
                            }

                            var copyMeal = angular.copy(date.Meals);
                            return mealRegistrationUtilities.resetDisableMeal(copyMeal);
                        },
                        workDate: function () {
                            return '';
                        },
                        mealTimesheet: function () {
                            return $scope.mealTimesheet;
                        },
                        isSameMeal: function () {
                            return mealRegistrationUtilities.checkSameMealInMonth(allWeekMeals, $scope.mealTimesheet);
                        },
                        mode: function () {
                            return 'month';
                        },
                        timePeriod: function () {
                            return new Date($scope.currentDate).toString('MMM yyyy');
                        },
                        isSerialRegistration: function () {
                            return true;
                        },
                        registerSetting: function () {
                            return $scope.registerSetting;
                        },
                        listRegister: function () {
                            return '';
                        }
                    },
                    windowClass: "modal-confirm-center register-meal-dialog"
                });

                $scope.modalRegisterInstance.result.then(function (registrationInfo) {
                    var radioVegetarian = registrationInfo.meals;
                    var noVegetarian = true;
                    radioVegetarian.forEach(x => {
                        if (x.IsVegetarian === 1)
                            noVegetarian = false;
                    });

                    if (!noVegetarian) {
                        notificationFactory.error("", "Can't register vegetarian for all days in month");
                    } else {
                        var mealRegistration = {
                            meal: registrationInfo.mealsTemplate,
                            onDate: mealRegistrationUtilities.getRegisterDateInMonth(allWeekMeals)
                        };
                        mealRegistrationFactory.registerMealByRangeDate(mealRegistration).success(function (response) {
                            if (response.IsSuccessful) {
                                var isUpdateVegetarian = mealRegistrationUtilities.checkIsUpdateVegetarian(registrationInfo.meals),
                                    hasDateEffectedVegetarian = updatedRegistrationMealToMonth(allWeekMeals, registrationInfo.meals),
                                    hasNonRegisterVegetarianDate = mealRegistrationUtilities.checkNonRegisterVegetarianDateInMonth(allWeekMeals, $scope.registerSetting);

                                var changedMonth = $scope.currentDate,
                                    fromDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth(), 1),
                                    toDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth() + 1, 0);

                                if (isUpdateVegetarian && hasNonRegisterVegetarianDate) {
                                    if (hasDateEffectedVegetarian) {
                                        notificationFactory.success("UpdateVegetarianInfo");
                                        getRegisterMealByRangeDate(fromDate, toDate, null);
                                    }
                                    else {
                                        notificationFactory.success("NonUpdateVegetarianWarning");
                                        getRegisterMealByRangeDate(fromDate, toDate, null);
                                    }
                                }
                                else {
                                    notificationFactory.success("DataChangeSuccessfully");
                                    getRegisterMealByRangeDate(fromDate, toDate, null);
                                }
                            }
                            else {
                                notificationFactory.error("", response.Message);
                            }
                        }).error(function (response, status) {
                            if (status === 405) {
                                ErrorFactory.errorHandle(status);
                            }
                            else {
                                notificationFactory.error("DataChangeFail");
                            }
                        });
                    }
                });
            };

            /* 
            ** Register meal for a same date in month.
            */
            var updatedRegistrationMealToSameDateInWeek = function (weeks, meals, weekDayIndex) {
                var hasDateEffectedVegetarian = false;

                angular.forEach(weeks, function (week) {
                    var date = week.WeekMeals[weekDayIndex];
                    var isEffectedDate = mealRegistrationUtilities.updateMealForDate(date, meals, $scope.mealTimesheet, $scope.registerSetting, true);

                    if (isEffectedDate) {
                        hasDateEffectedVegetarian = isEffectedDate;
                    }
                });

                return hasDateEffectedVegetarian;
            };

            $scope.registerMealDate = function (date, allWeekMeals) {
                var weekDayIndex = mealRegistrationUtilities.getWeekDayIndex(date);

                $scope.modalRegisterInstance = $modal.open({
                    templateUrl: baseUrl + 'src/app/EmployeeRegistration/Templates/registerMeal.html',
                    controller: "RegisterMealController",
                    backdrop: 'static',
                    resolve: {
                        meals: function () {
                            var date = mealRegistrationUtilities.getWeekDayNotTodayInMonth(allWeekMeals, weekDayIndex);

                            if (!date) {
                                date = mealRegistrationUtilities.getWeekDayIsToday(allWeekMeals, weekDayIndex);
                            }

                            var copyMeal = angular.copy(date.Meals);
                            return mealRegistrationUtilities.resetDisableMeal(copyMeal);
                        },
                        workDate: function () {
                            return '';
                        },
                        mealTimesheet: function () {
                            return $scope.mealTimesheet;
                        },
                        isSameMeal: function () {
                            return mealRegistrationUtilities.checkSameMealWeekDayInMonth(allWeekMeals, weekDayIndex, $scope.mealTimesheet);
                        },
                        mode: function () {
                            return 'month';
                        },
                        timePeriod: function () {
                            return mealRegistrationUtilities.getFullNameWeekday(date);
                        },
                        isSerialRegistration: function () {
                            return true;
                        },
                        registerSetting: function () {
                            return $scope.registerSetting;
                        },
                        listRegister: function () {
                            return '';
                        }
                    },
                    windowClass: "modal-confirm-center register-meal-dialog"
                });

                $scope.modalRegisterInstance.result.then(function (registrationInfo) {
                    var radioVegetarian = registrationInfo.meals;
                    var noVegetarian = true;
                    radioVegetarian.forEach(x => {
                        if (x.IsVegetarian === 1)
                            noVegetarian = false;
                    });

                    if (!noVegetarian) {
                        notificationFactory.error("", "Can't register vegetarian for all same days in month");
                    } else {
                        var mealRegistration = {
                            meal: registrationInfo.mealsTemplate,
                            onDate: mealRegistrationUtilities.getRegisterDateOfWeek(weekDayIndex, allWeekMeals)
                        };

                        mealRegistrationFactory.registerMealByRangeDate(mealRegistration).success(function (response) {
                            if (response.IsSuccessful) {
                                var isUpdateVegetarian = mealRegistrationUtilities.checkIsUpdateVegetarian(registrationInfo.meals),
                                    hasDateEffectedVegetarian = updatedRegistrationMealToSameDateInWeek(allWeekMeals, registrationInfo.meals, weekDayIndex),
                                    hasNonRegisterVegetarianDate =
                                        mealRegistrationUtilities.checkNonRegisterVegetarianSameDateInMonth(allWeekMeals, weekDayIndex, $scope.registerSetting);
                                var changedMonth = $scope.currentDate,
                                    fromDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth(), 1),
                                    toDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth() + 1, 0);


                                if (isUpdateVegetarian && hasNonRegisterVegetarianDate) {
                                    if (hasDateEffectedVegetarian) {
                                        notificationFactory.success("UpdateVegetarianInfo");
                                        getRegisterMealByRangeDate(fromDate, toDate, null);
                                    }
                                    else {
                                        notificationFactory.success("NonUpdateVegetarianWarning");
                                        getRegisterMealByRangeDate(fromDate, toDate, null);
                                    }
                                }
                                else {
                                    notificationFactory.success("DataChangeSuccessfully");
                                    getRegisterMealByRangeDate(fromDate, toDate, null);
                                }
                            }
                            else {
                                notificationFactory.error("", response.Message);
                            }
                        }).error(function (response, status) {
                            if (status === 405) {
                                ErrorFactory.errorHandle(status);
                            }
                            else {
                                notificationFactory.error("DataChangeFail");
                            }
                        });
                    }
                });
            };
        }]);