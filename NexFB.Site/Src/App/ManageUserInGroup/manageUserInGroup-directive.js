'use strict';
/*global manageUserInGroupModule*/

manageUserInGroupModule.directive('getSelectedMember', function () {
	return {
		link: function (scope, iElement) {
			iElement.on('click', function () {
				var selectedMembers = $('#groupMember').find('td input:checked'),
					sysIds = '';

				angular.forEach(selectedMembers, function (member) {
					var id = $(member).val();
					sysIds += sysIds ? ',' + id : id;
				});

				scope.assignGroupToMember.call(this, sysIds);
			});
		}
	};
});

manageUserInGroupModule.directive('checkAllMemberInPage', function () {
	return {
		link: function (scope, iElement) {
			iElement.on('click', function () {
				var checked = this.checked;

				if (checked) {
					var uncheckMembers = $('#groupMember').find('td input:not(:checked)');

					angular.forEach(uncheckMembers, function (member) {
						$(member).prop('checked', true);
					});
				}
				else {
					var checkMembers = $('#groupMember').find('td input:checked');

					angular.forEach(checkMembers, function (member) {
						$(member).prop('checked', false);
					});
				}
			});
		}
	};
});

manageUserInGroupModule.directive('resetCheckAllMember', function () {
	return {
		link: function (scope) {
			scope.$on('resetCheckAllMember', function () {
				var checkAllCheckbox = $('#groupMember').find('[name="checkAllMember"]');
				checkAllCheckbox.prop('checked', false);

				var checkMembers = $('#groupMember').find('td input:checked');
				angular.forEach(checkMembers, function (member) {
					$(member).prop('checked', false);
				});
			});
		}
	};
});