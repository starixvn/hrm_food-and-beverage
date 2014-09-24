'use strict';
/*global appModule*/

appModule.directive('paginationBar', function () {
	return {
		restrict: 'AE',
		replace: 'true',
		template:
			'<ul class="pagination pull-right custom-paging">' +
				'<li ng-class="{disabled: currentPage == 0}">' +
					'<a class="first" href ng-click="firstPage()" title="First Page">«</a>' +
				'</li>' +
				'<li ng-class="{disabled: currentPage == 0}">' +
					'<a class="pre" href ng-click="prevPage()" title="Previous Page">‹</a>' +
				'</li>' +
				'<li ng-repeat="n in range(pagedItems.length, currentPage, currentPage + gap) " ng-click="setPage()">' +
					'<a href ng-bind="n + 1" ng-class="{pagingactive: n == currentPage}">1</a>' +
				'</li>' +
				'<li ng-class="{disabled: (currentPage) == pagedItems.length - 1}">' +
					'<a class="next" href ng-click="nextPage()" title="Next Page">›</a>' +
				'</li>' +
				'<li ng-class="{disabled: (currentPage) == pagedItems.length - 1}">' +
					'<a class="last" href ng-click="lastPage()" title="Last Page">»</a>' +
				'</li>' +
			'</ul>'
	};
});

appModule.directive('searchAutoComplete', ['$timeout', function ($timeout) {
	return {
		scope: { employee: "=employee", searchEmployees: "&searchEmployees" },
		link: function (scope, iElement) {
			iElement.autocomplete({
				source: function (request, response) {
					var employees = scope.searchEmployees();
					var data = [];
						angular.forEach(employees.data, function (employee) {
							data.push(
								{ value: employee.NickName + ' (' + employee.FullName + ')',
									id: employee.UserId,
									employee: employee
								});
							});

					response(data);
				},
				select: function (event, ui) {
					scope.employee = ui.item.employee;
					$timeout(function () {
						iElement.trigger('input');
					}, 0);
				},
				appendTo: iElement.parent()
			});
		}
	};
} ]);

appModule.directive('ngBox', function () {
	return function () {
		$(".boxtitle").on("click", function () {
			var header = $(this);
			$('.boxtitle').not(this).each(function () {
				var headerOther = $(this);
				var contentOther = headerOther.next();
				contentOther.slideUp();
			});
			var content = header.next();
			$(content).slideToggle();
		});
	};
});

// remove class button, add class form-control to multi-select
appModule.directive('addClassToRoleFilter', ['$compile', function ($compile) {
	return {
		restrict: 'E',
		link: function (scope) {
			var roleGroup = $("[name='multiSelectOption']"),
				button = roleGroup.find("button.multiSelectButton");

			button.removeClass('button').addClass('form-control');
			$compile(button)(scope);
		}
	};
} ]);

// multi-select use Chosen plugin
appModule.directive('multiSelectChosen', function () {
	return {
		link: function (scope, iElement) {
			scope.$watchCollection('groupOptions', function () {
				iElement.trigger("chosen:updated");
			});
			iElement.chosen({ width: "95%" });
		}
	};
});
