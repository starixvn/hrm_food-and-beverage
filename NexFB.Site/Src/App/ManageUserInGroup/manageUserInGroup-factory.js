'use strict';
/*global manageUserInGroupModule*/

manageUserInGroupModule.factory('manageUserInGroupFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		getMembers: function (filter) {
			return $http.get(baseUrl + 'api/GroupControl/GetMember', { params: filter });
		},
		getAllDepartment: function () {
			return $http.get(baseUrl + 'api/Common/GetAllDepartment');
		},
		getAllEmployeeType: function () {
			return $http.get(baseUrl + 'api/Common/GetAllEmployeeType');
		},
		getAllGroup: function () {
			return $http.get(baseUrl + 'api/GroupControl/GetAllGroup');
		},
		getUserPermissionOnManageUserInGroup: function () {
			return $http.get(baseUrl + 'api/GroupControl/GetUserPermissionOnManageUserInGroup');
		},
		assignMemberToGroup: function (filter) {
			return $http.post(baseUrl + 'api/GroupControl/AssignMemberToGroup', filter);
		},
		deleteMemberGroup: function (memberId, groupId) {
			return $http['delete'](baseUrl + 'api/GroupControl/DeleteMemberGroup', 
					{ params: { memberId: memberId, groupId: groupId } });
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