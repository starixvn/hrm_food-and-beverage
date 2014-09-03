'use strict';
/*global orderingModule*/

orderingModule.controller("OrderingController",
['$scope', '$route', 'orderingFactory',
function ($scope, $route, orderingFactory) {
	orderingFactory.getOrderForAddNew()
	.success(function (data) {
		$scope.order = data;
	})
	.error(function () {
		// TODO
	});

	orderingFactory.getLocation()
	.success(function (data) { 
		$scope.locations = data;
	})
	.error(function () { 
		// TODO
	});

		$scope.addMember = function () {
		/**
		* Ajax event loaded. 
		*/
		var successAddProjectMember = function () {
			notificationFactory.success('DataChangeSuccessfully', '');
			projectMembersFactory.getMember(projectIdInUrl, null, null).then(successGetMembers);
		};

		// Popup Add New Memeber.
		var populateModalInstance = function (roles) {
			var addModalInstance = $modal.open({
				templateUrl: '/src/app/ordering/templates/addmember.html',
				controller: "AddMemberController",
				backdrop: 'static',
				resolve: {
					roles: function () {
						return roles;
					},
					projectId: function () {
						return projectIdInUrl;
					}
				},
				windowClass: "modal-confirm-center"
			});
			addModalInstance.result.then(function (member) {
				if (member.joinDate != null) {
					var selectedStartDate = new Date(member.joinDate);
					member.joinDate = selectedStartDate ? selectedStartDate.toDateString() : null;
				}
				projectMembersFactory.addMember(member).then(successAddProjectMember, errorNotification);
			});
		};
