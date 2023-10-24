var bCloudZoomGallerySettingFunction = function ($scope, field) {
    this.init = function () {
    }

    $scope.onShowGridColumnsClick = function () {
        $('#wnFieldSettings').modal('show');

        $scope.$broadcast('reInitialEditor');
    };

    $scope.onAddColumnClick = function () {
        $scope.column = angular.copy({ Contents: [{ Content: '' }] });

        $('#wnEditColumn').modal('show');
    };

    $scope.onEditColumnClick = function (column) {
        $scope.oldColumn = column;
        $scope.column = angular.copy(column);
        $scope.column.IsEdit = true;

        $('#wnEditColumn').modal('show');
    };

    $scope.onSwapColumnClick = function (from, to) {
        var temp = $scope.currentField.Settings.Columns[from];
        $scope.currentField.Settings.Columns[from] = $scope.currentField.Settings.Columns[to];
        $scope.currentField.Settings.Columns[to] = temp;
    };

    $scope.onDeleteColumnClick = function ($index) {
        $scope.currentField.Settings.Columns.splice($index, 1);
    };

    $scope.onSaveColumnClick = function () {
        if ($scope.column.IsEdit)
            $scope.currentField.Settings.Columns[$scope.currentField.Settings.Columns.indexOf($scope.oldColumn)] = $scope.column;
        else
            $scope.currentField.Settings.Columns.push($scope.column);

        delete $scope.column;
        delete $scope.oldColumn;

        $('#wnEditColumn').modal('hide');
    };

    $scope.onColumnActionChange = function () {
        var index = $scope.column.ContentscurrentRow || 0;

        if ($scope.columnAction == 'updateItem') {
            $scope.columnContentUpdateActions = angular.copy([]);
            $('#wnColumnContentUpdateActions').modal('show');
        }
        else {
            var content = $scope.column.Contents[index].Content ? $scope.column.Contents[index].Content : '';
            $scope.column.Contents[index].Content = content + $scope.columnAction;
        }

        delete $scope.columnAction;
    };

    $scope.onColumnContentConditionsClick = function (content) {
        $scope.columnContentConditions = angular.copy(content.Conditions || []);
        $('#wnColumnContentConditions').modal('show');
    };

    $scope.onSaveColumnContentConditionClick = function () {
        var conditions = $filter('filter')($scope.columnContentConditions, function (c) { return !c.IsHide });

        var index = $scope.column.ContentscurrentRow || 0;
        $scope.column.Contents[index].Conditions = conditions;

        $('#wnColumnContentConditions').modal('hide');

        delete $scope.columnContentConditions;
    };

    $scope.onupdateItemActionsClick = function (content) {
        $scope.columnContentUpdateActions = angular.copy(content.UpdateItemActions);
        $('#wnColumnContentUpdateActions').modal('show');
    };

    $scope.onSaveColumnContentUpdateActionsClick = function () {
        var actions = $filter('filter')($scope.columnContentUpdateActions, function (c) { return !c.IsHide });

        var index = $scope.column.ContentscurrentRow || 0;
        $scope.column.Contents[index].UpdateItemActions = actions;

        $scope.column.Contents[index].Content = '<a href="" {Action:UpdateItem}>Update Item</a>';

        $('#wnColumnContentUpdateActions').modal('hide');

        delete $scope.columnContentConditions;
    };

    $scope.onSaveSettingsClick = function () {
        $scope.onSaveFieldClick();

        $('#wnGridColumns').modal('hide');
    };

    if (field.Settings.DataSource) {
        $.grep($scope.actions, function (a) { return a.ActionID == field.Settings.DataSource.ActionID }).map(function (action) {
            $.grep($scope.services, function (s) { return s.ServiceID == action.ServiceID }).map(function (service) {
                if (service.Settings && service.Settings.Model && service.Settings.Model.Columns) field.DataModelColumns = service.Settings.Model.Columns;
            });
        });
    }
}
