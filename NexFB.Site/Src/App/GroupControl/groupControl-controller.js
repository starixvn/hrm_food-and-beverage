'use strict';
/*global groupControlModule*/

groupControlModule.controller("ManageGroupController",
['$scope', '$modal', '$route', 'commonFactory', 'groupControlFactory', 'notificationFactory', 'ErrorFactory',
'baseUrl', '$translate',
function ($scope, $modal, $route, commonFactory, groupControlFactory, notificationFactory, ErrorFactory,
baseUrl, $translate) {

	$scope.filter = {
		isActive: ''
	};

	$scope.originalPage = 0;

	$translate('RemoveMemberGroupConfirm').then(function (message) {
		$scope.removeMemberGroupConfirmMsg = message;
	});

	/*
	** Paging.
	*/
	$scope.showEntries = function () {
		commonFactory.tablePagination($scope, $scope.groups, $scope.entrySelected);
	};

	$scope.entrySelected = 10;

	commonFactory.numberEntryList($scope);

	/*
	** Get all groups for dropdown.
	*/
	groupControlFactory.getGroups($scope.filter).success(function (groups) {
		if (groups) {
			$scope.groupOptions = groups;
		}
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

	var getGroups = function (filter) {
		groupControlFactory.getGroups(filter).success(function (groups) {
			if (groups) {
				$scope.groups = groups;
				commonFactory.tablePagination($scope, $scope.groups, $scope.entrySelected);
				$scope.currentPage = $scope.originalPage;
			}
		}).error(function () {
			notificationFactory.error("ErrorMessage");
		});
	};

	/*
	** If route change, close dialog.
	*/
	$scope.$on('$routeChangeStart', function () {
		// if modal instance defined, dismiss dialog
		if ($scope.modalInstance) {
			$scope.modalInstance.dismiss('cancel');
		}
	});

	/*
	** Get user permission on manage group.
	*/
	groupControlFactory.getUserPermissionOnManageGroup().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	getGroups($scope.filter);

	$scope.query = function (filter) {
		getGroups(filter);
	};

	/*
	** Add group.
	*/
	$scope.addGroup = function () {
		$scope.originalPage = $scope.currentPage;

		$scope.modalInstance = $modal.open({
			templateUrl: baseUrl + 'src/app/GroupControl/Templates/create.html',
			controller: "CreateGroupController",
			windowClass: "modal-confirm-center manage-group-modal"
		});

		$scope.modalInstance.result.then(function () {
		})['finally'](function () {
			getGroups($scope.filter);
			$scope.modalInstance = '';
		});
	};

	/*
	** Edit group.
	*/

	var updateChangedGroup = function (group, changedGroup) {
		group.GroupId = changedGroup.GroupId;
		group.GroupName = changedGroup.GroupName;
		group.Members = changedGroup.Members;
		group.IsActive = changedGroup.IsActive;
	};

	$scope.editGroup = function (group) {
		$scope.modalInstance = $modal.open({
			templateUrl: baseUrl + 'src/app/GroupControl/Templates/edit.html',
			controller: "EditGroupController",
			resolve: {
				group: function () {
					return angular.copy(group);
				}
			},
			windowClass: "modal-confirm-center manage-group-modal"
		});

		$scope.modalInstance.result.then(function (changedGroup) {
			updateChangedGroup(group, changedGroup);
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};

	/*
	** Delete group.
	*/
	var updatePaging = function (group, currentPage) {
		var index = $scope.groups.indexOf(group);
		$scope.groups.splice(index, 1);

		commonFactory.tablePagination($scope, $scope.groups, $scope.entrySelected);
		var numberPage = $scope.pagedItems.length;

		if (currentPage <= numberPage - 1) {
			$scope.currentPage = currentPage;
		}
		else {
			$scope.currentPage = currentPage - 1;
		}
	};

	$scope.deleteGroup = function (group) {
		$scope.modalInstance = $modal.open({
			templateUrl: baseUrl + 'src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			resolve: {
				action: function () {
					return 'delete';
				},
				name: function () {
					return group.GroupName;
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		$scope.modalInstance.result.then(function () {
			var currentPage = $scope.currentPage;

			groupControlFactory.deleteGroup(group.GroupId).success(function (response) {
				if (response.IsSuccessful) {
					updatePaging(group, currentPage);
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};

	/*
	** Manage member of group.
	*/
	$scope.manageGroupMember = function (group) {
		$scope.originalPage = $scope.currentPage;

		$scope.modalInstance = $modal.open({
			templateUrl: baseUrl + 'src/app/groupControl/templates/manageGroupMember.html',
			controller: "ManageGroupMemberController",
			resolve: {
				group: function () {
					return group;
				},
				removeMessage: function () {
					return $scope.removeMemberGroupConfirmMsg;
				},
				permission: function () {
					return $scope.policy.permission;
				}
			},
			backdrop: 'static',
			windowClass: "modal-confirm-center manage-group-member"
		});

		$scope.modalInstance.result.then()
		['finally'](function () {
			getGroups($scope.filter);
			$scope.modalInstance = '';
		});
	};


	/*
	** Activate or deactivate group.
	*/
	$scope.confirmActive = function (group) {
		var action = !group.IsActive ? "activate" : "deactivate";
		group.IsActive = !group.IsActive;

		$scope.modalInstance = $modal.open({
			templateUrl: baseUrl + 'src/components/extensions/common/templates/ModalConfirmContent.html',
			controller: "ModalConfirmController",
			backdrop: 'static',
			resolve: {
				name: function () {
					return group.GroupName;
				},
				action: function () {
					return action;
				}
			},
			windowClass: "modal-confirm-center food-item-modal"
		});

		var keyMessage = '';

		$scope.modalInstance.result.then(function () {
			groupControlFactory.activateGroup(group).success(function (response) {
				if (response.IsSuccessful) {
					keyMessage = action === "activate" ? "ActivationSuccess" : "DeactivationSuccess";
					notificationFactory.success(keyMessage);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					group.IsActive = !group.IsActive;
					notificationFactory.error(keyMessage);
				}
			}).error(function (response, status) {
				if (status === 405) {
					ErrorFactory.errorHandle(status);
				}
				else {
					keyMessage = action === "activate" ? "ActivationFail" : "DeactivationFail";
					group.IsActive = !group.IsActive;
					notificationFactory.error(keyMessage);
				}
			});
		}, function () {
			group.IsActive = !group.IsActive;
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};
} ]);

groupControlModule.controller('ManageGroupMemberController',
['$scope', '$modalInstance', 'group', 'manageUserInGroupFactory', 'orderingFactory', 'notificationFactory',
'ErrorFactory', 'removeMessage', 'baseUrl', '$modal', 'commonFactory', 'groupControlFactory', 'permission',
function ($scope, $modalInstance, group, manageUserInGroupFactory, orderingFactory, notificationFactory,
ErrorFactory, removeMessage, baseUrl, $modal, commonFactory, groupControlFactory, permission) {

	$scope.group = group;
	$scope.removeMessage = removeMessage;
	$scope.permission = permission;
	$scope.member = {};
	$scope.searchData = {};

	$scope.query = {
		groupId: $scope.group.GroupId
	};

	$scope.criteria = {
		groupIds: $scope.group.GroupId,
		sysIds: ''
	};

	/*
	** Get employees.
	*/
	$scope.getEmployee = function (searchKey) {
		orderingFactory.getEmployees(searchKey, true).then(
		function (employees) {
			$scope.searchEmployees = employees;
		},
		function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	var sortMembers = function (members, userId) {
		var member = '',
			numberMembers = members.length;

		for (var i = 0; i < numberMembers; i++) {
			if (userId === members[i].SysId) {
				member = members[i];
				members.splice(i, 1);
				members.splice(0, 0, member);
				i = numberMembers;
			}
		}
	};

	/*
	** Get members of group.
	*/
	var getMembersOfGroup = function (filter, userId) {
		manageUserInGroupFactory.getMembers(filter).success(function (members) {
			$scope.members = members;

			if (userId && $scope.members && $scope.members.length > 0) {
				sortMembers($scope.members, userId);
			}

			$scope.searchMemberGroup($scope.members);
		}).error(function () {
			notificationFactory.error("ErrorMessage");
		});
	};

	getMembersOfGroup($scope.query);

	/*
	** Add member into group.
	*/
	var checkExistMember = function (members, userId) {
		var isExisted = false;

		angular.forEach(members, function (member) {
			if (member.SysId === userId) {
				isExisted = true;
			}
		});

		return isExisted;
	};

	$scope.searchMemberGroup = function () {
		commonFactory.searchMemberGroup($scope, $scope.members, $scope.searchData.queryData);
	};

	$scope.add = function () {
		var employee = $scope.member.employee;

		if (employee) {
			$scope.criteria.sysIds = employee.UserId;
			var isExisted = checkExistMember($scope.members, employee.UserId),
				userId = employee.UserId;

			if (isExisted) {
				notificationFactory.error("ExistMemberWarning");
			}
			else {
				groupControlFactory.assignMemberToGroup($scope.criteria)
				.success(function (response) {
					if (response.IsSuccessful) {
						notificationFactory.success("DataChangeSuccessfully");
						getMembersOfGroup($scope.query, userId);
					}
					else {
						notificationFactory.error("", response.Message);
					}
				}).error(function (response, status) {
					ErrorFactory.errorHandle(status);
				});
			}

			$scope.member.employee = '';
			$scope.$broadcast('clearSearchMemberInput');
		}
	};

	/*
	** Delete member of group.
	*/
	$scope.deleteMember = function (member) {
		$scope.modalInstance = $modal.open({
			templateUrl: baseUrl + 'src/components/extensions/common/templates/ModalConfirmContent2.html',
			controller: "ModalConfirmController2",
			backdrop: 'static',
			resolve: {
				content: function () {
					return $scope.removeMessage;
				}
			},
			windowClass: "modal-confirm-center"
		});

		$scope.modalInstance.result.then(function () {
			manageUserInGroupFactory.deleteMemberGroup(member.SysId, $scope.group.GroupId)
			.success(function (response) {
				if (response.IsSuccessful) {
					var index = $scope.members.indexOf(member);
					$scope.members.splice(index, 1);
					$scope.searchMemberGroup($scope.members);
					notificationFactory.success("DataChangeSuccessfully");
				}
				else {
					notificationFactory.error("", response.Message);
				}
			}).error(function (response, status) {
				ErrorFactory.errorHandle(status);
			});
		})['finally'](function () {
			$scope.modalInstance = '';
		});
	};

	$scope.cancel = function () {
		$modalInstance.dismiss("cancel");
	};
} ]);
