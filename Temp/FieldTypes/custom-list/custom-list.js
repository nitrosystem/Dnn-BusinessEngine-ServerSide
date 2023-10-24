var bField_CustomList = function ($scope, field) {
    var initialPaging = 0;
    var itemTemplate = '';
    var listName = '';
    var oldTotalCount;
    var paging;
    var fieldName = field.FieldName;

    this.init = function () {
        if (field.Settings.ItemTemplate) {
            itemTemplate = field.Settings.ItemTemplate;
        }

        if (field.Settings.DataSource) {
            $.grep($scope.actions, function (a) { return a.ActionID == field.Settings.DataSource.ActionID }).map(function (action) {
                listName = /{(\w+)}$/.exec(action.ActionDetails.ResultName)[1];
            });
        }

        if (!field.IsShow) {
            $scope.$watch('Field.' + field.FieldName + '.IsShow', function (newVal, oldVal) {
                if (newVal != oldVal && newVal) {
                    setTimeout(function () {
                        renderList(field.Options);
                    }, 1000);
                }
            });
        }
    }

    $scope.$watch('Field.' + field.FieldName + '.Options', function (newVal, oldVal) {
        if (newVal != oldVal) {
            renderList(newVal);
        }
    });

    function renderList(newVal) {
        $('#bCustomList' + field.FieldName).html('<style type="text/css">' + (field.Settings.CustomCss ? field.Settings.CustomCss : '') + '</style> \n ');

        for (var i = 0; i < newVal.length; i++) {
            var template = itemTemplate.replace(/{Item([\\.]?.[^}]+)?}/gm, '{' + listName + '[' + i + ']$1}');
            template = template.replace(/{ListItem([\\.]?.[^}]+)?}/gm, listName + '[' + i + ']$1');
            $('#bCustomList' + field.FieldName).append($scope.$compile(template)($scope));
        }

        //var $list = $('<div ng-repeat="Item in ' + listName + '"></div>');
        //$list.append(itemTemplate);
        //$('#bCustomList' + field.FieldName).append($scope.$compile($list)($scope));

        if (field.Settings.EnablePaging && field.Settings.PageSize) {
            field.Settings.PageIndex = field.Settings.PageIndex || 1;
            field.Settings.PageCount = Math.ceil(parseInt(field.Settings.TotalCount) / field.Settings.PageSize);

            if (oldTotalCount != field.Settings.TotalCount) {
                field.Settings.PageIndex = 1;

                if (paging) $('#paging' + fieldName).twbsPagination('destroy');

                setTimeout(function () {
                    renderPaging();
                });
            }

            oldTotalCount = field.Settings.TotalCount;
        }
    }

    $scope.bCustomList_onPageChange = function () {
        $.grep($scope.actions, function (a) { return a.ActionID == field.Settings.DataSource.ActionID }).map(function (action) {
            $scope.callAction(action.ActionName, { PageIndex: field.Settings.PageIndex, PageSize: field.Settings.PageSize, TotalCount: field.Settings.TotalCount });
        });
    };

    $scope.bCustomList_onPageSizeChange = function (pageSize) {
        field.Settings.PageSize = pageSize;
        field.Settings.PageIndex = 1;

        initialPaging = 0;

        $scope.bCustomList_onPageChange();
    };

    function renderPaging() {
        paging = $('#paging' + field.FieldName).twbsPagination({
            totalPages: field.Settings.PageCount,
            visiblePages: 5,
            pageClass: 'page-item',
            first: 'اولین',
            prev: 'قبلی',
            next: 'بعدی',
            last: 'آخرین',
            onPageClick: function (event, page) {
                if (++initialPaging > 1) {
                    $scope.$apply(function () {
                        field.Settings.PageIndex = page;
                        $scope.bCustomList_onPageChange();
                    });
                }
            }
        });
    }
}
