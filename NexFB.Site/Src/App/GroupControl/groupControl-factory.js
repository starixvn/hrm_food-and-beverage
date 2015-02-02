'use strict';
/*global groupControlModule*/

groupControlModule.factory('groupControlFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
	return {
		getGroups: function (filter) {
			return $http.get(baseUrl + 'api/GroupControl/GetGroup', { params: filter });
		},
		getUserPermissionOnManageGroup: function () {
			return $http.get(baseUrl + 'api/GroupControl/GetUserPermissionOnManageGroup');
		},
		deleteGroup: function (groupId) {
			return $http['delete'](baseUrl + 'api/GroupControl/DeleteGroup', { params: { groupId: groupId} });
		},
		insertGroup: function (group) {
			return $http.post(baseUrl + 'api/GroupControl/InsertGroup', group);
		},
		updateGroup: function (group) {
			return $http.put(baseUrl + 'api/GroupControl/UpdateGroup', group);
		},
		activateGroup: function (group) {
			return $http.put(baseUrl + 'api/GroupControl/ActivateGroup', group);
		},
		assignMemberToGroup: function (filter) {
			return $http.post(baseUrl + 'api/GroupControl/AssignMemberToGroup', filter);
		},
		getEmployees: function (keyWord, departmentId, teamId, isShowDisable) {
			return $http.get(baseUrl + 'api/Employee/GetEmployees',
				{
					params: { 
						keyWord: keyWord,
						departmentId: departmentId,
						teamId: teamId,
						isShowDisable: isShowDisable
					}
				}
			);
		},
	};
} ]);

groupControlModule.factory('groupControlUtilities',
['notificationFactory', function (notificationFactory) {
	var utility = {};

	utility.validateGroup = function (group) {
		var isValid = true;

		if (!group) {
			isValid = false;
		}
		else {
			if (!group.GroupName) {
				isValid = false;
				notificationFactory.error("UserGroupNameRequire");
			}
		}

		return isValid;
	};

	return utility;
} ]);