'use strict';
/*global groupControlModule*/

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
        }
//      deleteGroup: function (groupId) {
//			return $http['delete']('/api/GroupControl/DeleteGroup', { params: { groupId: groupId} });
//		},
//        insertGroup: function (group) {
//			return $http.post('/api/GroupControl/InsertGroup', group);
//		},
//        updateGroup: function (group) {
//			return $http.put('/api/GroupControl/UpdateGroup', group);
//		}
    };
} ]);


manageUserInGroupModule.factory('manageUserInGroupUtilities', ['notificationFactory',
function (notificationFactory) {
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

    utility.getGroupId = function (groupOptions) {
        var groupIds = [];

        angular.forEach(groupOptions, function (value) {
            if (value.ticked) {
                groupIds.push(value.GroupId);

            }
        });

        return groupIds.join(',');
    };

    return utility;
} ]);