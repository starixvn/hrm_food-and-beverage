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