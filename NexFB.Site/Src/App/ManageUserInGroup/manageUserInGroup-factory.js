'use strict';
/*global manageUserInGroupModule*/

manageUserInGroupModule.factory('manageUserInGroupFactory', ['$http', function ($http) {
    return {
        getMembers: function (filter) {
            return $http.get('/api/GroupControl/GetMember', { params: filter });
        },
        getAllDepartment: function () {
			return $http.get('/api/Common/GetAllDepartment');
		},
        getAllEmployeeType: function () {
			return $http.get('/api/Common/GetAllEmployeeType');
		},
        getAllGroup: function () {
			return $http.get('/api/GroupControl/GetAllGroup');
		},
        getUserPermissionOnManageUserInGroup: function () {
            return $http.get('/api/GroupControl/GetUserPermissionOnManageUserInGroup');
        },
        assignMemberToGroup: function (filter) {
			return $http.post('/api/GroupControl/AssignMemberToGroup', filter);
		}
    };
} ]);

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

    utility.getGroupIds = function(groups) {
        var groupIds = [];
        angular.forEach(groups, function(group) {
            groupIds.push(group.GroupId);
        });

        return groupIds;
    };

    return utility;
} ]);