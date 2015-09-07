'use strict';
/*global reportModule*/

reportModule.controller('MealForecastDetailReportController',
['$scope', '$location', '$routeParams', 'baseUrl', 'reportFactory', 'foodMenuFactory', 'foodMenuUtilities', 'ErrorFactory', 'reportUtilities',
function ($scope, $location, $routeParams, baseUrl, reportFactory, foodMenuFactory, foodMenuUtilities, ErrorFactory, reportUtilities) {
    $scope.filter = {
        date: $routeParams.date,
        departmentId: $routeParams.departmentId,
		employeeTypeId: $routeParams.employeeTypeId,
        dietType: $routeParams.dietType
    };

    $scope.numberFormat = reportUtilities.getNumberFormat();
    $scope.dateFormat = reportUtilities.getDateFormatEmployeeOrderHistoryReport();
    $scope.originalDepartmentId = '';
    $scope.baseUrl = baseUrl;

    $scope.gridOption = {
        itemsPerPage: 50,
        currentPage: 0,
        startIndex: 0
    };

    /*
    ** Reset grid option.
    */
    var resetGridOption = function () {
        $scope.gridOption = {
            itemsPerPage: 50,
            currentPage: 0,
            startIndex: 0
        };
    };

    /*
    ** Reset paging option.
    */
    var resetPagingOption = function (filter) {
        filter.startIndex = 0;
        filter.itemsPerPage = 50;
        filter.isPagingCallback = false;
    };

    /*
    ** Get all department.
    */
    reportFactory.getDepartmentByUserRole().success(function (departments) {
        $scope.departmentOptions = departments;
        $scope.filter.departmentId = $routeParams.departmentId.split(',').length > 1 ? $routeParams.departmentId : parseInt($routeParams.departmentId);
        $scope.originalDepartmentId = $scope.filter.departmentId;
        $scope.getMealForecastDetailReport($scope.filter);
        getEmployeeType($scope.filter.departmentId, null);
    }).error(function (response, status) {
        ErrorFactory.errorHandle(status);
    });

    /*
    ** Get all employee type.
    */
    var getEmployeeType = function (departmentId, isLocal) {
		foodMenuFactory.getAllEmployeeType(departmentId, isLocal).success(function (employeeTypes) {
			$scope.employeeTypesOption = employeeTypes;
			if ($scope.filter.employeeTypeId.length > 0) {
				var ids = $scope.filter.employeeTypeId.split(',');
				angular.forEach($scope.employeeTypesOption, function (employeeType) {
					angular.forEach(ids, function (id) {
						if (employeeType.EmployeeTypeId === parseInt(id)) {
							employeeType.ticked = true;
						}
					});
				});
			}
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
    };

    /*
    ** Get employee type when department change.
    */
	$scope.getEmployeeTypeOfDepartment = function (departmentId) {
		$scope.filter.employeeTypeId = '';

		if (!departmentId) {
			$scope.filter.departmentId = $scope.originalDepartmentId;
			departmentId = $scope.originalDepartmentId;
		}

		getEmployeeType(departmentId, null);
    };

    /*
    ** Get report
    */
    $scope.getMealForecastDetailReport = function (filter) {
        reportFactory.getMealForecastDetailReport(filter).success(function (report) {
            $scope.report = report;
            $scope.$broadcast("renderMealForecastDetailReport");
        }).error(function (response, status) {
            ErrorFactory.errorHandle(status);
        });
    };

    /*
    ** Filter report.
    */
    $scope.query = function (filter) {
        resetGridOption();
        resetPagingOption(filter);

        filter.employeeTypeId = foodMenuUtilities.getEmployeeTypeId($scope.employeeTypesOption);

        $scope.getMealForecastDetailReport(filter);
    };

    /*
    **  Export excel
    */
    $scope.exportExcel = function (filter) {
        $scope.criteria = {
            onDate: filter.date,
            departmentId: filter.departmentId,
            isVegetarian: filter.dietType === '' ? '' : filter.dietType === '0' ? false : true
        };

        $scope.$broadcast('exportMealForecastDetailReport');
    };

    /*
    **  go back
    */
    $scope.goback = function () {
        $location.url('/MealForecastReport/Index');
    };
} ]);