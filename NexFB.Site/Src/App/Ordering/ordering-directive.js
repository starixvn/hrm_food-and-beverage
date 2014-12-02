'use strict';
/*global orderingModule*/

orderingModule.directive('getSelectedMenu', function () {
	return {
		link: function (scope, iElement) {
			iElement.on('change', function () {
				var index = scope.$index;
				scope.member.Menus[0] = scope.selectedNewMenu[index];
			});
		}
	};
});

orderingModule.directive('getSelectedSavedMenu', function () {
	return {
		link: function (scope, iElement) {
			iElement.on('change', function () {
				var index = scope.$index;
				scope.member.Menus[0] = scope.selectedSavedMenu[index];
			});
		}
	};
});

orderingModule.directive('getSelectedEditMenu', function () {
	return {
		link: function (scope, iElement) {
			iElement.on('change', function () {
				var index = scope.$index;
				scope.member.Menus[0] = scope.selectedMenu[index];
			});
		}
	};
});

// close Saved Order and Side Dish Order when don't have New Order
orderingModule.directive('closeOrderContent', function () {
	return {
		link: function (scope) {
			scope.$on('closeOrderContent', function () {
				var savedOrder = $.find('[name="savedOrderDiv"]'),
					sideDishOrder = $.find('[name="sideDishOrderDiv"]');

				$(savedOrder).hide();
				$(sideDishOrder).hide();
			});
		}
	};
});

orderingModule.directive('clearSearchMemberInput', function () {
	return {
		link: function (scope) {
			scope.$on('clearSearchMemberInput', function () {
				var searchNewMember = $.find('[name="txtEmployeeForNewOrder"]'),
					searchSavedMember = $.find('[name="txtEmployeeForSavedOrder"]'),
					searchSideDishMember = $.find('[name="txtMemberSideDish"]');

				$(searchNewMember).val('');
				$(searchSavedMember).val('');
				$(searchSideDishMember).val('');
			});
		}
	};
});

orderingModule.directive('onChangeSelect', function () {
	return {
		link: function (scope, iElement) {
			iElement.change(function () {
				$(this).blur();
			});
			
		}
	};
});

orderingModule.directive('changePlaceholder', ['$timeout', function (timer) {
	return {
		restrict: 'A',
		link: function (scope, iElement) {
			var changePlaceHolder = function () {
				var searchInput = iElement.find('.inputFilter');
				$(searchInput).attr('placeholder', 'Enter delivery area');
			};

			timer(changePlaceHolder, 0);
		}
	};
} ]);

orderingModule.filter('range', function () {
	return function (input, total) {
		total = parseInt(total);

		for (var i = 1; i <= total; i++) {
			input.push(i);
		}

		return input;
	};
});