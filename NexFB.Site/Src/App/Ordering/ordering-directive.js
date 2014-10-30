'use strict';
/*global orderingModule*/

orderingModule.directive('getSelectedMenu', function () {
	return {
		link: function (scope, iElement) {
			iElement.on('change', function () {
				var selectMenu = $.find('[name="newOrderMenu"]'),
					selectedOption = $(selectMenu).find(':selected'),
					isMainMenu = $(selectedOption).attr('isMainMenu');

				scope.member.Menus[0].IsMainMenu = isMainMenu;
			});
		}
	};
});

orderingModule.directive('getSelectedSavedMenu', function () {
	return {
		link: function (scope, iElement) {
			iElement.on('change', function () {
				var selectMenu = $.find('[name="savedOrderMenu"]'),
					selectedOption = $(selectMenu).find(':selected'),
					isMainMenu = $(selectedOption).attr('isMainMenu');

				scope.member.Menus[0].IsMainMenu = isMainMenu;
			});
		}
	};
});

orderingModule.directive('getSelectedEditMenu', function () {
	return {
		link: function (scope, iElement) {
			iElement.on('change', function () {
				var userId = scope.member.Member.UserId,
					elementName = 'editOrderMenu' + userId,
					selectMenu = $.find('[name="' + elementName  + '"]'),
					selectedOption = $(selectMenu).find(':selected'),
					isMainMenu = $(selectedOption).attr('isMainMenu');

				scope.member.Menus[0].IsMainMenu = isMainMenu;
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

orderingModule.filter('range', function () {
	return function (input, total) {
		total = parseInt(total);

		for (var i = 1; i <= total; i++) {
			input.push(i);
		}

		return input;
	};
});

