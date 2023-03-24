'use strict';
/*global userControlModule*/

userControlModule.controller('ManageUserController', manageUserController);
manageUserController.$inject = ['$scope', '$route', '$modal', 'userControlFactory', 'notificationFactory', 'ErrorFactory', 'commonFactory', 'baseUrl'];

function manageUserController($scope, $route, $modal, userControlFactory, notificationFactory, ErrorFactory, commonFactory, baseUrl) {
    $scope.filter = {
        keyword: '',
        companyId: '',
        teamId: '',
        roleId: ''
    };

    $scope.entrySelected = 20;
    commonFactory.numberEntryList($scope);

    userControlFactory.getManageUserPermission().success(function (permission) {
        $scope.policy = {
            permission: parseInt(permission)
        };
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    userControlFactory.getAllCompany().success(function (companies) {
        $scope.companyOptions = companies;
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    $scope.getTeamByCompany = function (companyId) {
        if (companyId) {
            userControlFactory.getAllTeamByCompany(companyId).success(function (teams) {
                $scope.teamOptions = teams;
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });
        }
        $scope.teamOptions = '';
        $scope.filter.teamId = '';
    };

    userControlFactory.getAllSystemRole().success(function (roles) {
        $scope.roleOptions = roles;
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    userControlFactory.getDefaultPassword().success(function (password) {
        $scope.defaultPassword = password.split('"')[1];
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    $scope.showEntries = function () {
        commonFactory.tablePagination($scope, $scope.employees, $scope.entrySelected);
    };

    function getAllEmployeeRole(filter) {
        userControlFactory.getAllEmployeeRole(filter.keyword, filter.companyId, filter.teamId, filter.roleId)
            .success(function (employees) {
                $scope.employees = employees;
                commonFactory.tablePagination($scope, employees, $scope.entrySelected);
            })
            .error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });
    }

    function getSelectedRole(filter) {
        filter.roleId = '';

        angular.forEach($scope.roleOptions, function (value) {
            if (value.ticked) {
                var roleId = value.RoleId;
                filter.roleId = filter.roleId !== '' ? filter.roleId + ',' + roleId : roleId;
            }
        });
    }

    $scope.query = function (filter) {
        getSelectedRole(filter);
        getAllEmployeeRole(filter);
    };
    $scope.ToggleFreeOrder = function (employee) {
        employee.IsFreeOrder = !employee.IsFreeOrder;
        var action = employee.IsFreeOrder ? "activate free order" : "deactivate free order";

        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/components/extensions/common/templates/ModalConfirmContent.html',
            controller: "ModalConfirmController",
            resolve: {
                name: function () {
                    return employee.NickName;
                },
                action: function () {
                    return action;
                }
            },
            windowClass: "modal-confirm-center special-employee-modal"
        });

        $scope.modalInstance.result.then(function () {
            var criteria = {
                userId: employee.UserId,
                isFreeOrder: employee.IsFreeOrder
            }

            userControlFactory.assignFreeOrderToUser(criteria).success(function (response) {
                if (response.IsSuccessful) {
                    notificationFactory.success("DataChangeSuccessfully");
                }
                else {
                    employee.IsFreeOrder = !employee.IsFreeOrder;
                    notificationFactory.error("", response.message);
                }
            }).error(function (response, status) {
                employee.IsFreeOrder = !employee.IsFreeOrder;
                ErrorFactory.errorHandle(status);
            });
        }, function () {
            employee.IsFreeOrder = !employee.IsFreeOrder;
        })['finally'](function () {
            $scope.modalInstance = '';
        });
    };

    $scope.assignRole = function (employee) {
        var modalAssignInstance = $modal.open({
            templateUrl: baseUrl + 'src/app/UserControl/Templates/UserPermission/assignRole.html',
            controller: 'AssignRoleToUserController',
            backdrop: 'static',
            resolve: {
                userId: function () {
                    return employee.UserId;
                }
            }
        });

        modalAssignInstance.result.then(function (userRoles) {
            userControlFactory.assignRoleToUser(userRoles).success(function (response) {
                if (response.IsSuccessful) {
                    notificationFactory.success('DataChangeSuccessfully');
                    getAllEmployeeRole($scope.filter);
                }
                else {
                    notificationFactory.error('', response.Message);
                }
            }).error(function (response, status) {
                if (status === 405) {
                    ErrorFactory.errorHandle(status);
                }
                else {
                    notificationFactory.error('DataChangeFail');
                }
            });
        });
    };

    $scope.resetPassword = function (employee) {
        var modalResetPassword = $modal.open({
            templateUrl: baseUrl + 'src/app/UserControl/Templates/UserPermission/resetPassword.html',
            controller: 'ResetPasswordController',
            backdrop: 'static',
            resolve: {
                employee: function () {
                    return employee;
                },
                defaultPassword: function () {
                    return $scope.defaultPassword;
                }
            },
            windowClass: 'modal-reset-password'
        });

        modalResetPassword.result.then(function (userId) {
            userControlFactory.resetPassword(userId).success(function (response) {
                if (response.IsSuccessful) {
                    notificationFactory.success('DataChangeSuccessfully');
                }
                else {
                    notificationFactory.error('', response.Message);
                }
            }).error(function (response, status) {
                if (status === 405) {
                    ErrorFactory.errorHandle(status);
                }
                else {
                    notificationFactory.error('DataChangeFail');
                }
            });
        });
    };

    $scope.reload = function () {
        $route.reload();
    };

    getAllEmployeeRole($scope.filter);
}