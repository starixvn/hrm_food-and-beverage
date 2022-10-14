'use strict';
/*global manageVegetarianModule*/
/*jshint maxdepth:6 */

manageVegetarianModule.factory('manageVegetarianFactory', ['$http', 'baseUrl', function ($http, baseUrl) {
    return {
        setVegetarianManagementByRangeDate: function (mealRegistration) {
            return $http.post(baseUrl + 'api/MealRegistration/SetVegetarianManagementBySingleDate', mealRegistration);
        },
        getVegetarianManagementByRangeDate: function (fromDate, toDate, mealId) {
            return $http.get(baseUrl + 'api/MealRegistration/GetVegetarianManagementByRangeDate',
                { params: { fromDate: fromDate, toDate: toDate, mealId: mealId } });
        },
    };
}]);
