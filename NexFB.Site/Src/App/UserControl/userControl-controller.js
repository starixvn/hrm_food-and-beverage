'use strict';
/*global userControlModule*/

userControlModule.controller("ManageRoleController",
    ['$scope', '$route', 'userControlFactory', 'notificationFactory', 'ErrorFactory',
        function ($scope, $route, userControlFactory, notificationFactory, ErrorFactory) {
            $scope.roleFunction = {
                functions: '',
                roleId: ''
            };

            userControlFactory.getAllSystemRole(true).success(function (roles) {
                if (roles) {
                    var roleId = roles[0].RoleId;
                    $scope.filter = {
                        roleId: roleId
                    };

                    getFunctionByRole(roleId);
                    $scope.roleOptions = roles;
                }
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            userControlFactory.getManageRolePermission().success(function (permission) {
                $scope.policy = {
                    permission: permission
                };
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            var getFunctionByRole = function (roleId) {
                userControlFactory.getFunctionByRole(roleId).success(function (functionGroup) {
                    $scope.functionGroup = functionGroup;
                }).error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                });
            };

            $scope.query = function (roleId) {
                getFunctionByRole(roleId);
            };

            $scope.save = function (roleFunction) {
                $scope.disabledSubmitButton = true;

                userControlFactory.assignPermissionToRole(roleFunction).success(function (response) {
                    if (response.IsSuccessful) {
                        notificationFactory.success("DataChangeSuccessfully");
                    }
                    else {
                        notificationFactory.error("", response.message);
                    }

                    $scope.disabledSubmitButton = false;
                }).error(function (response, status) {
                    ErrorFactory.errorHandle(status);
                    $scope.disabledSubmitButton = false;
                });
            };

            $scope.reload = function () {
                $route.reload();
            };
        }]);

userControlModule.controller('AssignRoleToUserController',
    ['$scope', '$modalInstance', 'userId', 'userControlFactory', 'ErrorFactory',
        function ($scope, $modalInstance, userId, userControlFactory, ErrorFactory) {

            userControlFactory.getEmployeeRoleById(userId).success(function (employee) {
                $scope.employee = employee;
                $scope.roles = employee.RoleItems;
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            userControlFactory.getAllSystemRole(false).success(function (systemRoles) {
                $scope.systemRoles = systemRoles;
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            userControlFactory.getAllCompany().success(function (companies) {
                $scope.companyOptions = companies;
            }).error(function (response, status) {
                ErrorFactory.errorHandle(status);
            });

            $scope.checkRolePermission = function (roleId) {
                var isChecked = false;
                angular.forEach($scope.roles, function (role) {
                    if (role.RoleId === roleId) {
                        isChecked = true;
                    }
                });

                return isChecked;
            };

            $scope.checkCompanyPermission = function (roleId, companyId) {
                var isChecked = false;
                angular.forEach($scope.roles, function (role) {
                    if (role.RoleId === roleId) {
                        angular.forEach(role.Companies, function (company) {
                            if (company.CompanyId === companyId) {
                                isChecked = true;
                            }
                        });
                    }
                });

                return isChecked;
            };

            $scope.save = function (roles, roleCompanies, isFreeOrder) {
                var userRoles = {
                    systemID: userId,
                    roleID: roles,
                    roleCompanies: roleCompanies,
                    isFreeOrder: isFreeOrder
                };
                $modalInstance.close(userRoles);
            };

            $scope.cancel = function () {
                $modalInstance.dismiss("cancel");
            };

            $scope.ChangeFreeOrder = function (employee) {
                employee.IsFreeOrder = !employee.IsFreeOrder;
            };
        }]);

userControlModule.controller('ResetPasswordController',
    ['$scope', '$modalInstance', 'commonFactory', 'employee', 'defaultPassword',
        function ($scope, $modalInstance, commonFactory, employee, defaultPassword) {
            $scope.nickName = employee.NickName;
            $scope.defaultPassword = defaultPassword;

            $scope.save = function () {
                $modalInstance.close(employee.UserId);
            };

            $scope.cancel = function () {
                $modalInstance.dismiss("cancel");
            };
        }]);