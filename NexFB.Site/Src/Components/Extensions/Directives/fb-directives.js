'use strict';
/*global appModule*/

// Assign Permission to Role
appModule.directive('assignPermission', function () {
	var updateParent = function (checked, type) {
		var pid = $(this).attr('pid');

		if (pid && pid.length > 0) {
			var parent = $("#func" + type + pid);

			if (checked && !parent.is(':checked')) {
				parent.prop('checked', true);
			}

			if (!checked) {
				var siblings = $("[name='func" + type + "'][pid='" + parent.val() + "']:checked");

				if (!siblings || (siblings && siblings.length === 0)) {
					parent.prop('checked', false);
				}
			}

			updateParent.call(parent.get(0), checked, type);
		}
	};

	return {
		restrict: "A",
		link: function (scope, iElement) {
			iElement.on("change", function () {
				var permissionType = $(this).attr('name'),
					currentId = $(this).val(),
					checked = $(this).is(':checked');

				if (permissionType === 'funcHost') {
					if (checked) {
						$("#funcRead" + currentId).prop('checked', true).trigger('change');
					}

					// parent changed -> children changed
					$("[name='funcHost'][pid='" + currentId + "']").prop('checked', checked).trigger('change');

					// update parent
					updateParent.call(this, checked, "Host");
				}
				else {
					if (!checked) {
						$("#funcHost" + currentId).prop('checked', false).trigger('change');
					}

					// parent changed -> children changed
					$("[name='funcRead'][pid='" + currentId + "']").prop('checked', checked).trigger('change');

					// update parent
					updateParent.call(this, checked, "Read");
				}
			});
		}
	};
});

// get role permission when click save button
appModule.directive('getRolePermission', function () {
	var createTemplate = function (functionId, isRead, isHost) {
		var template = '<Function FunctionId="' + functionId + '"' +
							' IsRead="' + isRead + '"' +
							' IsHost="' + isHost + '"></Function>';
		return template;
	};

	return {
		restrict: "A",
		link: function (scope, iElement) {
			iElement.on("click", function () {
				var result = '<ROOT>';
				var table = $("[name='tableFunction']");
				var roleFunction = $(table).find('[type=checkbox][name=funcRead]');

				if (roleFunction) {
					for (var i = 0; i < roleFunction.length; i++) {
						var element = roleFunction[i];
						var functionId = $(element).val(),
							isRead = $(element).is(':checked') ? 1 : 0,
							isHost = $('#funcHost' + functionId).is(':checked') ? 1 : 0,
							template = createTemplate(functionId, isRead, isHost);
						result += template;
					}

					result += '</ROOT>';

					scope.roleFunction = {
						functions: result,
						roleId: scope.filter.roleId
					};

					// call save function
					scope.save.call(this, scope.roleFunction);
				}
			});
		}
	};
});

// assign permission to user
appModule.directive('assignUserPermission', function () {
	return {
		restrict: "A",
		link: function (scope, iElement) {
			iElement.on("change", function () {
				var currentId = $(this).val(),
					checked = $(this).is(':checked'),
					parentId = $(this).attr('pid');

				if (parentId) {
					var parent = $('#role' + parentId);

					if (checked && !parent.is(':checked')) {
						parent.prop('checked', true);
					}
					else {
						var siblings = $("[name='department'][pid='" + parentId + "']:checked");

						if (!siblings || (siblings && siblings.length === 0)) {
							parent.prop('checked', false);
						}
					}
				}
				else if (currentId) {
					$("[name='department'][pid='" + currentId + "']").prop('checked', checked);
				}
			});
		}
	};
});

// get user permission when click save button
appModule.directive('getUserPermission', function () {
	var getDepartmentTemplate = function (roleDiv, roleId) {
		var result = '',
		template = '<RoleDepartment RoleID="' + roleId + '"',
		departments = $(roleDiv).find("[name='department'][pid='" + roleId + "']:checked");

		if (departments) {
			for (var i = 0; i < departments.length; i++) {
				var department = departments[i],
					departmentId = $(department).val(),
					departmentTemplate = template + ' DepartmentID="' + departmentId + '"/>';
				result = result !== '' ? result + departmentTemplate : departmentTemplate;
			}
		}

		return result;
	};

	var getDepartmentName = function (roleDiv, roleId) {
		var result = '',
			departments = $(roleDiv).find("[name='department'][pid='" + roleId + "']:checked");

		if (departments) {
			for (var i = 0; i < departments.length; i++) {
				var department = departments[i],
					departmentId = $(department).val(),
					departmentName = $('#departmentName' + departmentId).text();

				result = result !== '' ? result + ', ' + departmentName : departmentName;
			}
		}

		return result;
	};

	return {
		restrict: "A",
		link: function (scope, iElement) {
			iElement.on("click", function () {
				var roleDiv = $("[name='userRoles']"),
				roleItems = $(roleDiv).find("[name='role']:checked"),
				roles = '',
				rolesName = '',
				roleDepartments = '<ROOT>';

				if (roleItems) {
					for (var i = 0; i < roleItems.length; i++) {
						var item = roleItems[i],
							roleId = $(item).val(),
							roleName = $('#roleName' + roleId).text();

						roles = roles !== '' ? roles + ',' + roleId : roleId;
						rolesName = rolesName !== '' ? rolesName + ', ' + roleName : roleName;

						// get department of role if it has
						var departmentTemplate = getDepartmentTemplate(roleDiv, roleId);
						roleDepartments += departmentTemplate;

						// get department name
						var departmentsName = getDepartmentName(roleDiv, roleId);

						if (departmentsName) {
							rolesName += '(' + departmentsName + ')';
						}
					}
				}

				roleDepartments += '</ROOT>';

				// call save function
				scope.save.call(this, roles, roleDepartments, rolesName);
			});
		}
	};
});

// Manage User: remove class button, add class form-control to role multi-select
appModule.directive('addClassToRoleFilter',['$compile', function ($compile) {
	return {
		restrict: 'E',
		link: function (scope) {
			var roleGroup = $("[name='roleOption']"),
				button = roleGroup.find("button.multiSelectButton");

			button.removeClass('button').addClass('form-control');
			$compile(button)(scope);
		}
	};
} ]);

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

appModule.directive('onlyOneCheck', function () {
	return {
		restrict: 'A',
		link: function (scope, iElement) {
			iElement.on("click", function () {
				var mealId = $(this).val(),
					currentElementId = $(this).prop('id'),
					currentChecked = $(this).prop('checked'),
					isVegetarian = currentElementId.replace('meal' + mealId, '') == '_non' ? false : true,
					anotherElementId = isVegetarian ? ('#meal' + mealId + '_non') : ('#meal' + mealId),
					anotherChecked = $(anotherElementId).prop('checked');

				if (currentChecked) {
					$(anotherElementId).prop('checked', !currentChecked);
				}
			});
		}
	};
});

appModule.directive('getMealRegistration', function () {
	return {
		restrict: 'A',
		link: function (scope, iElement) {
			iElement.on('click', function () {
				var meals = scope.meals,
					mealsTemplate = '<ROOT>',
					registrationMealTable = $("[name='registrationMeal']"),
					mealRegister = $(registrationMealTable).find('tbody tr');

				for (var i = 0; i < mealRegister.length; i++) {
					var rowMeal = mealRegister[i],
					checkedElement = $(rowMeal).find('input:checked');

					if (checkedElement != null && checkedElement.length > 0) {
						var mealId = checkedElement.val(),
							checkedElementId = checkedElement.prop('id'),
							isVegetarian = checkedElementId.replace('meal' + mealId, '') == '_non' ? 0 : 1,
							template = '<Meal MealID="' + mealId + '" IsVeg="' + isVegetarian + '" />';

						meals[i].IsVegetarian = isVegetarian;
						mealsTemplate += template;
					}
				}

				mealsTemplate += '</ROOT>';
				scope.save.call(this, meals, mealsTemplate);
			});
		}
	};
});

appModule.directive('checkMealTimesheet', function () {
	return {
		restrictL 'A',
		link: function (scope) {
			
		}
	};
});