'use strict';
/*global groupControlModule*/

groupControlModule.controller('ManageGroupMemberController', manageGroupMemberController);

manageGroupMemberController.$inject = ['$scope', '$modalInstance', 'group', 'manageUserInGroupFactory', 'notificationFactory',
    'ErrorFactory', 'removeMessage', 'baseUrl', '$modal', 'commonFactory', 'groupControlFactory', 'permission', 'companies'];

function manageGroupMemberController($scope, $modalInstance, group, manageUserInGroupFactory, notificationFactory,
    ErrorFactory, removeMessage, baseUrl, $modal, commonFactory, groupControlFactory, permission, companies) {
    $scope.group = group;
    $scope.removeMessage = removeMessage;
    $scope.permission = permission;
    $scope.member = {};
    $scope.searchData = {};

    $scope.query = {
        groupId: $scope.group.GroupId
    };

    $scope.criteria = {
        groupIds: $scope.group.GroupId,
        sysIds: ''
    };

    /*
    ** Get employees.
    */
    $scope.getEmployee = function (searchKey) {
        const companyIds = companies.map(item => item.CompanyId).join(',');
        return groupControlFactory.getEmployees(searchKey, companyIds, null, true);
    };

    $scope.unSelectEmployee = function (searchKey) {
        if (searchKey.trim() === '') {
            if ($scope.filter && $scope.filter.employee) {
                $scope.filter.employee = null;
            }
        }
    };

    var sortMembers = function (members, userId) {
        var member = '',
            numberMembers = members.length;

        for (var i = 0; i < numberMembers; i++) {
            if (userId === members[i].SysId) {
                member = members[i];
                members.splice(i, 1);
                members.splice(0, 0, member);
                i = numberMembers;
            }
        }
    };

    /*
    ** Get members of group.
    */
    var getMembersOfGroup = function (filter, userId) {
        manageUserInGroupFactory.getMembers(filter).success(function (members) {
            $scope.members = members;

            if (userId && $scope.members && $scope.members.length > 0) {
                sortMembers($scope.members, userId);
            }

            $scope.searchMemberGroup();
        }).error(function () {
            notificationFactory.error("ErrorMessage");
        });
    };

    getMembersOfGroup($scope.query);

    /*
    ** Add member into group.
    */
    var checkExistMember = function (members, userId) {
        var isExisted = false;

        angular.forEach(members, function (member) {
            if (member.SysId === userId) {
                isExisted = true;
            }
        });

        return isExisted;
    };

    $scope.searchMemberGroup = function () {
        commonFactory.searchMemberGroup($scope, $scope.members, $scope.searchData.queryData);
    };

    $scope.add = function () {
        var employee = $scope.member.employee;

        if (employee) {
            $scope.criteria.sysIds = employee.UserId;
            var isExisted = checkExistMember($scope.members, employee.UserId),
                userId = employee.UserId;

            if (isExisted) {
                notificationFactory.error("ExistMemberWarning");
            }
            else {
                groupControlFactory.assignMemberToGroup($scope.criteria)
                    .success(function (response) {
                        if (response.IsSuccessful) {
                            notificationFactory.success("DataChangeSuccessfully");
                            getMembersOfGroup($scope.query, userId);
                        }
                        else {
                            notificationFactory.error("", response.Message);
                        }
                    }).error(function (response, status) {
                        ErrorFactory.errorHandle(status);
                    });
            }

            $scope.member.employee = '';
            $scope.$broadcast('clearSearchMemberInput');
        }
    };

    /*
    ** Delete member of group.
    */
    $scope.deleteMember = function (member) {
        $scope.modalInstance = $modal.open({
            templateUrl: baseUrl + 'src/components/extensions/common/templates/ModalConfirmContent2.html',
            controller: "ModalConfirmController2",
            backdrop: 'static',
            resolve: {
                content: function () {
                    return $scope.removeMessage;
                }
            },
            windowClass: "modal-confirm-center"
        });

        $scope.modalInstance.result.then(function () {
            manageUserInGroupFactory.deleteMemberGroup(member.SysId, $scope.group.GroupId)
                .success(function (response) {
                    if (response.IsSuccessful) {
                        var index = $scope.members.indexOf(member);
                        $scope.members.splice(index, 1);
                        $scope.searchMemberGroup($scope.members);
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

    $scope.cancel = function () {
        $modalInstance.dismiss("cancel");
    };
}