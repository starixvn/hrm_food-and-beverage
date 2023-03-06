'use strict';
/*global userControlModule*/

userControlModule.factory('userControlFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
    return {
        getAllCompany: function () {
            return $http.get(baseUrl + 'api/common/getAllCompany');
        },
        getFunctionByRole: function (roleId) {
            return $http.get(baseUrl + 'api/UserControl/GetFunctionByRole', { params: { roleId: roleId } });
        },
        getAllSystemRole: function (exceptSystemAdmin) {
            return $http.get(baseUrl + 'api/UserControl/GetAllSystemRole', { params: { exceptSystemAdmin: exceptSystemAdmin } });
        },
        assignPermissionToRole: function (roleFunction) {
            return $http.post(baseUrl + 'api/UserControl/AssignPermissionToRole', roleFunction);
        },
        getAllEmployeeRole: function (keyword, companyId, teamId, roleId) {
            return $http.get(baseUrl + 'api/UserControl/GetAllEmployeeRole',
                { params: { keyword: keyword, companyId: companyId, teamId: teamId, roleId: roleId } });
        },
        getEmployeeRoleById: function (systemId) {
            return $http.get(baseUrl + 'api/UserControl/GetEmployeeRoleById', { params: { systemId: systemId } });
        },
        getAllTeamByCompany: function (companyId) {
            return $http.get(baseUrl + 'api/Common/GetAllTeamByCompany', { params: { companyId: companyId } });
        },
        getOrgChart: function (companyId) {
            return $http.get(baseUrl + 'api/Common/GetOrgChart', { params: { companyId: companyId } });
        },
        assignRoleToUser: function (userRoles) {
            return $http.post(baseUrl + 'api/UserControl/AssignRoleToUser', userRoles);
        },
        getManageRolePermission: function () {
            return $http.get(baseUrl + 'api/UserControl/GetManageRolePermission');
        },
        getManageUserPermission: function () {
            return $http.get(baseUrl + 'api/UserControl/GetManageUserPermission');
        },
        resetPassword: function (userId) {
            return $http.get(baseUrl + 'api/UserControl/ResetPassword', { params: { userId: userId } });
        },
        getDefaultPassword: function () {
            return $http.get(baseUrl + 'api/UserControl/GetDefaultPassword');
        }
    };
}]);