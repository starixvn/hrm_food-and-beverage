'use strict';
/*global orderingModule*/

orderingModule.directive('getSelectedMenu', function () {
	return {
		link: function (scope, iElement) {
			iElement.on('change', function () {
				var selectMenu = $.find('[name="newOrderMenu"]'),
					selectedOption = $(selectMenu).find(':selected'),
					isMainMenu = $(selectedOption).attr('isMainMenu');

				scope.member.Menu.IsMainMenu = isMainMenu;
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

				scope.member.Menu.IsMainMenu = isMainMenu;
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

				scope.member.Menu.IsMainMenu = isMainMenu;
			});
		}
	};
});