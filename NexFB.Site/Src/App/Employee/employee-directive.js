'use strict';
/*global employeeModule*/

employeeModule.directive('ieSelectFix', ['$document',
function ($document) {
	return {
		restrict: 'A',
		require: 'ngModel',
		link: function (scope, element, attributes) {
			var isIE = $document[0] && $document[0].attachEvent, control;

			if (!isIE) {
				return;
			}

			control = element[0];
			// to fix IE8 and IE9 issue with parent and detail controller, we need to depend on the parent controller
			scope.$watch(attributes.ieSelectFix, function () {
				// this will add and remove the options to trigger the rendering in IE8 and IE9
				var option = document.createElement("option");
				control.add(option, null);
				control.remove(control.options.length - 1);
			});
		}
	};
}
]);