var bField_Grid = function ($scope, field) {
    var initialPaging = 0;
    var oldTotalCount;
    var paging;
    var fieldName = field.FieldName;
    var pageIndexKey = 'bGridPageIndex_' + field.FieldID;
    var pageSizeKey = 'bGridPageSize_' + field.FieldID;

    this.init = function () {
        $scope.gridPageSizes = [10, 20, 30, 50, 100];

        var pageIndex = localStorage.getItem(pageIndexKey);
        if (pageIndex) field.Settings.PageIndex = parseInt(pageIndex);

        var pageSize = localStorage.getItem(pageSizeKey);
        if (pageSize) field.Settings.PageSize = parseInt(pageSize);

        $scope.$watch('Field.' + field.FieldName + '.Options', function (newValue, oldValue) {
            if (newValue != oldValue) {
                if (field.Settings.EnablePaging && field.Settings.PageSize) {
                    field.Settings.PageIndex = field.Settings.PageIndex || 1;
                    field.Settings.PageCount = Math.ceil(parseInt(field.Settings.TotalCount) / field.Settings.PageSize);

                    if (oldTotalCount != field.Settings.TotalCount) {
                        if (paging) $('#paging' + fieldName).twbsPagination('destroy');
                        if (field.Settings.TotalCount) {
                            setTimeout(function () {
                                renderPaging();
                            });
                        }
                    }

                    oldTotalCount = field.Settings.TotalCount;
                }
            }
        }, true);

        if (field.Settings.ListName) {
            $scope.$watch(field.Settings.ListName, function (newValue, oldValue) {
                if (newValue != oldValue) {
                    field.Options = newValue;
                    field.Settings.TotalCount = $scope.service.getScopePropertyValue($scope, field.Settings.TotalCountIn);

                    if (field.Settings.EnablePaging && field.Settings.PageSize) {
                        field.Settings.PageIndex = field.Settings.PageIndex || 1;
                        field.Settings.PageCount = Math.ceil(parseInt(field.Settings.TotalCount) / field.Settings.PageSize);

                        if (oldTotalCount != field.Settings.TotalCount) {
                            if (paging) $('#paging' + fieldName).twbsPagination('destroy');
                            if (field.Settings.TotalCount) {
                                setTimeout(function () {
                                    renderPaging();
                                });
                            }
                        }

                        oldTotalCount = field.Settings.TotalCount;
                    }
                }
            }, true);
        }
    }

    $scope.bGrid_onPageChange = function (field) {
        $.grep($scope.allActions, function (a) { return a.ActionID == field.Settings.DataSource.ActionID }).map(function (action) {
            $scope.callAction(action.ActionName, { PageIndex: field.Settings.PageIndex, PageSize: field.Settings.PageSize, TotalCount: field.Settings.TotalCount });
        });

        localStorage.setItem(pageIndexKey, field.Settings.PageIndex);
    };

    $scope.bGrid_onPageSizeChange = function (field, pageSize) {
        field.Settings.PageSize = pageSize;
        field.Settings.PageIndex = 1;

        initialPaging = 0;

        oldTotalCount = 0;

        if (paging) $('#pagingDataGrid').twbsPagination('destroy');

        $scope.bGrid_onPageChange(field);

        localStorage.setItem(pageSizeKey, field.Settings.PageSize);
    };

    function renderPaging() {
        paging = $('#paging' + fieldName).twbsPagination({
            totalPages: field.Settings.PageCount,
            visiblePages: 5,
            startPage: field.Settings.PageIndex || 0,
            pageClass: 'page-item',
            first: 'First',
            prev: 'Previous ',
            next: 'Next',
            last: 'Last',
            onPageClick: function (event, page) {
                if (++initialPaging > 1) {
                    $scope.$apply(function () {
                        field.Settings.PageIndex = page;
                        $scope.bGrid_onPageChange(field);
                    });
                }
            }
        });
    }

    $scope.onShowRowDetailsClick = function (field, item, $index) {
        if (!item.IsExpanded) {
            item.IsExpanded = true;

            if (!$('#bGridRowDetails' + $index).length) {
                var html = $('#rowDetails_' + field.FieldName).html();
                html = html.replace('{RowIndex}', $index).replace(/{RowItem}/g, 'rowItem' + $index);

                $scope['rowItem' + $index] = item;

                var $rowElement = $scope.$compile($(html))($scope);
                $rowElement.insertAfter($('#bGridRow' + $index + '_' + field.FieldName));
            }
        }
        else {
            item.IsExpanded = false;
        }
    };
}
