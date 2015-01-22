'use strict';
/*global manageUserInGroupModule*/

manageUserInGroupModule.controller("ManageUserInGroupController",
['$scope', '$modal', '$route', '$log', '$routeParams', 'commonFactory', 'manageUserInGroupFactory',
'manageUserInGroupUtilities', 'notificationFactory', 'ErrorFactory', 'baseUrl', 'groupControlFactory',
'foodMenuFactory',
function ($scope, $modal, $route, $log, $routeParams, commonFactory, manageUserInGroupFactory,
manageUserInGroupUtilities, notificationFactory, ErrorFactory, baseUrl, groupControlFactory,
foodMenuFactory) {
	var groupId = parseInt($routeParams.groupId, 10);

	var originalFilter = {
		DepartmentId: '',
		GroupId: '',
		EmployeeTypeId: '',
		JoinedDateFrom: '',
		JoinedDateTo: ''
	};

	$scope.filter = angular.copy(originalFilter);
	$scope.filter.GroupId = isNaN(groupId) ? '' : groupId;
	$scope.groupOptions = [];

	$scope.calendar = {
		startDateOpened: false
	};

	/*
	** Open calendar.
	*/
	$scope.openPopupStartDate = function ($event) {
		commonFactory.openPopupStartDate($event, $scope);
	};

	$scope.openPopupEndDate = function ($event) {
		commonFactory.openPopupEndDate($event, $scope);
	};

	/*
	** Paging.
	*/
	$scope.showEntries = function () {
		commonFactory.tablePagination($scope, $scope.members, $scope.entrySelected);
	};

	$scope.entrySelected = 50;
	$scope.originalPage = 0;
	commonFactory.numberEntryList($scope);

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
	** Get user permission on food menu.
	*/
	manageUserInGroupFactory.getUserPermissionOnManageUserInGroup().success(function (permission) {
		$scope.policy = {
			permission: permission
		};
	}).error(function (response, status) {
		ErrorFactory.errorHandle(status);
	});

	/*
	** Get all company for drop down.
	*/
	manageUserInGroupFactory.getAllDepartment().success(function (companies) {
		if (companies) {
			$scope.companyOptions = companies;
		}
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

	/*
	** Get all groups.
	*/
	groupControlFactory.getGroups($scope.filter).success(function (groups) {
		$scope.groupOptions = groups;
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

	/*
	** Get all employee type.
	*/
	var getEmployeeType = function (departmentId, isLocal) {
		foodMenuFactory.getAllEmployeeType(departmentId, isLocal).success(function (employeeTypes) {
			$scope.employeeTypeOptions = employeeTypes;
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	/*
	** Get employee type by department.
	*/
	$scope.getEmployeeTypeOfDepartment = function (departmentId) {
		getEmployeeType(departmentId, null);
	};

	getEmployeeType(null, null);

	var getMembers = function (filter) {
		manageUserInGroupFactory.getMembers(filter).success(function (members) {
			if (members) {
				$scope.members = members;
				commonFactory.tablePagination($scope, $scope.members, $scope.entrySelected);
				$scope.currentPage = $scope.originalPage;
			}
		}).error(function () {
			notificationFactory.error("ErrorMessage");
		});
	};

	getMembers($scope.filter);

	/*
	** Filter data.
	*/
	$scope.query = function (filter) {
		if (filter) {
			var customizeFilter = angular.copy(filter),
				isValid = true;

			customizeFilter.GroupId = manageUserInGroupUtilities.getSelectedGroupIds($scope.groupOptions);
			customizeFilter.EmployeeTypeId = manageUserInGroupUtilities.getEmployeeTypeId($scope.employeeTypeOptions);

			if (customizeFilter.JoinedDateFrom && customizeFilter.JoinedDateTo) {
				var fromDate = new Date(customizeFilter.JoinedDateFrom),
					toDate = new Date(customizeFilter.JoinedDateTo);

				if (fromDate > toDate) {
					notificationFactory.error("DateRangeInvalid");
					isValid = false;
				}
				else {
					customizeFilter.JoinedDateFrom = customizeFilter.JoinedDateFrom.toDateString();
					customizeFilter.JoinedDateTo = customizeFilter.JoinedDateTo.toDateString();
				}
			}

			if (isValid) {
				$scope.customizeFilter = customizeFilter;
				getMembers(customizeFilter);
			}
		}
	};

	/*
	** Reset all checkbox member when user paging.
	*/
	$scope.$watch(
		function () {
			return $scope.currentPage;
		},
		function (newValue, oldValue) {
			if (newValue !== oldValue) {
				$scope.$broadcast('resetCheckAllMember');
			}
	});

	$scope.$watch(
		function () {
			return $scope.entrySelected;
		},
		function (newValue, oldValue) {
			if (newValue !== oldValue) {
				$scope.$broadcast('resetCheckAllMember');
			}
		});

	/*
	** Assign group to member.
	*/
	var getAssignedGroup = function (sysId, members) {
		var assignedGroup = '';

		if (members) {
			for (var i = 0; i < members.length; i++) {
				var member = members[i];

				if (member.SysId.toString() === sysId) {
					return member.AssignedGroups;
				}
			}

		}
		return assignedGroup;
	};

	$scope.assignGroupToMember = function (sysIds) {
		if (sysIds) {
			$scope.originalPage = $scope.currentPage;
			var assignedGroups = '';

			if (sysIds.search(',') === -1) {
				assignedGroups = getAssignedGroup(sysIds, $scope.members);
			}

			$scope.modalInstance = $modal.open({
				templateUrl: baseUrl + 'src/app/ManageUserInGroup/Templates/AssignGroupToMember.html',
				controller: "AsignGroupToMemberController",
				resolve: {
					sysIds: function () {
						return sysIds;
					},
					assignedGroups: function () {
						return assignedGroups;
					}
				},
				backdrop: 'static',
				windowClass: "modal-confirm-center assign-group-modal"
			});

			$scope.modalInstance.result.then(function () {
				getMembers($scope.customizeFilter);
				$scope.$broadcast('resetCheckAllMember');
			}
			)['finally'](function () {
				$scope.modalInstance = '';
			});
		}
		else {
			notificationFactory.error("NonMemberSelectedWarning");
		}
	};
} ]);

manageUserInGroupModule.controller('AsignGroupToMemberController',
['$scope', 'sysIds', '$modalInstance', 'groupControlFactory', 'notificationFactory', 'manageUserInGroupFactory',
'ErrorFactory', 'manageUserInGroupUtilities', 'assignedGroups',
function ($scope, sysIds, $modalInstance, groupControlFactory, notificationFactory, manageUserInGroupFactory,
ErrorFactory, manageUserInGroupUtilities, assignedGroups) {

	$scope.sysIds = sysIds;
	$scope.assignedGroups = assignedGroups;

	var checkAssignedGroup = function () {
		angular.forEach($scope.groupOptions, function (groupOption) {
			angular.forEach($scope.assignedGroups, function (assignedGroup) {
				if (groupOption.GroupId === assignedGroup.GroupId) {
					groupOption.ticked = true;
				}
			});
		});
	};

	/*
	** Get all groups.
	*/
	groupControlFactory.getGroups($scope.filter).success(function (groups) {
		$scope.groupOptions = groups;

		if ($scope.assignedGroups) {
			checkAssignedGroup();
		}
	}).error(function () {
		notificationFactory.error("ErrorMessage");
	});

	/*
	** Assign group to member.
	*/
	$scope.save = function () {
		var groupIds = manageUserInGroupUtilities.getSelectedGroupIds($scope.groupOptions),
			filter = {
				sysIds: $scope.sysIds,
				groupIds: groupIds
			};

		manageUserInGroupFactory.assignGroupToMember(filter).success(function (response) {
			if (response.IsSuccessful) {
				notificationFactory.success("DataChangeSuccessfully");
				$modalInstance.close();
			}
			else {
				notificationFactory.error("", response.Message);
			}
		}).error(function (response, status) {
			ErrorFactory.errorHandle(status);
		});
	};

	$scope.cancel = function () {
		$modalInstance.dismiss('cancel');
	};
            if (member.IsEditing) {
                var selectedGroups = manageUserInGroupUtilities.getSelectedGroups(member.GroupOptions);
                editingMembers.push(member);
                editingGroupOptions.push(selectedGroups);

                sysIds.push(member.SysId);
                groupIds.push(manageUserInGroupUtilities.getGroupIds(selectedGroups).join(','));
            }
        });

        filter.SysIds = sysIds.join(';');
        filter.GroupIds = groupIds.join(';');

        manageUserInGroupFactory.assignMemberToGroup(filter)
            .success(function () {
                $scope.globalMember.IsEditing = false;
                $scope.globalMember.HasEditing = false;
                $scope.globalMember.AssignedGroups = [];
                $scope.globalMember.GroupOptions = angular.copy(originalGroupOptions);

                var length = editingMembers.length;
                for (var i = 0; i < length; i++) {
                    editingMembers[i].IsEditing = false;
                    editingMembers[i].AssignedGroups = editingGroupOptions[i];
                }
            }).error(function () {
                notificationFactory.error("ErrorMessage");
            });
    };

    /*####### End: Assign Multiple Member ####*/

    $scope.selectMember = function (SysId) {
        $scope.selectedMember = SysId;
    };

    $scope.resetMember = function () {
        var members = $scope.pagedItems[$scope.previousPage];
        angular.forEach(members, function (member) {
            if (member.IsEditing) {
                member.IsEditing = false;
                member.GroupOptions = angular.copy(member.BackupGroupOptions);
            }
        });

        $scope.HasEditing = false;
        $scope.globalMember.IsEditing = false;
        $scope.globalMember.GroupOptions = angular.copy($scope.globalMember.BackupGroupOptions);
        $scope.previousPage = $scope.currentPage;
    };

    $scope.$watch('currentPage', function () {
        var current = parseInt($scope.currentPage);
        if (!isNaN(current)) {
            $scope.resetMember();
        }
    });
} ]);