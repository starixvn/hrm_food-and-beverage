'use strict';
/*global groupControlModule*/

groupControlModule.directive('clearSearchMemberInput', function () {
	return {
		link: function (scope) {
			scope.$on('clearSearchMemberInput', function () {
				var searchMember = $.find('[name="txtMemberGroup"]');
				$(searchMember).val('');
			});
		}
	};
});