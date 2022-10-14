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
                    templateUrl: baseUrl + 'src/app/ManageVegetarian/Templates/registerMeal.html',
                    controller: "RegisterVegetarianCalendarController",
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
                        listVegetarianManagement: function () {
                            return $scope.listVegetarianManagement;
                        }
                    },
                    windowClass: "modal-confirm-center register-meal-dialog"
                });

                $scope.modalRegisterInstance.result.then(function (registrationInfo) {
                    var mealRegistration = {
                        meal: registrationInfo.mealsTemplate,
                        onDate: date.WorkDate.toString()
                    };

                    manageVegetarianFactory.setVegetarianManagementByRangeDate(mealRegistration).success(function (response) {
                        if (response.IsSuccessful) {
                            var changedMonth = $scope.currentDate,
                                fromDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth(), 1),
                                toDate = new Date(changedMonth.getFullYear(), changedMonth.getMonth() + 1, 0);
                            getVegetarianManagementByRangeDate(fromDate, toDate, null)

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
                });
            };
        }]);

manageVegetarianModule.controller('RegisterVegetarianCalendarController',
    ['meals', '$scope', 'workDate', '$modalInstance', 'mealTimesheet', 'mode', 'isSameMeal', 'timePeriod',
        'isSerialRegistration', '$modal', 'notificationFactory', 'registerSetting', 'listVegetarianManagement', 'baseUrl', 'mealRegistrationUtilities',
        function (meals, $scope, workDate, $modalInstance, mealTimesheet, mode, isSameMeal, timePeriod,
            isSerialRegistration, $modal, notificationFactory, registerSetting, listVegetarianManagement, baseUrl, mealRegistrationUtilities) {

            $scope.meals = meals;
            $scope.mode = mode;
            if (typeof (timePeriod) === "object") {
                $scope.timePeriod = timePeriod.toString("MMM dd, yyyy");
            } else {
                $scope.timePeriod = timePeriod;
            }

            $scope.registerSetting = registerSetting;
            $scope.originalMeals = angular.copy(meals);
            $scope.isNotToday = true;
            $scope.vegetarianDate = {};
            $scope.nonVegetarianDate = {};
            $scope.listVegetarianManagement = listVegetarianManagement;
            $scope.wd = workDate;

            $scope.isVegetarianMeal = function (id) {
                var isShow = $scope.listVegetarianManagement.find(item => (item.WorkDate === workDate && item.MealId === id));
                return isShow && isShow.IsAllowVegetarian
            }

            var checkWorkDateNotToday = function (workDate) {
                var toDay = new Date(),
                    currentDate = new Date(workDate);

                toDay.setHours(0, 0, 0, 0);
                currentDate.setHours(0, 0, 0, 0);

                if (+currentDate === +toDay) {
                    return false;
                }

                return true;
            };

            if (mode === 'month') {
                $scope.workDate = workDate;
                $scope.isRegisterDate = workDate !== '' || isSameMeal ? true : false;
                $scope.mealTimesheet = mealTimesheet;
                $scope.isNotToday = workDate !== '' ? checkWorkDateNotToday(workDate) : true;
            }
            else if (mode === 'default') {
                $scope.isRegisterDate = isSameMeal ? true : false;
            }

            if (!$scope.isRegisterDate) {
                $scope.checkNonRegistration = true;
            }

            $scope.submit = function () {
                var template = '<ROOT>',
                    registrationMealTable = $("[name='vegetarian-management']"),
                    mealRegister = $(registrationMealTable).find('tbody tr');

                for (var i = 0; i < mealRegister.length; i++) {
                    var row = mealRegister[i],
                        isChecked = $(row).find('input:checked').length, // = 0 is not checked, = 1 is checked
                        mealId = i + 1;

                    var templateItem = '<Meal MealID="' + mealId + '" IsAllowRegisteringVegetarian="' + isChecked + '" />';
                    template += templateItem;
                }

                template += '</ROOT>';

                var registrationInfo = {
                    mealsTemplate: template
                };
                $modalInstance.close(registrationInfo);
            }

            $scope.save = function (meals, mealsTemplate) {
                if (isSerialRegistration) {
                    $scope.modalConfirmInstance = $modal.open({
                        templateUrl: baseUrl + 'src/app/EmployeeRegistration/Templates/modalConfirmApplyTemplate.html',
                        controller: "ApplyTemplateConfirmController",
                        backdrop: 'static',
                        resolve: {
                            applyTemplate: function () {
                                return false;
                            }
                        },
                        windowClass: "modal-confirm-center register-meal-dialog"
                    });

                    $scope.modalConfirmInstance.result.then(function () {
                        var registrationInfo = {
                            meals: meals,
                            mealsTemplate: mealsTemplate
                        };
                        $modalInstance.close(registrationInfo);
                    });
                }
                else {
                    var registrationInfo = {
                        mealsTemplate: mealsTemplate
                    };
                    $modalInstance.close(registrationInfo);
                }
            };

            $scope.cancel = function () {
                $modalInstance.dismiss("cancel");
            };

            /*
            ** If route change , close dialog.
            */
            $scope.$on('$routeChangeStart', function () {
                // if modal instance defined, dismiss dialog
                if ($scope.modalConfirmInstance) {
                    $scope.modalConfirmInstance.dismiss('cancel');
                }
            });
        }]);