'use strict';
/*global reportModule*/

reportModule.directive('viewMenuReport', function () {
    var generateHeaderRow = function (dates) {
        var columnHeader = [];
        var menusLength = dates.length
        if (dates && menusLength > 0) {
            columnHeader.push({ HeaderText: "#", Width: 40, Name: "No", HeaderAlign: "center", CellAlign: "center" });

            angular.forEach(dates, function (item, index) {
                var dateString = new Date(item);
                var date = {
                    HeaderText: dateString.toString("dd MMM, yyyy"),
                    Width: menusLength > 4 ? 230 : 500,
                    Name: "Date" + index,
                    HeaderAlign: "center",
                    CellAlign: "center"
                };
                columnHeader.push(date);
            });
        } else {
            columnHeader.push({
                HeaderText: "No information",
                Width: 1160,
                Name: "Date",
                HeaderAlign: "center",
                CellAlign: "center"
            });
        }
        return columnHeader;
    }

    var generateMenuTypeRows = function (data) {
        var rows = [];
        angular.forEach(data.Menus, function (item) {
            var currentRow = {};

            if (data.Menus.length > 0) {
                var iconClass = "pull-left ";
                iconClass += item.IsVegetarian === true ? "icon-veg" : "icon-none-veg";
                var menuClass = item.IsVegetarian === true ? "menu-vegetarian" : "menu-non-vegetarian";

                currentRow = {
                    Columns: [
                        {
                            Name: "No",
                            Value: '<div class="' + iconClass + '"></div>' + item.MenuName,
                            MergeColNumber: data.Dates.length + 1,
                            CellAlign: "left",
                            Key: item.MenuName
                        }
                    ],
                    SubRows: [],
                    CssClass: menuClass
                };
                rows.push(currentRow);
            }
        });
        return rows;
    };

    var getMenuItems = function (rowKey, menu) {
        var rows = [];
        for (var i = 0; i < menu.MaxMenuItemPerMeal; i++) {
            var columns = [{ Name: "No", Value: i + 1 }];
            angular.forEach(menu.MenuForEachDay, function (item, index) {
                var menuClass = "";
                if (item.Menus[i] && item.Menus[i].IsVegetarian) {
                    menuClass = "menu-vegetarian";
                }
                var menuItem = {
                    Name: "Date" + index,
                    Value: item.Menus[i] ? item.Menus[i].FoodVNMName + "</br>" + "<i class='english-text'>" + item.Menus[i].FoodEngName + "</i>" : "",
                    CellAlign: "left",
                    CssClass: menuClass
                }
                columns.push(menuItem);
            });
            rows.push(columns);
        }
        return rows;
    }

    var getMenuItemByMenuName = function (menuName, menus) {
        var menuItems = [];
        angular.forEach(menus, function (menu) {
            if (menu.MenuName === menuName) {
                menuItems.push(menu);
            }
        });
        return menuItems;
    };

    var generateBodyRows = function (data) {
        var rows = [];

        if (data.Menus) {
            rows = generateMenuTypeRows(data);
            var menuItems = [];
            angular.forEach(rows, function (rowTitle) {
                menuItems = getMenuItemByMenuName(rowTitle.Columns[0].Key, data.Menus);
                var items = getMenuItems(rowTitle.Columns[0].Key, menuItems[0]);
                angular.forEach(items, function (row) {
                    var subRow = {
                        Columns: row
                    }
                    rowTitle.SubRows.push(subRow);
                });
            });
        }
        return rows;
    };

    return {
        link: function (scope, element) {
            scope.$on('renderViewMenuReport', function () {
                var bodyRows = generateBodyRows(scope.reportData);
                var columnHeaders = generateHeaderRow(scope.reportData.Dates);
                var title = "Menu from " + scope.criteria.fromDate.toString("dd MMM, yyyy") + " to " + scope.criteria.toDate.toString("dd MMM, yyyy");

                var gridOptions = {
                    columns: columnHeaders,
                    bodyRows: bodyRows,
                    showTree: true,
                    showTitle: true,
                    titleInfo: {
                        text: title,
                        showExcelButton: true,
                        exportExcelFunction: function () {
                            scope.exportExcel.call(this);
                        }
                    },
                    gridExpandHeight: 250
                };

                $(element).fanexGrid(gridOptions);
            });
        }
    };
});