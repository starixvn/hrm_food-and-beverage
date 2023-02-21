'use strict';
/*global employeeModule*/

employeeModule.factory('employeeFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
    return {
        getSpecialUsers: function (companyId, employeeTypeId, isActive) {
            return $http.get(baseUrl + 'api/Employee/GetSpecialUsers',
                { params: { companyId: companyId, employeeTypeId: employeeTypeId, isActive: isActive } });
        },
        getSpecialUserById: function (userId) {
            return $http.get(baseUrl + 'api/Employee/GetSpecialUserById',
                { params: { userId: userId } });
        },
        insertSpecialUser: function (user) {
            return $http.post(baseUrl + 'api/Employee/InsertSpecialUser', user);
        },
        updateSpecialUser: function (user) {
            return $http.put(baseUrl + 'api/Employee/UpdateSpecialUser', user);
        },
        deleteSpecialUser: function (userId) {
            return $http['delete'](baseUrl + 'api/Employee/DeleteSpecialUser', { params: { userId: userId } });
        },
        updateStatusSpecialUser: function (user) {
            return $http.put(baseUrl + 'api/Employee/UpdateStatusSpecialUser', user);
        },
        getEmployeeType: function (companyId, isLocal) {
            return $http.get(baseUrl + 'api/Common/GetAllEmployeeType',
                { params: { companyId: companyId, isLocal: isLocal } });
        },
        getAllCompany: function () {
            return $http.get(baseUrl + 'api/common/getAllCompany');
        },
        getPermissonSpecialEmployee: function () {
            return $http.get(baseUrl + 'api/Employee/GetUserPermissionOnSpecialEmployee');
        }
    };
}]);