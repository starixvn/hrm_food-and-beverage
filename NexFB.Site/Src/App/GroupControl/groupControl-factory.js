'use strict';
/*global groupControlModule*/

groupControlModule.factory('groupControlFactory', ['$http', function ($http) {
    return {
        getGroups: function (filter) {
            return $http.get('/api/GroupControl/GetGroup', { params: filter });
        },
        getUserPermissionOnManageGroup: function () {
            return $http.get('/api/GroupControl/GetUserPermissionOnManageGroup');
        },
        deleteGroup: function (groupId) {
			return $http['delete']('/api/GroupControl/DeleteGroup', { params: { groupId: groupId} });
		},
        insertGroup: function (group) {
			return $http.post('/api/GroupControl/InsertGroup', group);
		},
        updateGroup: function (group) {
			return $http.put('/api/GroupControl/UpdateGroup', group);
		}
    };
} ]);