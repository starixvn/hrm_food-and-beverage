'use strict';
/*global manageUserInGroupModule*/

manageUserInGroupModule.factory('manageUserInGroupFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
    return {
        getMembers: function (filter) {
            return $http.get(baseUrl + 'api/GroupControl/GetMember', { params: filter });
        },
        getAllCompany: function () {
            return $http.get(baseUrl + 'api/common/getAllCompany');
        },
        getAllEmployeeType: function (companyId, isLocal) {
            return $http.get(baseUrl + 'api/Common/GetAllEmployeeType',
                { params: { companyId: companyId, isLocal: isLocal } });
        },
        getAllGroup: function () {
            return $http.get(baseUrl + 'api/GroupControl/GetAllGroup');
        },
        getUserPermissionOnManageUserInGroup: function () {
            return $http.get(baseUrl + 'api/GroupControl/GetUserPermissionOnManageUserInGroup');
        },
        assignGroupToMember: function (filter) {
            return $http.post(baseUrl + 'api/GroupControl/AssignGroupToMember', filter);
        },
        deleteMemberGroup: function (memberId, groupId) {
            return $http['delete'](baseUrl + 'api/GroupControl/DeleteMemberGroup',
                { params: { memberId: memberId, groupId: groupId } });
        }
    };
}]);

manageUserInGroupModule.factory('manageUserInGroupUtilities', ['notificationFactory',
    function () {
        var utility = {};

        utility.getEmployeeTypeId = function (employeeTypeOptions) {
            var employeeTypeIds = [];

            angular.forEach(employeeTypeOptions, function (value) {
                if (value.ticked) {
                    employeeTypeIds.push(value.EmployeeTypeId);
                }
            });

            return employeeTypeIds.join(',');
        };

        utility.getSelectedGroups = function (groupOptions) {
            var groups = [];

            angular.forEach(groupOptions, function (value) {
                if (value.ticked) {
                    groups.push({
                        GroupId: value.GroupId,
                        GroupName: value.GroupName
                    });
                }
            });

            return groups;
        };

        utility.getGroupIds = function (groups) {
            var groupIds = [];
            angular.forEach(groups, function (group) {
                groupIds.push(group.GroupId);
            });

            return groupIds;
        };

        utility.getTeamIds = function (teamOptions) {
            var teamIds = [];

            angular.forEach(teamOptions, function (value) {
                if (value.ticked) {
                    teamIds.push(value.TeamId);
                }
            });

            return teamIds.join(',');
        };

        /*
        ** Assign group to member.
        */
        utility.getSelectedGroupIds = function (groupOptions) {
            var groupIds = '';

            angular.forEach(groupOptions, function (group) {
                if (group.ticked) {
                    var id = group.GroupId;
                    groupIds = groupIds ? groupIds + ',' + id : id;
                }
            });

            return groupIds;
        };

        return utility;
    }]);