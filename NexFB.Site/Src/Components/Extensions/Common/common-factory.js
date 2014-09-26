'use strict';
/*global appModule*/

appModule.factory('commonFactory',
['$filter', function ($filter) {
	var common = {};

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

	common.tablePagination = function ($scope, dataPagination, itemPerPage) {
		/**
		* Scope Variable. 
		*/
		$scope.gap = 7;
		$scope.filteredItems = [];
		$scope.groupedItems = [];

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

		// init the filtered items
		$scope.search = function () {
			$scope.filteredItems = $filter('filter')(dataPagination, function (item) {

				for (var attr in item) {
					if (item[attr] != null) {
						if (searchMatch(item[attr].toString(), $scope.queryData)) {
							return true;
						}
					}
				}
				return false;
			});
			// take care of the sorting order
			//			if ($scope.sort.sortingOrder !== '') {
			//				$scope.filteredItems = $filter('orderBy')($scope.filteredItems, $scope.sort.sortingOrder, $scope.sort.reverse);
			//			}
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
					//$scope.gap = $scope.pagedItems.length;
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

	return common;
} ]);