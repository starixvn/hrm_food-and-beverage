'use strict';
/*global appModule*/

appModule.factory('commonFactory',
['$filter', function ($filter) {
	var common = {};

	common.getDateFormat = function ($scope) {
		$scope.dateFormat = "dd MMM, yyyy";
	};

	common.formatString = function () {
		var theString = arguments[0];
		for (var i = 1; i < arguments.length; i++) {
			var regEx = new RegExp("\\{" + (i - 1) + "\\}", "gm");
			theString = theString.replace(regEx, arguments[i]);
		}

		return theString;
	};

	common.numberEntryList = function ($scope) {
		$scope.entries = [10, 15, 20, 50, 100, 200];
	};

	common.openPopupStartDate = function ($event, $scope) {
		$event.preventDefault();
		$event.stopPropagation();

		$scope.calendar.startDateOpened = true;
		$scope.calendar.endDateOpened = false;
	};

	common.openPopupEndDate = function ($event, $scope) {
		$event.preventDefault();
		$event.stopPropagation();

		$scope.calendar.endDateOpened = true;
		$scope.calendar.startDateOpened = false;
	};

	common.tablePagination = function ($scope, dataPagination, itemPerPage, attrSearch) {
		/**
		* Scope Variable.
		*/
		$scope.gap = 7;
		$scope.filteredItems = [];
		$scope.groupedItems = [];
		$scope.attrSearch = attrSearch;

		if (itemPerPage != null) {
			$scope.itemsPerPage = itemPerPage;
		}
		else {
			$scope.itemsPerPage = 20;
		}

		$scope.pagedItems = [];
		$scope.currentPage = 0;
		/**
		* Init. 
		*/
		$scope.sort = {
			sortingOrder: 'id',
			reverse: false
		};

		/**
		* Search local data when data in table loaded. 
		*/
		var searchMatch = function (haystack, needle) {
			if (!needle) {
				return true;
			}

			return haystack.toLowerCase().indexOf(needle.toLowerCase()) !== -1;
		};

		var checkAllItemRecursion = function (item) {
			for (var attr in item) {
				var childItem = item[attr];

				if (childItem != null && /^[a-zA-Z0-9]*$/.test(attr) &&
					attr.toLowerCase().search('id') === -1 && attr.toLowerCase().search('length') === -1 &&
					attr.toLowerCase().search('index') === -1 && attr.toLowerCase().search('hash') === -1 &&
					attr.toLowerCase().search('time') === -1 && attr.toLowerCase().search('date') === -1 &&
					attr.toLowerCase().search('modifiedby') === -1 && attr.toLowerCase().search('createdby') === -1) {

					if (typeof (childItem) === 'object') {
						var exist = checkAllItemRecursion(childItem);

						if (exist) {
							return exist;
						}
					}
					else {
						if (searchMatch(item[attr].toString(), $scope.queryData)) {
							return true;
						}
					}
				}
			}

			return false;
		};

		// init the filtered items
		$scope.search = function () {
			$scope.filteredItems = [];

			if (dataPagination && dataPagination !== 'null') {
				$scope.filteredItems = $filter('filter')(dataPagination, function (item) {
					return checkAllItemRecursion(item);
				});
			}

			$scope.currentPage = 0;
			// now group by pages
			$scope.groupToPages();
		};

		/*
		** Use to search in Shift-Meal Mapping page.
		*/
		var checkShift = function (item, attrSearch) {
			for (var attr in item) {
				var childItem = item[attr];

				if (childItem != null && attrSearch && attr.toLowerCase().search(attrSearch.toLowerCase()) !== -1) {
					if (searchMatch(item[attr].toString(), $scope.queryData)) {
						return true;
					}
				}
			}

			return false;
		};

		$scope.searchInShift = function () {
			$scope.filteredItems = [];

			if (dataPagination && dataPagination !== 'null') {
				$scope.filteredItems = $filter('filter')(dataPagination, function (item) {
					return checkShift(item, $scope.attrSearch);
				});
			}

			$scope.currentPage = 0;
			// now group by pages
			$scope.groupToPages();
		};

		// calculate page in place
		$scope.groupToPages = function () {
			$scope.pagedItems = [];
			for (var i = 0; i < $scope.filteredItems.length; i++) {
				$scope.filteredItems[i].itemIndex = i;
				if (i % $scope.itemsPerPage === 0) {
					$scope.pagedItems[Math.floor(i / $scope.itemsPerPage)] = [$scope.filteredItems[i]];
				} else {
					$scope.pagedItems[Math.floor(i / $scope.itemsPerPage)].push($scope.filteredItems[i]);
				}
			}
		};

		/**
		* Event. 
		*/
		$scope.range = function (size, start, end) {
			var ret = [];
			if (size < end) {
				end = size;
				start = size - $scope.gap;
				if (start < 0) {
					start = 0;
				}
			}
			for (var i = start; i < end; i++) {
				ret.push(i);
			}
			return ret;
		};

		$scope.firstPage = function () {
			if ($scope.currentPage > 0) {
				$scope.currentPage = 0;
			}
		};

		$scope.lastPage = function () {
			if ($scope.currentPage < $scope.pagedItems.length - 1) {
				$scope.currentPage = $scope.pagedItems.length - 1;
			}

		};

		$scope.prevPage = function () {
			if ($scope.currentPage > 0) {
				$scope.currentPage--;
			}
		};

		$scope.nextPage = function () {
			if ($scope.currentPage < $scope.pagedItems.length - 1) {
				$scope.currentPage++;
			}
		};

		$scope.setPage = function () {
			$scope.currentPage = this.n;
		};

		// functions have been describe process the data for display
		$scope.search();
	};

	/*
	** Update employee type.
	*/
	var getEmployeeTypeById = function (employeeTypeId, employeeTypesOption) {
		var employeeType = '',
			length = employeeTypesOption.length;

		for (var i = 0; i < length; i++) {
			var currentEmployeeType = employeeTypesOption[i];

			if (currentEmployeeType.EmployeeTypeId === employeeTypeId) {
				return currentEmployeeType;
			}
		}

		return employeeType;
	};

	common.updateEmployeeTypes = function (employeeTypeIds, employeeTypesOption) {
		var updateEmployeeTypeList = [],
			employeeTypeIdList = employeeTypeIds.split(',');

		angular.forEach(employeeTypeIdList, function (employeeTypeId) {
			var addedEmployeeType = getEmployeeTypeById(parseInt(employeeTypeId), employeeTypesOption);
			updateEmployeeTypeList.push(addedEmployeeType);
		});

		return updateEmployeeTypeList;
	};

	return common;
} ]);