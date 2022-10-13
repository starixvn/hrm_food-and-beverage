'use strict';
/*global manageVegetarianModule*/
/*jshint maxdepth:6 */

manageVegetarianModule.factory('manageVegetarianFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
    return {
        //getEmployeeRegisterByRangeDate: function (fromDate, toDate, mealId) {
        //    return $http.get(baseUrl + 'api/MealRegistration/GetEmployeeRegisterFoodByRangeDate',
        //        { params: { fromDate: fromDate, toDate: toDate, mealId: mealId } });
        //},
        getVegetarianManagementByRangeDate: function (fromDate, toDate, mealId) {
            return $http.get(baseUrl + 'api/MealRegistration/GetVegetarianManagementByRangeDate',
                { params: { fromDate: fromDate, toDate: toDate, mealId: mealId } });
        },
    };
}]);
