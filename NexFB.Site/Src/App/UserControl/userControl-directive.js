'use strict';
/*global userControlModule*/

// Assign Permission to Role
userControlModule.directive('assignPermission', function () {
    var updateParent = function (checked, type) {
        var pid = $(this).attr('pid');

        if (pid && pid.length > 0) {
            var parent = $("#func" + type + pid);

            if (checked && !parent.is(':checked')) {
                parent.prop('checked', true);
            }

            if (!checked) {
                var siblings = $("[name='func" + type + "'][pid='" + parent.val() + "']:checked");

                if (!siblings || (siblings && siblings.length === 0)) {
                    parent.prop('checked', false);
                }
            }

            updateParent.call(parent.get(0), checked, type);
        }
    };

    return {
        restrict: "A",
        link: function (scope, iElement) {
            iElement.on("change", function () {
                var permissionType = $(this).attr('name'),
                    currentId = $(this).val(),
                    checked = $(this).is(':checked');

                if (permissionType === 'funcHost') {
                    if (checked) {
                        $("#funcRead" + currentId).prop('checked', true).trigger('change');
                    }

                    // parent changed -> children changed
                    $("[name='funcHost'][pid='" + currentId + "']").prop('checked', checked).trigger('change');

                    // update parent
                    updateParent.call(this, checked, "Host");
                }
                else {
                    if (!checked) {
                        $("#funcHost" + currentId).prop('checked', false).trigger('change');
                    }

                    // parent changed -> children changed
                    $("[name='funcRead'][pid='" + currentId + "']").prop('checked', checked).trigger('change');

                    // update parent
                    updateParent.call(this, checked, "Read");
                }
            });
        }
    };
});

// get role permission when click save button
userControlModule.directive('getRolePermission', function () {
    var createTemplate = function (functionId, isRead, isHost) {
        var template = '<Function FunctionId="' + functionId + '"' +
            ' IsRead="' + isRead + '"' +
            ' IsHost="' + isHost + '"></Function>';
        return template;
    };

    return {
        restrict: "A",
        link: function (scope, iElement) {
            iElement.on("click", function () {
                var result = '<ROOT>';
                var table = $("[name='tableFunction']");
                var roleFunction = $(table).find('[type=checkbox][name=funcRead]');

                if (roleFunction) {
                    for (var i = 0; i < roleFunction.length; i++) {
                        var element = roleFunction[i];
                        var functionId = $(element).val(),
                            isRead = $(element).is(':checked') ? 1 : 0,
                            isHost = $('#funcHost' + functionId).is(':checked') ? 1 : 0,
                            template = createTemplate(functionId, isRead, isHost);
                        result += template;
                    }

                    result += '</ROOT>';

                    scope.roleFunction = {
                        functions: result,
                        roleId: scope.filter.roleId
                    };

                    // call save function
                    scope.save.call(this, scope.roleFunction);
                }
            });
        }
    };
});

// assign permission to user
userControlModule.directive('assignUserPermission', function () {
    return {
        restrict: "A",
        link: function (scope, iElement) {
            iElement.on("change", function () {
                var currentId = $(this).val(),
                    checked = $(this).is(':checked'),
                    parentId = $(this).attr('pid');

                if (parentId) {
                    var parent = $('#role' + parentId);

                    if (checked && !parent.is(':checked')) {
                        parent.prop('checked', true);
                    } else {
                        var siblings = $("[name='company'][pid='" + parentId + "']:checked");

                        if (!siblings || (siblings && siblings.length === 0)) {
                            parent.prop('checked', false);
                        }
                    }
                } else if (currentId) {
                    $("[name='company'][pid='" + currentId + "']").prop('checked', checked);
                }
            });
        }
    };
});

// get user permission when click save button
userControlModule.directive('getUserPermission', function () {
    var getCompanyTemplate = function (roleDiv, roleId) {
        var result = '',
            template = '<Row RoleId="' + roleId + '"',
            companies = $(roleDiv).find("[name='company'][pid='" + roleId + "']:checked");

        if (companies) {
            for (var i = 0; i < companies.length; i++) {
                var company = companies[i],
                    companyId = $(company).val(),
                    companyTemplate = template + ' CompanyId="' + companyId + '"/>';
                result = result !== '' ? result + companyTemplate : companyTemplate;
            }
        }

        return result;
    };

    return {
        restrict: "A",
        link: function (scope, iElement) {
            iElement.on("click", function () {
                var roleDiv = $("[name='userRoles']"),
                    roleItems = $(roleDiv).find("[name='role']:checked"),
                    roles = '',
                    roleCompanies = '<ROOT>',
                    isFreeOrder = $('#free-order').is(':checked');

                if (roleItems) {
                    for (var i = 0; i < roleItems.length; i++) {
                        var item = roleItems[i],
                            roleId = $(item).val();

                        roles = roles !== '' ? roles + ',' + roleId : roleId;

                        // get company of role if it has
                        var companyTemplate = getCompanyTemplate(roleDiv, roleId);
                        roleCompanies += companyTemplate;
                    }
                }

                roleCompanies += '</ROOT>';

                // call save function
                scope.save.call(this, roles, roleCompanies, isFreeOrder);
            });
        }
    };
});