'use strict';
/*global appModule*/

appModule.filter('customDate', ['$filter', function ($filter) {
	return function (input) {
		if (input == null) { return ""; }
		var _date = $filter('date')(new Date(input), 'dd MMM, yyyy');
		return _date;
	};
} ]);
