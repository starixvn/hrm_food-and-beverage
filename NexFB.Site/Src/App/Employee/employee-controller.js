'use strict';
/*global employeeModule*/

employeeModule.controller('EmployeeController', employeeController);
employeeController.$inject = ['$scope', '$modal', 'employeeFactory', 'notificationFactory', 'commonFactory', 'ErrorFactory', 'baseUrl'];

function employeeController($scope, $modal, employeeFactory, notificationFactory, commonFactory, ErrorFactory, baseUrl) {
    $scope.filter = {
        companyId: '',
        employeeTypeId: '',
        isActive: ''
    };

    $scope.originalPage = 0;
    $scope.entrySelected = 10;
    commonFactory.numberEntryList($scope);

    // If route change, close dialog.
    $scope.$on('$routeChangeStart', function () {
        // if modal instance defined, dismiss dialog
        if ($scope.modalInstance) {
            $scope.modalInstance.dismiss('cancel');
        }
    });

    // Paging.
    $scope.showEntries = function () {
        commonFactory.tablePagination($scope, $scope.users, $scope.entrySelected);
    };

    employeeFactory.getAllCompany().success(function (companies) {
        $scope.companyOptions = companies;
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    var getEmployeeType = function (companyId) {
        employeeFactory.getEmployeeType(companyId).success(function (employeeTypes) {
            $scope.employeeTypesOption = employeeTypes;
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    getEmployeeType('');

    /*
    ** Get all special users.
    */
    $scope.getSpecialUsers = function (filter) {
        var companyId = filter.companyId == null ? '' : filter.companyId;
        var employeeTypeId = filter.employeeTypeId == null ? '' : filter.employeeTypeId;
        employeeFactory.getSpecialUsers(companyId, employeeTypeId, filter.isActive).success(function (users) {
            $scope.users = users;
            commonFactory.tablePagination($scope, $scope.users, $scope.entrySelected);
            $scope.currentPage = $scope.originalPage;
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    $scope.getSpecialUsers($scope.filter);

    $scope.getEmployeeTypeByCompany = function (companyId) {
        getEmployeeType(companyId);
    };

    /*
    ** Get user permission on special employee.
    */
    employeeFactory.getPermissonSpecialEmployee().success(function (permission) {
        $scope.policy = {
            permission: permission
        };
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    /*
    ** Add special user.
    */
    $scope.addSpecialUser = function () {
        $scope.originalPage = $scope.currentPage;

        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/app/Employee/Templates/create.html',
            controller: "CreateSpecialEmployeeController",
            backdrop: 'static',
            resolve: {
                companyItems: function () {
                    return $scope.companyOptions;
                }
            },
            windowClass: "modal-confirm-center special-employee-modal"
        });

        $scope.modalInstance.result.then(function () {
        })['finally'](function () {
            $scope.getSpecialUsers($scope.filter);
            $scope.modalInstance = '';
        });
    };

    /*
    ** Edit special user.
    */
    $scope.editSpecialUser = function (user) {
        employeeFactory.getSpecialUserById(user.UserId).success(function (data) {
            $scope.modalInstance = $modal.open({
                templateUrl: baseUrl + 'src/app/Employee/Templates/edit.html',
                controller: "EditSpecialEmployeeController",
                backdrop: 'static',
                resolve: {
                    user: function () {
                        return data;
                    },
                    companyItems: function () {
                        return $scope.companyOptions;
                    }
                },
                windowClass: "modal-confirm-center special-employee-modal"
            });

            $scope.modalInstance.result.then(function () {
            })['finally'](function () {
                $scope.getSpecialUsers($scope.filter);
                $scope.modalInstance = '';
            });
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    /*
    ** Activate or deactivate special employee.
    */
    $scope.confirmActive = function (user) {
        user.IsActive = !user.IsActive;
        var action = user.IsActive ? "activate" : "deactivate";

        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/components/extensions/common/templates/ModalConfirmContent.html',
            controller: "ModalConfirmController",
            resolve: {
                name: function () {
                    return user.NickName;
                },
                action: function () {
                    return action;
                }
            },
            windowClass: "modal-confirm-center special-employee-modal"
        });

        var keyMessage = '';

        $scope.modalInstance.result.then(function () {
            employeeFactory.updateStatusSpecialUser(user).success(function (response) {
                if (response.IsSuccessful) {
                    keyMessage = action === "activate" ? "ActivationSuccess" : "DeactivationSuccess";
                    notificationFactory.success(keyMessage);
                }
                else {
                    keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
                    user.IsActive = !user.IsActive;

                    if (response.Code === 100) {
                        notificationFactory.error("DuplicateSetMenuMessage");
                    }
                    else {
                        notificationFactory.error(keyMessage);
                    }
                }
            }).error(function (response, status) {
                user.IsActive = !user.IsActive;
                ErrorFactory.errorHandle(status);
            });
        }, function () {
            user.IsActive = !user.IsActive;
        })['finally'](function () {
            $scope.modalInstance = '';
        });
    };

    /*
    ** Delete special user.
    */
    var updatePaging = function (user, currentPage) {
        var index = $scope.users.indexOf(user);
        $scope.users.splice(index, 1);

        commonFactory.tablePagination($scope, $scope.users, $scope.entrySelected);
        var numberPage = $scope.pagedItems.length;

        if (currentPage <= numberPage - 1) {
            $scope.currentPage = currentPage;
        }
        else {
            $scope.currentPage = currentPage - 1;
        }
    };

    $scope.deleteUser = function (user) {
        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/components/extensions/common/templates/ModalConfirmContent.html',
            controller: "ModalConfirmController",
            resolve: {
                action: function () {
                    return 'delete';
                },
                name: function () {
                    return user.NickName;
                }
            },
            windowClass: "modal-confirm-center special-employee-modal"
        });

        $scope.modalInstance.result.then(function () {
            var currentPage = $scope.currentPage;

            employeeFactory.deleteSpecialUser(user.UserId).success(function (response) {
                if (response.IsSuccessful) {
                    updatePaging(user, currentPage);
                    notificationFactory.success("DataChangeSuccessfully");
                }
                else {
                    notificationFactory.error("", response.Message);
                }
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });
        })['finally'](function () {
            $scope.modalInstance = '';
        });
    };
}