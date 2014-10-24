'use strict';
/*global setMenuManagementModule*/

setMenuManagementModule.directive('resetSetMenu', function () {
	var clearInputData = function (setMenu) {
		setMenu.SetMenuCode = '';
		setMenu.SetMenuVnmName = '';
		setMenu.IsActive = true;
		setMenu.FoodItems = [];
		setMenu.Menus = [];
		setMenu.RawFoodIds = [];
		setMenu.RawMenuIds = [];
	};

	return {
		link: function (scope) {
			scope.$on('resetSetMenu', function () {
				clearInputData(scope.setMenu);
				var foodChosenSelect = $("[name='foodSelect']");
				$(foodChosenSelect).val('').trigger("chosen:updated");

				var menuChosenSelect = $("[name='menuSelect']");
				$(menuChosenSelect).val('').trigger("chosen:updated");
			});
		}
	};
});